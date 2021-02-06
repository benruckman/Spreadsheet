namespace SpreadsheetGUI
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.spreadsheetPanel = new SS.SpreadsheetPanel();
            this.cellNameBox = new System.Windows.Forms.TextBox();
            this.CellValueBox = new System.Windows.Forms.TextBox();
            this.CellContentBox = new System.Windows.Forms.TextBox();
            this.CalculateValuesButton = new System.Windows.Forms.Button();
            this.CellNameLabel = new System.Windows.Forms.Label();
            this.CellValueLabel = new System.Windows.Forms.Label();
            this.CellContentLabel = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.OpenFileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveFileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.NewFileMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.QuitMenuButton = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // spreadsheetPanel
            // 
            this.spreadsheetPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel.Location = new System.Drawing.Point(12, 68);
            this.spreadsheetPanel.Name = "spreadsheetPanel";
            this.spreadsheetPanel.Size = new System.Drawing.Size(776, 370);
            this.spreadsheetPanel.TabIndex = 0;
            // 
            // cellNameBox
            // 
            this.cellNameBox.BackColor = System.Drawing.SystemColors.Info;
            this.cellNameBox.Location = new System.Drawing.Point(44, 42);
            this.cellNameBox.Name = "cellNameBox";
            this.cellNameBox.ReadOnly = true;
            this.cellNameBox.Size = new System.Drawing.Size(28, 20);
            this.cellNameBox.TabIndex = 1;
            this.cellNameBox.Text = "A1";
            // 
            // CellValueBox
            // 
            this.CellValueBox.BackColor = System.Drawing.SystemColors.Info;
            this.CellValueBox.Location = new System.Drawing.Point(78, 42);
            this.CellValueBox.Name = "CellValueBox";
            this.CellValueBox.ReadOnly = true;
            this.CellValueBox.Size = new System.Drawing.Size(115, 20);
            this.CellValueBox.TabIndex = 2;
            // 
            // CellContentBox
            // 
            this.CellContentBox.BackColor = System.Drawing.SystemColors.Info;
            this.CellContentBox.Location = new System.Drawing.Point(199, 42);
            this.CellContentBox.Name = "CellContentBox";
            this.CellContentBox.Size = new System.Drawing.Size(244, 20);
            this.CellContentBox.TabIndex = 3;
            // 
            // CalculateValuesButton
            // 
            this.CalculateValuesButton.Location = new System.Drawing.Point(449, 40);
            this.CalculateValuesButton.Name = "CalculateValuesButton";
            this.CalculateValuesButton.Size = new System.Drawing.Size(87, 23);
            this.CalculateValuesButton.TabIndex = 4;
            this.CalculateValuesButton.Text = "Enter Contents";
            this.CalculateValuesButton.UseVisualStyleBackColor = true;
            this.CalculateValuesButton.Click += new System.EventHandler(this.CalculateValuesButton_Click);
            // 
            // CellNameLabel
            // 
            this.CellNameLabel.AutoSize = true;
            this.CellNameLabel.Location = new System.Drawing.Point(32, 26);
            this.CellNameLabel.Name = "CellNameLabel";
            this.CellNameLabel.Size = new System.Drawing.Size(55, 13);
            this.CellNameLabel.TabIndex = 5;
            this.CellNameLabel.Text = "Cell Name";
            // 
            // CellValueLabel
            // 
            this.CellValueLabel.AutoSize = true;
            this.CellValueLabel.Location = new System.Drawing.Point(103, 26);
            this.CellValueLabel.Name = "CellValueLabel";
            this.CellValueLabel.Size = new System.Drawing.Size(54, 13);
            this.CellValueLabel.TabIndex = 6;
            this.CellValueLabel.Text = "Cell Value";
            // 
            // CellContentLabel
            // 
            this.CellContentLabel.AutoSize = true;
            this.CellContentLabel.Location = new System.Drawing.Point(292, 26);
            this.CellContentLabel.Name = "CellContentLabel";
            this.CellContentLabel.Size = new System.Drawing.Size(69, 13);
            this.CellContentLabel.TabIndex = 7;
            this.CellContentLabel.Text = "Cell Contents";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.HelpButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(800, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenFileMenuButton,
            this.SaveFileMenuButton,
            this.NewFileMenuButton,
            this.QuitMenuButton});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // OpenFileMenuButton
            // 
            this.OpenFileMenuButton.Name = "OpenFileMenuButton";
            this.OpenFileMenuButton.Size = new System.Drawing.Size(124, 22);
            this.OpenFileMenuButton.Text = "Open File";
            this.OpenFileMenuButton.Click += new System.EventHandler(this.OpenFileMenuButton_Click);
            // 
            // SaveFileMenuButton
            // 
            this.SaveFileMenuButton.Name = "SaveFileMenuButton";
            this.SaveFileMenuButton.Size = new System.Drawing.Size(124, 22);
            this.SaveFileMenuButton.Text = "Save File";
            this.SaveFileMenuButton.Click += new System.EventHandler(this.SaveFileMenuButton_Click);
            // 
            // NewFileMenuButton
            // 
            this.NewFileMenuButton.Name = "NewFileMenuButton";
            this.NewFileMenuButton.Size = new System.Drawing.Size(124, 22);
            this.NewFileMenuButton.Text = "New File";
            this.NewFileMenuButton.Click += new System.EventHandler(this.NewFileMenuButton_Click);
            // 
            // QuitMenuButton
            // 
            this.QuitMenuButton.Name = "QuitMenuButton";
            this.QuitMenuButton.Size = new System.Drawing.Size(124, 22);
            this.QuitMenuButton.Text = "Quit";
            this.QuitMenuButton.Click += new System.EventHandler(this.QuitMenuButton_Click);
            // 
            // HelpButton
            // 
            this.HelpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.HelpButton.Image = ((System.Drawing.Image)(resources.GetObject("HelpButton.Image")));
            this.HelpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpButton.Name = "HelpButton";
            this.HelpButton.Size = new System.Drawing.Size(36, 22);
            this.HelpButton.Text = "Help";
            this.HelpButton.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.CellContentLabel);
            this.Controls.Add(this.CellValueLabel);
            this.Controls.Add(this.CellNameLabel);
            this.Controls.Add(this.CalculateValuesButton);
            this.Controls.Add(this.CellContentBox);
            this.Controls.Add(this.CellValueBox);
            this.Controls.Add(this.cellNameBox);
            this.Controls.Add(this.spreadsheetPanel);
            this.Name = "Form1";
            this.Text = "Form1";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SS.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.TextBox cellNameBox;
        private System.Windows.Forms.TextBox CellValueBox;
        private System.Windows.Forms.TextBox CellContentBox;
        private System.Windows.Forms.Button CalculateValuesButton;
        private System.Windows.Forms.Label CellNameLabel;
        private System.Windows.Forms.Label CellValueLabel;
        private System.Windows.Forms.Label CellContentLabel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem OpenFileMenuButton;
        private System.Windows.Forms.ToolStripMenuItem SaveFileMenuButton;
        private System.Windows.Forms.ToolStripMenuItem NewFileMenuButton;
        private System.Windows.Forms.ToolStripMenuItem QuitMenuButton;
        private System.Windows.Forms.ToolStripButton HelpButton;
    }
}

