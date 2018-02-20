using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using MatrixCalculations;
using Microsoft.VisualBasic.FileIO;

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
 * Copyright (c) 2016 Dennis Schwarz  ( DRSNova@gmx.de )
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
namespace AssignmentProblem
{
	/* TODO: 

		- Preferences: step pref/disl, multiplier default, grey out cost step when flat, step two values! pref/disl
		- Load/Save preferences
		- Edit player
		- print results in text window
		- auto color range  
  	*/
	public partial class RoleAssignmentHelper : Form
	{
		#region constants

	    private readonly Color[] _cellColors =
	    {
	        Color.Green,
	        Color.LightGreen,
	        Color.GreenYellow,
	        Color.Yellow,
	        Color.Orange,
	        Color.OrangeRed,
	        Color.Red,
	        Color.DarkRed
	    };
		#endregion

		#region variables

        /// <summary>
        /// CSV separator, use comma per default options
        /// </summary>
        private static readonly string _separator = ",";

		/// <summary>
		/// List containing all tasks, identified by GUID
		/// </summary>
		//private List<Tuple<Guid, string>> _tasks = new List<Tuple<Guid, string>>();
		private List<Task> _tasks = new List<Task>();

        /// <summary>
        /// Result of ssignment computation
        /// </summary>
	    private List<int> _assignment = null;

		/// <summary>
		/// List containing all agents, identified by GUID
		/// including preferences (ordered by most to least preferred)
		/// and impossible tasks (in no particular order)
		/// </summary>
		//private List<Tuple<Guid, string, List<Guid>, List<Guid>>> _agents = new List<Tuple<Guid, string, List<Guid>, List<Guid>>>();
		private List<Agent> _agents = new List<Agent>();

		/// <summary>
		/// The Main Data Grid that shows all agent-task-costs and the final result
		/// </summary>
		private DataGrid _mainTable = new DataGrid();

		/// <summary>
		/// Cost of non-preferred tasks is this factor times #maximum preferences
		/// </summary>
		private int _nonPreferenceFactor = 30;

		/// <summary>
		/// Cost of impossible tasks is this factor times #maximum preferences
		/// </summary>
		private int _impossibleFactor = 1000;

        /// <summary>
        /// Preferences mapping
        /// </summary>
		private readonly Preferences _preferences = new Preferences();
		#endregion

		#region methods
		public RoleAssignmentHelper()
		{
			InitializeComponent();

			_preferences.Load();
			UpdateLabels();

			MainTable.AllowUserToDeleteRows = false;
			MainTable.AllowUserToAddRows = false;
			MainTable.ShowEditingIcon = false;
			MainTable.AllowUserToOrderColumns = false;
			MainTable.AllowDrop = false;
			MainTable.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
		}

		private void AddAgentButton_Click(object sender, EventArgs e)
		{
			var newAgent = Prompt.AgentPrompt("Enter " + _preferences.Agent + " name (optional):", "Add new " + _preferences.Agent, _tasks, _preferences);

			// Cancelled?
		    if (newAgent == null) return;

		    _agents.Add(newAgent);
		    RebuildGrid();
		}

        private static int _lastAgentUsedIndex;

		private void AddRandomAgentButton_Click(object sender, EventArgs e)
		{
			var rnd = new Random();
            var newAgent = new Agent("Giocatore " + _lastAgentUsedIndex++);
			Task currentTask;

			for (var i = 0; i < Math.Min(_preferences.MaxPreferences, _tasks.Count); i++)
			{
				do
				{
					var taskId = rnd.Next(_tasks.Count);
					currentTask = _tasks[taskId];
				}
				while (newAgent.PreferredTasks.Contains(currentTask));
				newAgent.AddPreferredTask(currentTask);
			}

			if (_tasks.Count > _preferences.MaxPreferences)
			{
				do
				{
                    var taskId = rnd.Next(_tasks.Count);
					currentTask = _tasks[taskId];
				}
				while (newAgent.PreferredTasks.Contains(currentTask) || newAgent.ImpossibleTasks.Contains(currentTask));
				newAgent.AddImpossibleTask(currentTask);
			}

			_agents.Add(newAgent);

			RebuildGrid();
		}

