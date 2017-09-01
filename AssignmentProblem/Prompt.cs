using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

namespace AssignmentProblem
{
	class Prompt
	{
		public static string SingleStringPrompt(string text, string caption, string initialValue = "")
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
			TextBox textBox = new TextBox() { Left = 50, Top = 35, Width = 400 };
			Button confirmation = new Button() { Text = "Ok", Left = 240, Width = 100, Height = 25, Top = 70, DialogResult = DialogResult.OK };
			Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Height = 25, Top = 70, DialogResult = DialogResult.Cancel };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			cancel.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.Controls.Add(textLabel);
			prompt.AcceptButton = confirmation;

			textBox.Text = initialValue;
			textBox.Focus();

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : string.Empty;
		}

		public static string ChooseNamedEntityPrompt(string text, string caption, List<NamedEntity> entities)
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 150,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
			TextBox textBox = new TextBox() { Left = 50, Top = 35, Width = 400 };
			Button confirmation = new Button() { Text = "OK", Left = 240, Width = 100, Height = 25, Top = 70, DialogResult = DialogResult.OK };
			Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Height = 25, Top = 70, DialogResult = DialogResult.Cancel };
			confirmation.Click += (sender, e) => { prompt.Close(); };
			cancel.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.Controls.Add(textLabel);
			prompt.AcceptButton = confirmation;

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : null;
		}


		public static Agent AgentPrompt(string text, string caption, List<Task> p_tasks, Preferences p_preferences)
		{
			Agent agent = new Agent();

			Form prompt = new Form()
			{
				Width = 500,
				Height = 230 + p_preferences.MaxPreferences * 30 + p_preferences.MaxImpossible * 30,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};
			Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
			TextBox textBox = new TextBox() { Left = 50, Top = 35, Width = 400 };

			Label textLabelPref = new Label() { Left = 50, Top = 70, Width = 400, Text = "Preferences - in order from most to less preferred (max. " + p_preferences.MaxPreferences.ToString() + "):" };

			// Create drop-down combo boxes for preferences
			List<ComboBox> prefDropDowns = new List<ComboBox>();
			for (int i = 0; i < p_preferences.MaxPreferences; i++)
			{
				ComboBox newDropDown = new ComboBox() { Left = 50, Top = 90 + i * 30, Width = 400 };

				newDropDown.Items.Add("none");
				foreach (Task task in p_tasks)
				{
					newDropDown.Items.Add(task.GetNumberAndName());
				}
				newDropDown.SelectedIndex = 0;

				newDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
				prompt.Controls.Add(newDropDown);
				prefDropDowns.Add(newDropDown);
			}
			int topPosition = 80 + p_preferences.MaxPreferences * 30;

			Label textLabelImpos = new Label() { Left = 50, Top = topPosition + 20, Width = 400, Text = "Disliked/Impossible " + p_preferences.Task + "s - in no particular order (max. " + p_preferences.MaxImpossible.ToString() + "):" };

			// Create drop-down combo boxes for impossible tasks
			List<ComboBox> impDropDowns = new List<ComboBox>();
			for (int i = 0; i < p_preferences.MaxImpossible; i++)
			{
				ComboBox newDropDown = new ComboBox() { Left = 50, Top = topPosition + 45 + i * 30, Width = 400 };

				newDropDown.Items.Add("none");
				foreach (Task task in p_tasks)
				{
					newDropDown.Items.Add(task.GetNumberAndName());
				}
				newDropDown.SelectedIndex = 0;

				newDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
				prompt.Controls.Add(newDropDown);
				impDropDowns.Add(newDropDown);
			}

			topPosition += 45 + p_preferences.MaxImpossible * 30;

			Button confirmation = new Button() { Text = "Ok", Left = 240, Width = 100, Height = 34, Top = topPosition + 10, DialogResult = DialogResult.OK };
			Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Height = 34, Top = topPosition + 10, DialogResult = DialogResult.Cancel };

			confirmation.Click += (sender, e) =>
			{
				agent.Name = textBox.Text;
				prompt.Close();

				// Iterate over dropdowns to get preferences
				foreach (ComboBox dropDown in prefDropDowns)
				{
					if (dropDown.SelectedIndex != 0)
					{
						agent.PreferredTasks.Add(p_tasks[dropDown.SelectedIndex - 1]);
					}
				}

				// Iterate over dropdowns to get preferences
				foreach (ComboBox dropDown in impDropDowns)
				{
					if (dropDown.SelectedIndex != 0)
					{
						agent.ImpossibleTasks.Add(p_tasks[dropDown.SelectedIndex - 1]);
					}
				}
			};
			cancel.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(textBox);
			prompt.Controls.Add(confirmation);
			prompt.Controls.Add(cancel);
			prompt.Controls.Add(textLabel);
			prompt.Controls.Add(textLabelPref);
			prompt.Controls.Add(textLabelImpos);
			prompt.AcceptButton = confirmation;

			return prompt.ShowDialog() == DialogResult.OK ? agent : null;
		}

		public static string SaveFileDialog()
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = "xml files (*.xml)|*.xml";
			saveFileDialog.FilterIndex = 2;
			saveFileDialog.RestoreDirectory = true;

			if (saveFileDialog.ShowDialog() == DialogResult.OK)
			{
				return saveFileDialog.FileName;
			}

			return null;
		}

        public static string ExportFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "csv files (*.csv)|*.csv";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                return saveFileDialog.FileName;
            }

            return null;
        }

		public static string OpenFileDialog()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "xml files (*.xml)|*.xml";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				return openFileDialog.FileName;
			}

			return null;
		}

		/// <summary>
		/// Returns true if settings were changed
		/// </summary>
		/// <param name="p_preferences"></param>
		/// <returns></returns>
		public static bool ChangePreferences(Preferences p_preferences)
		{
			Form prompt = new Form()
			{
				Width = 500,
				Height = 600,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = "Set preferences",
				StartPosition = FormStartPosition.CenterScreen
			};

			Label chooseAgentTextLabel = new Label() { Left = 50, Top = 20, Width = 190, Text = "Display agents as..." };
			TextBox chooseAgentTextInput = new TextBox() { Left = 50, Top = 45, Width = 190, Text = p_preferences.Agent };
			prompt.Controls.Add(chooseAgentTextLabel);
			prompt.Controls.Add(chooseAgentTextInput);

			Label chooseTaskTextLabel = new Label() { Left = 250, Top = 20, Width = 190, Text = "Display tasks as..." };
			TextBox chooseTaskTextInput = new TextBox() { Left = 250, Top = 45, Width = 190, Text = p_preferences.Task };
			prompt.Controls.Add(chooseTaskTextLabel);
			prompt.Controls.Add(chooseTaskTextInput);


			Label maxNumPrefLabel = new Label() { Left = 50, Top = 80, Width = 190, Text = "Maximum number of preferences" };
			TextBox maxNumPrefInput = new TextBox() { Left = 50, Top = 105, Width = 190, Text = p_preferences.MaxPreferences.ToString() };
			prompt.Controls.Add(maxNumPrefLabel);
			prompt.Controls.Add(maxNumPrefInput);

			GroupBox prefTypeOptions = new GroupBox() { Left = 250, Top = 70, Width = 200, Height = 60 };
			RadioButton prefTypeOrdered = new RadioButton() { Width = 190 };
			prefTypeOrdered.Text = "Preferences strong -> weak";
			prefTypeOrdered.Location = new System.Drawing.Point(5, 10);
			prefTypeOptions.Controls.Add(prefTypeOrdered);
			RadioButton prefTypeEqual = new RadioButton() { Width = 190 };
			prefTypeEqual.Text = "All preferences equal";
			prefTypeEqual.Location = new System.Drawing.Point(5, 30);
			prefTypeOptions.Controls.Add(prefTypeEqual);
			if (p_preferences.PreferencesOrdered)
			{
				prefTypeOrdered.Checked = true;
			}
			else
			{
				prefTypeEqual.Checked = true;
			}
			prompt.Controls.Add(prefTypeOptions);


			Label maxNumDislikesLabel = new Label() { Left = 50, Top = 150, Width = 190, Text = "Maximum number of dislikes" };
			TextBox maxNumDislikesInput = new TextBox() { Left = 50, Top = 175, Width = 190, Text = p_preferences.MaxDislikes.ToString() };
			prompt.Controls.Add(maxNumDislikesLabel);
			prompt.Controls.Add(maxNumDislikesInput);

			GroupBox dislikeTypeOptions = new GroupBox() { Left = 250, Top = 140, Width = 200, Height = 60 };
			RadioButton dislikeTypeOrdered = new RadioButton() { Width = 190 };
			dislikeTypeOrdered.Text = "Dislikes strong -> weak";
			dislikeTypeOrdered.Location = new System.Drawing.Point(5, 10);
			dislikeTypeOptions.Controls.Add(dislikeTypeOrdered);
			RadioButton dislikeTypeEqual = new RadioButton() { Width = 190 };
			dislikeTypeEqual.Text = "All dislikes equal";
			dislikeTypeEqual.Location = new System.Drawing.Point(5, 30);
			dislikeTypeOptions.Controls.Add(dislikeTypeEqual);
			if (p_preferences.DislikesOrdered)
			{
				dislikeTypeOrdered.Checked = true;
			}
			else
			{
				dislikeTypeEqual.Checked = true;
			}
			prompt.Controls.Add(dislikeTypeOptions);


			Label maxNumImpossibleLabel = new Label() { Left = 50, Top = 220, Width = 190, Text = "Maximum number of impossible tasks" };
			TextBox maxNumImpossibleInput = new TextBox() { Left = 50, Top = 245, Width = 190, Text = p_preferences.MaxImpossible.ToString() };
			prompt.Controls.Add(maxNumImpossibleLabel);
			prompt.Controls.Add(maxNumImpossibleInput);

			Label costImpossibleLabel = new Label() { Left = 250, Top = 220, Width = 190, Text = "Cost for impossible tasks" };
			TextBox costImpossibleInput = new TextBox() { Left = 250, Top = 245, Width = 190, Text = p_preferences.CostImpossible.ToString() };
			prompt.Controls.Add(costImpossibleLabel);
			prompt.Controls.Add(costImpossibleInput);

			/*
			Label costStepLabel = new Label() { Left = 50, Top = 220, Width = 190, Text = "Maximum number of impossible tasks" };
			TextBox costStepInput = new TextBox() { Left = 50, Top = 245, Width = 190, Text = p_preferences.CostStep.ToString() };
			prompt.Controls.Add(costStepLabel);
			prompt.Controls.Add(costStepInput);

			Label costImpossibleLabel = new Label() { Left = 250, Top = 220, Width = 190, Text = "Cost for impossible tasks" };
			TextBox costImpossibleInput = new TextBox() { Left = 250, Top = 245, Width = 190, Text = p_preferences.CostImpossible.ToString() };
			prompt.Controls.Add(costImpossibleLabel);
			prompt.Controls.Add(costImpossibleInput);*/



			Button save = new Button() { Text = "Save", Left = 240, Width = 100, Height = 25, Top = 500, DialogResult = DialogResult.OK };
			save.Click += (sender, e) =>
			{
				p_preferences.Agent = chooseAgentTextInput.Text;
				p_preferences.Task = chooseTaskTextInput.Text;

				p_preferences.MaxPreferences = Int32.Parse(maxNumPrefInput.Text);
				p_preferences.PreferencesOrdered = prefTypeOrdered.Checked;

				p_preferences.MaxDislikes = Int32.Parse(maxNumDislikesInput.Text);
				p_preferences.DislikesOrdered = dislikeTypeOrdered.Checked;

				p_preferences.MaxImpossible = Int32.Parse(maxNumImpossibleInput.Text);
				p_preferences.CostImpossible = Int32.Parse(costImpossibleInput.Text);

				prompt.Close();

			};
			prompt.Controls.Add(save);

			Button cancel = new Button() { Text = "Cancel", Left = 350, Width = 100, Height = 25, Top = 500, DialogResult = DialogResult.Cancel };
			cancel.Click += (sender, e) => { prompt.Close(); };
			prompt.Controls.Add(cancel);


			prompt.AcceptButton = save;
			return (prompt.ShowDialog() == DialogResult.OK);
		}
	}
}
