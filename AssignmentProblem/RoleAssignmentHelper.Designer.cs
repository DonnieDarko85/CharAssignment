namespace AssignmentProblem
{
	partial class RoleAssignmentHelper
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.MainTable = new System.Windows.Forms.DataGridView();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.AddAgentButton = new System.Windows.Forms.Button();
            this.AddRoleLabel = new System.Windows.Forms.Label();
            this.AddPlayerLabel = new System.Windows.Forms.Label();
            this.LoadButton = new System.Windows.Forms.Button();
            this.CalculateLabel = new System.Windows.Forms.Label();
            this.CalculateButton = new System.Windows.Forms.Button();
            this.LoadSaveLabel = new System.Windows.Forms.Label();
            this.AddNamelessTaskButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.ModifyTaskButton = new System.Windows.Forms.Button();
            this.DeleteTaskButton = new System.Windows.Forms.Button();
            this.ModifyAgentButton = new System.Windows.Forms.Button();
            this.DeleteAgentButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.OptionsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ImportPGButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.DummyFillButton = new System.Windows.Forms.Button();
            this.ImportPrefButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MainTable)).BeginInit();
            this.SuspendLayout();
            // 
            // MainTable
            // 
            this.MainTable.AllowUserToOrderColumns = true;
            this.MainTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MainTable.Location = new System.Drawing.Point(12, 106);
            this.MainTable.Name = "MainTable";
            this.MainTable.Size = new System.Drawing.Size(1326, 607);
            this.MainTable.TabIndex = 0;
            this.MainTable.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainTable_ColumnHeaderMouseClick);
            this.MainTable.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.MainTable_RowHeaderMouseClick);
            this.MainTable.SelectionChanged += new System.EventHandler(this.MainTable_SelectionChanged);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(1025, 40);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(100, 25);
            this.SaveButton.TabIndex = 1;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // AddTaskButton
            // 
            this.AddTaskButton.Location = new System.Drawing.Point(201, 10);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(131, 25);
            this.AddTaskButton.TabIndex = 2;
            this.AddTaskButton.Text = "Add role";
            this.AddTaskButton.UseVisualStyleBackColor = true;
            this.AddTaskButton.Click += new System.EventHandler(this.AddJobButton_Click);
            // 
            // AddAgentButton
            // 
            this.AddAgentButton.Location = new System.Drawing.Point(548, 10);
            this.AddAgentButton.Name = "AddAgentButton";
            this.AddAgentButton.Size = new System.Drawing.Size(131, 25);
            this.AddAgentButton.TabIndex = 3;
            this.AddAgentButton.Text = "Add player";
            this.AddAgentButton.UseVisualStyleBackColor = true;
            this.AddAgentButton.Click += new System.EventHandler(this.AddAgentButton_Click);
            // 
            // AddRoleLabel
            // 
            this.AddRoleLabel.AutoSize = true;
            this.AddRoleLabel.Location = new System.Drawing.Point(9, 13);
            this.AddRoleLabel.Name = "AddRoleLabel";
            this.AddRoleLabel.Size = new System.Drawing.Size(155, 52);
            this.AddRoleLabel.TabIndex = 4;
            this.AddRoleLabel.Text = "1. Add all roles.\r\n    Names for Roles are optional\r\n\r\n    + button adds unnamed " +
    "role";
            // 
            // AddPlayerLabel
            // 
            this.AddPlayerLabel.AutoSize = true;
            this.AddPlayerLabel.Location = new System.Drawing.Point(349, 12);
            this.AddPlayerLabel.Name = "AddPlayerLabel";
            this.AddPlayerLabel.Size = new System.Drawing.Size(169, 78);
            this.AddPlayerLabel.TabIndex = 5;
            this.AddPlayerLabel.Text = "2. Add all players\r\n    Including their preferences\r\n    and invalid choices (wro" +
    "ng age)\r\n\r\n    + button adds random player\r\n    for testing purposes";
            // 
            // LoadButton
            // 
            this.LoadButton.Location = new System.Drawing.Point(1025, 10);
            this.LoadButton.Name = "LoadButton";
            this.LoadButton.Size = new System.Drawing.Size(100, 25);
            this.LoadButton.TabIndex = 6;
            this.LoadButton.Text = "Load";
            this.LoadButton.UseVisualStyleBackColor = true;
            this.LoadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // CalculateLabel
            // 
            this.CalculateLabel.AutoSize = true;
            this.CalculateLabel.Location = new System.Drawing.Point(685, 12);
            this.CalculateLabel.Name = "CalculateLabel";
            this.CalculateLabel.Size = new System.Drawing.Size(103, 26);
            this.CalculateLabel.TabIndex = 7;
            this.CalculateLabel.Text = "3. Press to calculate\r\n    best assignment";
            // 
            // CalculateButton
            // 
            this.CalculateButton.Location = new System.Drawing.Point(794, 10);
            this.CalculateButton.Name = "CalculateButton";
            this.CalculateButton.Size = new System.Drawing.Size(100, 84);
            this.CalculateButton.TabIndex = 8;
            this.CalculateButton.Text = "Calculate";
            this.CalculateButton.UseVisualStyleBackColor = true;
            this.CalculateButton.Click += new System.EventHandler(this.CalculateButton_Click);
            // 
            // LoadSaveLabel
            // 
            this.LoadSaveLabel.AutoSize = true;
            this.LoadSaveLabel.Location = new System.Drawing.Point(912, 13);
            this.LoadSaveLabel.Name = "LoadSaveLabel";
            this.LoadSaveLabel.Size = new System.Drawing.Size(104, 26);
            this.LoadSaveLabel.TabIndex = 9;
            this.LoadSaveLabel.Text = "Load and save table\r\nfrom/to .XML file";
            // 
            // AddNamelessTaskButton
            // 
            this.AddNamelessTaskButton.Location = new System.Drawing.Point(170, 10);
            this.AddNamelessTaskButton.Name = "AddNamelessTaskButton";
            this.AddNamelessTaskButton.Size = new System.Drawing.Size(25, 25);
            this.AddNamelessTaskButton.TabIndex = 10;
            this.AddNamelessTaskButton.Text = "+";
            this.AddNamelessTaskButton.UseVisualStyleBackColor = true;
            this.AddNamelessTaskButton.Click += new System.EventHandler(this.AddJobNamelessButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(12, 716);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 13);
            this.StatusLabel.TabIndex = 11;
            // 
            // ModifyTaskButton
            // 
            this.ModifyTaskButton.Enabled = false;
            this.ModifyTaskButton.Location = new System.Drawing.Point(201, 41);
            this.ModifyTaskButton.Name = "ModifyTaskButton";
            this.ModifyTaskButton.Size = new System.Drawing.Size(131, 25);
            this.ModifyTaskButton.TabIndex = 12;
            this.ModifyTaskButton.Text = "Rename role";
            this.ModifyTaskButton.UseVisualStyleBackColor = true;
            this.ModifyTaskButton.Click += new System.EventHandler(this.ModifyTaskButton_Click);
            // 
            // DeleteTaskButton
            // 
            this.DeleteTaskButton.Enabled = false;
            this.DeleteTaskButton.Location = new System.Drawing.Point(201, 72);
            this.DeleteTaskButton.Name = "DeleteTaskButton";
            this.DeleteTaskButton.Size = new System.Drawing.Size(131, 25);
            this.DeleteTaskButton.TabIndex = 13;
            this.DeleteTaskButton.Text = "Delete role";
            this.DeleteTaskButton.UseVisualStyleBackColor = true;
            this.DeleteTaskButton.Click += new System.EventHandler(this.DeleteTaskButton_Click);
            // 
            // ModifyAgentButton
            // 
            this.ModifyAgentButton.Enabled = false;
            this.ModifyAgentButton.Location = new System.Drawing.Point(548, 41);
            this.ModifyAgentButton.Name = "ModifyAgentButton";
            this.ModifyAgentButton.Size = new System.Drawing.Size(131, 25);
            this.ModifyAgentButton.TabIndex = 14;
            this.ModifyAgentButton.Text = "Modify player";
            this.ModifyAgentButton.UseVisualStyleBackColor = true;
            this.ModifyAgentButton.Click += new System.EventHandler(this.ModifyAgentButton_Click);
            // 
            // DeleteAgentButton
            // 
            this.DeleteAgentButton.Enabled = false;
            this.DeleteAgentButton.Location = new System.Drawing.Point(548, 72);
            this.DeleteAgentButton.Name = "DeleteAgentButton";
            this.DeleteAgentButton.Size = new System.Drawing.Size(131, 25);
            this.DeleteAgentButton.TabIndex = 15;
            this.DeleteAgentButton.Text = "Delete player";
            this.DeleteAgentButton.UseVisualStyleBackColor = true;
            this.DeleteAgentButton.Click += new System.EventHandler(this.DeleteAgentButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(517, 10);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(25, 25);
            this.button3.TabIndex = 16;
            this.button3.Text = "+";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.AddRandomAgentButton_Click);
            // 
            // OptionsButton
            // 
            this.OptionsButton.Location = new System.Drawing.Point(1025, 70);
            this.OptionsButton.Name = "OptionsButton";
            this.OptionsButton.Size = new System.Drawing.Size(100, 25);
            this.OptionsButton.TabIndex = 17;
            this.OptionsButton.Text = "Options";
            this.OptionsButton.UseVisualStyleBackColor = true;
            this.OptionsButton.Click += new System.EventHandler(this.OptionsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(912, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 26);
            this.label1.TabIndex = 18;
            this.label1.Text = "Set DataGrid and\r\ncalculation options";
            // 
            // ImportPGButton
            // 
            this.ImportPGButton.Location = new System.Drawing.Point(1133, 10);
            this.ImportPGButton.Name = "ImportPGButton";
            this.ImportPGButton.Size = new System.Drawing.Size(100, 25);
            this.ImportPGButton.TabIndex = 19;
            this.ImportPGButton.Text = "Import PG";
            this.ImportPGButton.UseVisualStyleBackColor = true;
            this.ImportPGButton.Click += new System.EventHandler(this.ImportPGButton_Click);
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(1238, 70);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(100, 25);
            this.ExportButton.TabIndex = 20;
            this.ExportButton.Text = "Export Result";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // DummyFillButton
            // 
            this.DummyFillButton.Location = new System.Drawing.Point(1133, 70);
            this.DummyFillButton.Name = "DummyFillButton";
            this.DummyFillButton.Size = new System.Drawing.Size(100, 25);
            this.DummyFillButton.TabIndex = 21;
            this.DummyFillButton.Text = "Dummy fill";
            this.DummyFillButton.UseVisualStyleBackColor = true;
            this.DummyFillButton.Click += new System.EventHandler(this.DummyFillButton_Click);
            // 
            // ImportPrefButton
            // 
            this.ImportPrefButton.Location = new System.Drawing.Point(1133, 39);
            this.ImportPrefButton.Name = "ImportPrefButton";
            this.ImportPrefButton.Size = new System.Drawing.Size(100, 25);
            this.ImportPrefButton.TabIndex = 22;
            this.ImportPrefButton.Text = "Import Pref.";
            this.ImportPrefButton.UseVisualStyleBackColor = true;
            this.ImportPrefButton.Click += new System.EventHandler(this.ImportPrefButton_Click);
            // 
            // RoleAssignmentHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 738);
            this.Controls.Add(this.ImportPrefButton);
            this.Controls.Add(this.DummyFillButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.ImportPGButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OptionsButton);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.DeleteAgentButton);
            this.Controls.Add(this.ModifyAgentButton);
            this.Controls.Add(this.DeleteTaskButton);
            this.Controls.Add(this.ModifyTaskButton);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.AddNamelessTaskButton);
            this.Controls.Add(this.LoadSaveLabel);
            this.Controls.Add(this.CalculateButton);
            this.Controls.Add(this.CalculateLabel);
            this.Controls.Add(this.LoadButton);
            this.Controls.Add(this.AddPlayerLabel);
            this.Controls.Add(this.AddRoleLabel);
            this.Controls.Add(this.AddAgentButton);
            this.Controls.Add(this.AddTaskButton);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.MainTable);
            this.Name = "RoleAssignmentHelper";
            this.Text = "Role Assignment Tool";
            ((System.ComponentModel.ISupportInitialize)(this.MainTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.DataGridView MainTable;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.Button AddTaskButton;
		private System.Windows.Forms.Button AddAgentButton;
		private System.Windows.Forms.Label AddRoleLabel;
		private System.Windows.Forms.Label AddPlayerLabel;
		private System.Windows.Forms.Button LoadButton;
		private System.Windows.Forms.Label CalculateLabel;
		private System.Windows.Forms.Button CalculateButton;
		private System.Windows.Forms.Label LoadSaveLabel;
		private System.Windows.Forms.Button AddNamelessTaskButton;
		private System.Windows.Forms.Label StatusLabel;
		private System.Windows.Forms.Button ModifyTaskButton;
		private System.Windows.Forms.Button DeleteTaskButton;
		private System.Windows.Forms.Button ModifyAgentButton;
		private System.Windows.Forms.Button DeleteAgentButton;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button OptionsButton;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ImportPGButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button DummyFillButton;
        private System.Windows.Forms.Button ImportPrefButton;
	}
}