		/// <summary>
		/// Adds a new job to the table, ommitting a name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddJobNamelessButton_Click(object sender, EventArgs e)
		{
			AddJob();
		}

		/// <summary>
		/// Adds a new job to the table, prompting for a name
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddJobButton_Click(object sender, EventArgs e)
		{
		    var jobName = Prompt.SingleStringPrompt("Please enter a name for " + _preferences.Task + " # " + _tasks.Count + ":", "New " + _preferences.Task);

			// Cancel
			if (jobName == string.Empty)
			{
				return;
			}

			AddJob(jobName);
		}


		/// <summary>
		/// Adds new job to the table
		/// </summary>
		/// <param name="p_jobName">Optional name for the new job</param>
		private void AddJob(string p_taskName = "", string p_posId = "")
		{
			Task newTask = new Task(p_taskName);

            if (!string.IsNullOrEmpty(p_posId)){
                newTask.PgNumber = p_posId;
            }

			_tasks.Add(newTask);

			string statusMessage = "New " + _preferences.Task + " (# " + (_tasks.Count - 1).ToString() + ") added";
			if (p_taskName != string.Empty)
			{
				statusMessage += ", named '" + newTask.Name + "'";
			}

			SetStatus(statusMessage);

			RebuildGrid();
		}


		/// <summary>
		/// Prints a new message to the status bar
		/// </summary>
		/// <param name="p_newStatus">Status bar message</param>
		private void SetStatus(string p_newStatus)
		{
			StatusLabel.Text = p_newStatus;
		}


