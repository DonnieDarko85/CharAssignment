using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssignmentProblem
{
	class Preferences
	{
		public const string c_defaultTask = "role";
		public const string c_defaultAgent = "player";
		public const int c_defaultMaxPreferences = 8;
		public const int c_defaultMaxDislikes = 0;
		public const int c_defaultMaxImpossible = 4;
		public const bool c_preferencesOrdered = true;
		public const bool c_dislikesOrdered = true;
		public const int c_costImpossible = 1000;
		public const int c_costStep = 1;
		public const int c_costStepAverage = 5;


		/// <summary>
		/// Display name of tasks
		/// </summary>
		public string Task
		{
			get;
			set;
		}

		/// <summary>
		///  Display names of agents
		/// </summary>
		public string Agent
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum number of preferences (can be 0)
		/// </summary>
		public int MaxPreferences
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum number of dislikes (can be 0)
		/// </summary>
		public int MaxDislikes
		{
			get;
			set;
		}

		/// <summary>
		/// Maximum number of impossible choices (can be 0)
		/// </summary>
		public int MaxImpossible
		{
			get;
			set;
		}

		/// <summary>
		/// Are preferences ordered or equal?
		/// </summary>
		public bool PreferencesOrdered
		{
			get;
			set;
		}

		/// <summary>
		/// Are dislikes ordered or equal?
		/// </summary>
		public bool DislikesOrdered
		{
			get;
			set;
		}

		/// <summary>
		/// Cost for impossible tasks
		/// </summary>
		public int CostImpossible
		{
			get;
			set;
		}

		/// <summary>
		/// Cost steps for preferences / dislikes
		/// </summary>
		public int CostStep
		{
			get;
			set;
		}

		/// <summary>
		/// Step cist for average tasks
		/// Example: Step = 2, Average = 3, #Pref/Dislike = 3
		///           |   pref   |  average  |   dislike   |
		/// Costs are   1, 3, 5,       8,      11, 13, 15
		/// </summary>
		public int CostStepAverage;

		/// <summary>
		/// Load preferences from XML file if exists, otherwise use defaults
		/// </summary>
		public void Load()
		{
			SetDefaultValues();
		}

		/// <summary>
		/// Save preferences to XML file
		/// </summary>
		public void Save()
		{

		}

		/// <summary>
		/// Set Default values
		/// </summary>
		private void SetDefaultValues()
		{
			Task = c_defaultTask;
			Agent = c_defaultAgent;
			MaxPreferences = c_defaultMaxPreferences;
			MaxDislikes = c_defaultMaxDislikes;
			MaxImpossible = c_defaultMaxImpossible;
			PreferencesOrdered = c_preferencesOrdered;
			DislikesOrdered = c_dislikesOrdered;
			CostImpossible = c_costImpossible;
			CostStep = c_costStep;
			CostStepAverage = c_costStepAverage;
		}
	}
}
