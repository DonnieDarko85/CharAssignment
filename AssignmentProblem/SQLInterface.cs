using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/********************************************************************************
 *                        Assignment Problem Solver
 *                    -------------------------------
 * Use to solve a certain subset of the assignment problem: Have several people give     
 * preferences and/or dislikes, make this software assign the appropriate costs and     
 * calculate the best overall assignment for them.
 * The classes Matrix, MatrixElement, MatrixMath and Program can be used on their
 * own to solve any sort of assignment problem.
 *      
 ********************************************************************************      
 *      
 * MIT License
 *
 * Copyright (c) 2016 Dennis Schwarz  ( DennisSchwarzDev@gmx.de )
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * *******************************************************************************/
namespace MatrixCalculations
{
	/// <summary>
	/// Use this class to connect to your database to read preferences and users from it
	/// And write results to a table. Ideally, you only have to change the constants provided
	/// </summary>
	class SQLInterface
	{
		#region constants
		private const string c_connectionString = "<insert connection string here>";

		private const string c_preferenceTableName = "<insert preference table name here>";
		private const string c_castingTableName = "<insert casting table name here>";

		private const string c_larpID = "LarpID";
		private const string c_playerID = "PlayerID";
		private const string c_characterID = "CharacterID";
		private const string c_preference = "Preference";
		#endregion

		#region methods
		/// <summary>
		/// Run calculations for single larp
		/// </summary>
		/// <param name="p_larpID">ID of larp you are running the calculations for</param>
		/// <returns></returns>
		public static void RunCalculation(int p_larpID)
		{
			// IDs of players
			List<int> playerIDs = new List<int>();

			// IDs of roles
			List<int> characterIDs = new List<int>();

			// Array of characterID-player preference pairs, indexed by playerID
			List<Tuple<int, int, int>> preferences = new List<Tuple<int, int, int>>();

			// Read preferences from database
			using (SqlConnection sqlConnection = new SqlConnection(c_connectionString))
			{
				sqlConnection.Open();

				using (SqlCommand cmd = new SqlCommand())
				{
					cmd.CommandText = "SELECT (" + c_playerID + ", " + c_characterID + ", " + c_preference + " FROM " + c_preferenceTableName + " WHERE " + c_larpID + " = @larpID";
					cmd.Parameters.AddWithValue("larpID", p_larpID);
					cmd.CommandType = CommandType.Text;
					cmd.Connection = sqlConnection;

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						int playerIdOrd = reader.GetOrdinal(c_playerID);
						int characterIdOrd = reader.GetOrdinal(c_characterID);
						int preferenceIdOrd = reader.GetOrdinal(c_preference);

						while (reader.Read())
						{
							int playerID = reader.GetInt32(playerIdOrd);
							int characterID = reader.GetInt32(characterIdOrd);
							int preferenceId = reader.GetInt32(preferenceIdOrd);

							if (!playerIDs.Contains(playerID))
							{
								playerIDs.Add(playerID);
							}

							if (!characterIDs.Contains(characterID))
							{
								characterIDs.Add(characterID);
							}

							Tuple<int, int, int> newPreference = new Tuple<int, int, int>(playerID, characterID, preferenceId);
							preferences.Add(newPreference);
						}
					}

				}

				sqlConnection.Close();
			}

			// #CharacterIDs == #PlayerIDs?
			int id = -1;
			while (playerIDs.Count > characterIDs.Count)
			{
				// Add dummy character
				characterIDs.Add(id--);
			}
			id = -1;
			while (playerIDs.Count < characterIDs.Count)
			{
				// Add dummy player
				playerIDs.Add(id--);
			}

			// Build cost matrix
			List<int> costs = new List<int>();
			for (int i = 0; i < playerIDs.Count * characterIDs.Count; i++)
			{
				costs.Add(10);
			}

			// Iterate over preferences, write to cost matrix
			foreach (Tuple<int, int, int> preference in preferences)
			{
				int cost = 10;
				if (preference.Item3 == -1)
				{
					cost = 100;
				}
				else
				{
					// FLIP PREFERENCES! 5 -> 1, 1 -> 5, etc.
					cost = 5 - preference.Item3;
				}

				// Get ID of player in Matrix (index of player list)
				int playerIndex = playerIDs.IndexOf(preference.Item1);

				// Get ID of character in Matrix (index of character list)
				int characterIndex = characterIDs.IndexOf(preference.Item2);

				// Set cost
				costs[characterIndex + playerIndex * characterIDs.Count] = cost;
			}

			// START CALCULATION
			MatrixMath calc = new MatrixMath(characterIDs.Count, costs);
			List<int> rawAssignment = calc.Calculate();

			// Create assignment list 
			List<Tuple<int, int, int>> assignments = new List<Tuple<int, int, int>>();
			for (int i = 0; i < rawAssignment.Count; i++)
			{
				int playerID = playerIDs[i];
				int characterID = characterIDs[rawAssignment[i]];
				int originalPreference = 0;

				// Was Character preferred/disliked?
				foreach (Tuple<int, int, int> preference in preferences)
				{
					if (playerID == preference.Item1 && characterID == preference.Item2)
					{
						originalPreference = preference.Item3;
					}
				}

				assignments.Add(new Tuple<int, int, int>(playerID, characterID, originalPreference));
			}


			// Read preferences from database
			using (SqlConnection sqlConnection = new SqlConnection(c_connectionString))
			{
				sqlConnection.Open();

				// Clear casting table for larp
				using (SqlCommand cmd = new SqlCommand())
				{
					cmd.CommandText = "SELECT FROM " + c_castingTableName + " WHERE " + c_larpID + " = @larpID";
					cmd.Parameters.AddWithValue("larpID", p_larpID);
					cmd.CommandType = CommandType.Text;
					cmd.Connection = sqlConnection;

					cmd.ExecuteNonQuery();
				}

				foreach (Tuple<int, int, int> assignment in assignments)
				{
					// Create entries for casting table
					using (SqlCommand cmd = new SqlCommand())
					{
						cmd.CommandText = "INSERT INTO " + c_castingTableName + " (" + c_playerID + ", " + c_characterID + ", " + c_preference + ") Values (@larpID, @playerID, @characterId, @oldpreference);";
						cmd.Parameters.AddWithValue("larpID", p_larpID);
						cmd.Parameters.AddWithValue("playerID", assignment.Item1);
						cmd.Parameters.AddWithValue("characterId", assignment.Item2);
						cmd.Parameters.AddWithValue("oldpreference", assignment.Item3);
						cmd.CommandType = CommandType.Text;
						cmd.Connection = sqlConnection;

						cmd.ExecuteNonQuery();
					}
				}

				sqlConnection.Close();
			}
		}
		#endregion
	}
}