		/// <summary>
		/// Rebuild the DataGrid
		/// </summary>
		/// <param name="assignment">If assignment exists, show optimal assignment in DataGrid</param>
		private void RebuildGrid(List<int> assignment = null)
		{
			_mainTable = new DataGrid();

			MainTable.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
			MainTable.ClearSelection();

			// Setup table columns
			MainTable.ColumnCount = _tasks.Count;

			// Iterate over jobs and set column names and column ids within tasks
			for (var i = 0; i < _tasks.Count; i++)
			{
				_tasks[i].PosID = i;
                MainTable.Columns[i].Name = string.IsNullOrEmpty(MainTable.Columns[i].Name) ? _tasks[i].GetIDName() : _tasks[i].GetNumberAndName();
				MainTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			// Setup table rows
			MainTable.Rows.Clear();

			if (MainTable.ColumnCount > 0)
			{
				// Iterate over agents and set row names and show preferences/cost
				for (var i = 0; i < _agents.Count; i++)
				{
					var currentAgent = _agents[i];

					MainTable.Rows.Add();
					currentAgent.PosID = i;
					MainTable.Rows[i].HeaderCell.Value = currentAgent.GetIDName();

					// Colur all cells red by default, set higher cost value
					for (var j = 0; j < _tasks.Count; j++)
					{
						MainTable.Rows[i].Cells[j].Value = _nonPreferenceFactor;
                        MainTable.Rows[i].Cells[j].Style.ForeColor = Color.Black;
						MainTable.Rows[i].Cells[j].Style.BackColor = Color.White;
					}

					// Iterate over preferences to colour the DataGrid appropriately
					for (var prefId = 0; prefId < currentAgent.PreferredTasks.Count; prefId++)
					{
						var preference = currentAgent.PreferredTasks[prefId];
						var taskId = preference.PosID;

						MainTable.Rows[i].Cells[taskId].Value = (prefId + 1).ToString();

					    if (assignment != null) continue;

					    MainTable.Rows[i].Cells[taskId].Style.ForeColor = Color.Black;
					    MainTable.Rows[i].Cells[taskId].Style.BackColor = _cellColors[prefId >= _cellColors.Length ? _cellColors.Length - 1 : prefId];
					}

					// Iterate over preferences to colour the DataGrid appropriately
					foreach (var taskId in currentAgent.ImpossibleTasks.Select(impossible => impossible.PosID).Where(taskId => assignment == null))
					{
					    MainTable.Rows[i].Cells[taskId].Value = _impossibleFactor;
                        MainTable.Rows[i].Cells[taskId].Style.ForeColor = Color.White;
					    MainTable.Rows[i].Cells[taskId].Style.BackColor = Color.Black;
					}
				}

				// Show assignment
				if (assignment != null)
				{
					for (int rowID = 0; rowID < assignment.Count; rowID++)
					{
						int colID = assignment[rowID];

						MainTable.Rows[rowID].Cells[colID].Style.ForeColor = Color.White;
						MainTable.Rows[rowID].Cells[colID].Style.BackColor = Color.Blue;
					}
				}
			}

			MainTable.AutoResizeColumns();
			MainTable.AutoResizeRows();
			if (MainTable.Rows.Count > 0)
			{
				MainTable.AutoResizeRowHeadersWidth(0, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
			}
		}


		/// <summary>
		/// Loads current DataGrid configuration from disk
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void LoadButton_Click(object sender, EventArgs e)
		{
			string fileName = Prompt.OpenFileDialog();

			if (fileName != null)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(fileName);

				if (XMLToDataGrid(doc))
				{
					SetStatus("Loaded DataGrid configuration from '" + fileName + "'");
				}
				else
				{
					SetStatus("Error: unable to read DataGrid configuration from '" + fileName + "'");
				}
			}
		}


		/// <summary>
		/// Saves current DataGrid configuration to disk
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void SaveButton_Click(object sender, EventArgs e)
		{
			string fileName = Prompt.SaveFileDialog();

			if (fileName != null)
			{
				XmlDocument doc = DataGridToXML();
				doc.Save(fileName);
				SetStatus("DataGrid configuration saved to '" + fileName + "'");
			}
		}


		/// <summary>
		/// Reads DataGrid configuration from XML document
		/// </summary>
		/// <param name="dataGridXML"></param>
		/// <returns></returns>
		private bool XMLToDataGrid(XmlDocument dataGridXML)
		{
			_tasks = new List<Task>();
			_agents = new List<Agent>();

			// Parse tasks
			XmlNodeList tasks = dataGridXML.SelectNodes("//Assignment/Tasks/Task");
			foreach (XmlNode node in tasks)
			{
				string taskName = node.Attributes["Name"].Value;
				_tasks.Add(new Task(taskName));
			}

			// Parse agents
			XmlNodeList agents = dataGridXML.SelectNodes("//Assignment/Agents/Agent");
			foreach (XmlNode node in agents)
			{
				string agentName = node.Attributes["Name"].Value;
				Agent agent = new Agent(agentName);

				// Parse preferred items
				XmlNodeList prefTasks = node.SelectNodes("PreferredTasks/Task");
				foreach (XmlNode prefNode in prefTasks)
				{
					int posID = Int32.Parse(prefNode.Attributes["PosID"].Value);
					agent.PreferredTasks.Add(_tasks[posID]);
				}

				// Parse impossible items
				XmlNodeList impTasks = node.SelectNodes("ImpossibleTasks/Task");
				foreach (XmlNode impNode in impTasks)
				{
					int posID = Int32.Parse(impNode.Attributes["PosID"].Value);
					agent.ImpossibleTasks.Add(_tasks[posID]);
				}

				_agents.Add(agent);
			}

			RebuildGrid();

			return true;
		}


		/// <summary>
		/// Creates XML document from DataGrid configuration
		/// </summary>
		/// <returns></returns>
		private XmlDocument DataGridToXML()
		{
			// Create new XML document
			XmlDocument dataGridXML = new XmlDocument();

			// Create root node
			XmlElement newRoot = dataGridXML.CreateElement("Assignment");
			dataGridXML.AppendChild(newRoot);

			// Create tasks elememt
			XmlElement tasks = dataGridXML.CreateElement("Tasks");
			newRoot.AppendChild(tasks);

			// Iterate over tasks, create task nodes
			foreach (Task task in _tasks)
			{
				XmlElement taskXML = dataGridXML.CreateElement("Task");

				XmlAttribute taskName = dataGridXML.CreateAttribute("Name");
				taskName.Value = task.Name;
				taskXML.Attributes.Append(taskName);

				tasks.AppendChild(taskXML);
			}

			// Create agents element
			XmlElement agents = dataGridXML.CreateElement("Agents");
			newRoot.AppendChild(agents);

			// Iterate over agents, create agent nodes
			foreach (Agent agent in _agents)
			{
				XmlElement agentXML = dataGridXML.CreateElement("Agent");

				XmlAttribute agentName = dataGridXML.CreateAttribute("Name");
				agentName.Value = agent.Name;
				agentXML.Attributes.Append(agentName);

				agents.AppendChild(agentXML);

				// Create preferred tasks node
				XmlElement preferredtasks = dataGridXML.CreateElement("PreferredTasks");
				agentXML.AppendChild(preferredtasks);

				// Iterate over preferred tasks, add them to document
				foreach (Task task in agent.PreferredTasks)
				{
					XmlElement taskXML = dataGridXML.CreateElement("Task");

					XmlAttribute taskName = dataGridXML.CreateAttribute("PosID");
					taskName.Value = task.PosID.ToString();
					taskXML.Attributes.Append(taskName);

					preferredtasks.AppendChild(taskXML);
				}

				// Create impossible tasks node
				XmlElement impossibletasks = dataGridXML.CreateElement("ImpossibleTasks");
				agentXML.AppendChild(impossibletasks);

				// Iterate over preferred tasks, add them to document
				foreach (Task task in agent.ImpossibleTasks)
				{
					XmlElement taskXML = dataGridXML.CreateElement("Task");

					XmlAttribute taskName = dataGridXML.CreateAttribute("PosID");
					taskName.Value = task.PosID.ToString();
					taskXML.Attributes.Append(taskName);

					impossibletasks.AppendChild(taskXML);
				}
			}

			return dataGridXML;
		}

		private void MainTable_SelectionChanged(object sender, EventArgs e)
		{
			// Row selected?
			if (MainTable.SelectedRows.Count == 1)
			{
				DeleteAgentButton.Enabled = true;
				ModifyAgentButton.Enabled = true;
			}
			else
			{
				DeleteAgentButton.Enabled = false;
				ModifyAgentButton.Enabled = false;
			}

			// Column selected?
			if (MainTable.SelectedColumns.Count == 1)
			{
				DeleteTaskButton.Enabled = true;
				ModifyTaskButton.Enabled = true;
			}
			else
			{
				DeleteTaskButton.Enabled = false;
				ModifyTaskButton.Enabled = false;
			}
		}

		/// <summary>
		/// Erases agent
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteAgentButton_Click(object sender, EventArgs e)
		{
			if (MainTable.SelectedRows.Count == 1)
			{
				int index = MainTable.SelectedRows[0].Index;

				// Erase agent from agent list
				Agent agentToRemove = _agents.ElementAt(index);

				if (agentToRemove != null)
				{
					_agents.RemoveAt(index);

					string statusMessage = "Removed " + _preferences.Agent + " # " + agentToRemove.GetIDName();
					SetStatus(statusMessage);

					RebuildGrid();
				}
			}
		}

		/// <summary>
		/// Rename task
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ModifyTaskButton_Click(object sender, EventArgs e)
		{
			if (MainTable.SelectedColumns.Count == 1)
			{
				int index = MainTable.SelectedColumns[0].Index;

				// Rename Task
				Task taskToRename = _tasks.ElementAt(index);
				string oldName = taskToRename.Name;

				string newName = Prompt.SingleStringPrompt("Enter new name for " + _preferences.Task + ":", "Rename task", oldName);
				if (newName != string.Empty)
				{
					taskToRename.Name = newName;
					RebuildGrid();
				}
			}
		}


		/// <summary>
		/// Erases role
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeleteTaskButton_Click(object sender, EventArgs e)
		{
			if (MainTable.SelectedColumns.Count == 1)
			{
				int index = MainTable.SelectedColumns[0].Index;

				// Erase agent from agent list
				Task taskToRemove = _tasks.ElementAt(index);

				if (taskToRemove != null)
				{
					_tasks.RemoveAt(index);

					// Iterate over agents, remove task from all lists
					foreach (Agent agent in _agents)
					{
						agent.RemoveTask(taskToRemove);
					}

					string statusMessage = "Removed " + _preferences.Task + " # " + taskToRemove.GetIDName();
					SetStatus(statusMessage);

					RebuildGrid();
				}
			}
		}

		/// <summary>
		/// Starts calculating optimal assignment and gives results both in the DataGrid as well as a text popup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void CalculateButton_Click(object sender, EventArgs e)
		{
			// Check if Matrix is n x n
			if (_agents.Count < _tasks.Count)
			{
				SetStatus("Error: Too many " + _preferences.Task + "s for number of " + _preferences.Agent + "s!");
				return;
			}
			if (_agents.Count > _tasks.Count)
			{
				SetStatus("Error: Too many " + _preferences.Agent + "s for number of " + _preferences.Task + "s!");
				return;
			}

			var matrixSize = _agents.Count;

		    _assignment = null;
			// Read matrix from DataGrid
			/*var costs = new List<int>();

			foreach (DataGridViewRow row in MainTable.Rows)
			{
				foreach (DataGridViewCell cell in row.Cells)
				{
					costs.Add(int.Parse(cell.Value.ToString()));
				}
			}*/

            //LINK-Q solution
            var costs = (from DataGridViewRow row in MainTable.Rows from DataGridViewCell cell in row.Cells select int.Parse(cell.Value.ToString())).ToList();

			// Start matrix calculation
			var mm = new MatrixMath(matrixSize, costs);
			_assignment = mm.Calculate();

			// Show result in DataGrid
            RebuildGrid(_assignment);
		}

		/// <summary>
		/// Selects a column
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
		    if (e.Button != MouseButtons.Left) return;

		    MainTable.ClearSelection();
		    MainTable.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
		    MainTable.Columns[e.ColumnIndex].Selected = true;

		    MainTable_SelectionChanged(sender, e);
		}


		/// <summary>
		/// Select a row
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainTable_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
		    if (e.Button != MouseButtons.Left) return;

		    MainTable.ClearSelection();
		    MainTable.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
		    MainTable.Rows[e.RowIndex].Selected = true;

		    MainTable_SelectionChanged(sender, e);
		}

		#endregion

		/// <summary>
		/// Changes the program preferences
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OptionsButton_Click(object sender, EventArgs e)
		{
		    if (!Prompt.ChangePreferences(_preferences)) return;

		    _preferences.Save();

		    UpdateLabels();
		}

        /// <summary>
        /// Add dummy player to match number of players with number of characters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DummyFillButton_Click(object sender, EventArgs e)
        {
            int difference;
            if ((difference = _tasks.Count - _agents.Count) == 0) return;

            for (var i = 0; i < difference; i++)
            {
                var newAgent = new Agent("dummy");
                _agents.Add(newAgent);
                RebuildGrid();
            }
        }

		private void UpdateLabels()
		{
			// Labels
			AddRoleLabel.Text = "1. Add all " + _preferences.Task + "s.\n\n   Names for " + _preferences.Task + "s are optional\n\n    + button adds unnamed " + _preferences.Task;
			AddPlayerLabel.Text = "2. Add all " + _preferences.Agent + "s\n    Including their preferences\n    and invalid choices (wrong age)\n\n    +button adds random " + _preferences.Agent + "\n    for testing purposes";

			// Buttons
			AddTaskButton.Text = "Add " + _preferences.Task;
			ModifyTaskButton.Text = "Rename " + _preferences.Task;
			DeleteTaskButton.Text = "Delete " + _preferences.Task;

			AddAgentButton.Text = "Add " + _preferences.Agent;
			ModifyAgentButton.Text = "Modify " + _preferences.Agent;
			DeleteAgentButton.Text = "Delete " + _preferences.Agent;
		}

        private void ExportButton_Click(object sender, EventArgs e)
        {
            if (_assignment == null)
            {
                SetStatus("No assignment to save, please calculate it first!");
                return;
            }

            var fileName = Prompt.ExportFileDialog();
            if (fileName == null) return;

            ExportAssignment(fileName);
            SetStatus("Assignment saved to '" + fileName + "'");
        }

	    private void ExportAssignment(string fileName)
	    {
	        var assignmentOutput = string.Empty;
	        var toRemoveOutput = string.Empty;
           
            for (var i = 0; i < _agents.Count; i++)
	        {
	            var index = _assignment[i];

                if (_agents[i].Name.Equals("dummy")) continue;

                toRemoveOutput += _tasks[index].GetNumberAndName() + "\n";
	            assignmentOutput += _agents[i].Name + _separator + _tasks[index].GetNumberAndName() + _separator +
                                    (_agents[i].PreferredTasks.IndexOf(_tasks[index]) + 1) + "\n";
	        }

	        File.WriteAllText(fileName, assignmentOutput, Encoding.UTF8);
            File.WriteAllText(fileName.Replace(".csv", "") + "_assigned.csv", toRemoveOutput, Encoding.UTF8);
	    }

        private void ImportPGButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "txt files (*.txt)|*.txt",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            _tasks.Clear();
            _agents.Clear();

            var allPg = File.ReadAllLines(openFileDialog.FileName);

            foreach (var splitted in allPg.Select(pg => pg.Split('|')))
            {
                if (splitted.Length != 2)
                {
                    SetStatus("Bad format of file " + openFileDialog.FileName);
                    break;
                }
                AddJob(splitted[1].Trim(), splitted[0].Trim());
            }

            if (MainTable.Rows.Count > 0)
            {
                MainTable.AutoResizeRowHeadersWidth(0, DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders);
            }

            RebuildGrid();
        }

        private void ImportPrefButton_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            _agents.Clear();

            var parser = new TextFieldParser(openFileDialog.FileName)
            {
                TextFieldType = FieldType.Delimited
            };

            parser.SetDelimiters(",");
            //Discard first line
            parser.ReadLine();

            while (!parser.EndOfData)
            {
                //Processing row
                var fields = parser.ReadFields();
                if (fields == null) continue;

                //Create player
                var newAgent = new Agent(fields[0]);
                for (var i = 2; i < fields.Length; i++)
                {
                    var task = SplitAndFindTask(fields[i]);
                    if (task == null) continue;
                    //Preferences
                    if (i - 1 <= _preferences.MaxPreferences)
                    {
                        newAgent.AddPreferredTask(task);
                    }//Dislikes
                    else
                    {
                        newAgent.AddImpossibleTask(task);

                    }
                }
                //Add to set of agents the new created one
                _agents.Add(newAgent);
            }            

            //Rebuild for visualization
            RebuildGrid();
        }

        private Task SplitAndFindTask(string complexName)
        {
            var splitted = complexName.Split('|');
            if (splitted.Length == 2) return _tasks.Find(x => x.Equals(new Task(splitted[1].Trim())));

            //Error
            SetStatus("Bad format of imported file");
            return null;
        }

        private void ModifyAgentButton_Click(object sender, EventArgs e)
        {

        }
    }

   
}
