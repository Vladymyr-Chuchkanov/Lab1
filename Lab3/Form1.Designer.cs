namespace Lab3
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
            this.LeftDirs = new System.Windows.Forms.ListBox();
            this.LeftFiles = new System.Windows.Forms.ListBox();
            this.RightDirs = new System.Windows.Forms.ListBox();
            this.RightFiles = new System.Windows.Forms.ListBox();
            this.LeftCombo = new System.Windows.Forms.ComboBox();
            this.RightCombo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Button();
            this.SearchHtmlByWords = new System.Windows.Forms.Button();
            this.SearchHtmlTxt = new System.Windows.Forms.TextBox();
            this.TransportBtt = new System.Windows.Forms.Button();
            this.ProcesseBtt = new System.Windows.Forms.Button();
            this.checkDeleteProcesse = new System.Windows.Forms.CheckBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DeleteBttLeft = new System.Windows.Forms.Button();
            this.DeleteBttRight = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.EditLeftBtt = new System.Windows.Forms.Button();
            this.EditRightBtt = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LeftDirs
            // 
            this.LeftDirs.FormattingEnabled = true;
            this.LeftDirs.ItemHeight = 16;
            this.LeftDirs.Location = new System.Drawing.Point(15, 77);
            this.LeftDirs.Name = "LeftDirs";
            this.LeftDirs.Size = new System.Drawing.Size(202, 468);
            this.LeftDirs.TabIndex = 0;
            this.LeftDirs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LeftDirs_MouseDoubleClick);
            // 
            // LeftFiles
            // 
            this.LeftFiles.FormattingEnabled = true;
            this.LeftFiles.ItemHeight = 16;
            this.LeftFiles.Location = new System.Drawing.Point(223, 77);
            this.LeftFiles.Name = "LeftFiles";
            this.LeftFiles.Size = new System.Drawing.Size(269, 468);
            this.LeftFiles.TabIndex = 1;
            this.LeftFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LeftFiles_MouseClick);
            // 
            // RightDirs
            // 
            this.RightDirs.FormattingEnabled = true;
            this.RightDirs.ItemHeight = 16;
            this.RightDirs.Location = new System.Drawing.Point(534, 77);
            this.RightDirs.Name = "RightDirs";
            this.RightDirs.Size = new System.Drawing.Size(202, 468);
            this.RightDirs.TabIndex = 2;
            this.RightDirs.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RightDirs_MouseDoubleClick);
            // 
            // RightFiles
            // 
            this.RightFiles.FormattingEnabled = true;
            this.RightFiles.ItemHeight = 16;
            this.RightFiles.Location = new System.Drawing.Point(742, 77);
            this.RightFiles.Name = "RightFiles";
            this.RightFiles.Size = new System.Drawing.Size(269, 468);
            this.RightFiles.TabIndex = 3;
            this.RightFiles.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.RightFiles_MouseClick);
            // 
            // LeftCombo
            // 
            this.LeftCombo.FormattingEnabled = true;
            this.LeftCombo.Location = new System.Drawing.Point(15, 558);
            this.LeftCombo.Name = "LeftCombo";
            this.LeftCombo.Size = new System.Drawing.Size(202, 24);
            this.LeftCombo.TabIndex = 6;
            this.LeftCombo.SelectedValueChanged += new System.EventHandler(this.LeftCombo_SelectedValueChanged);
            // 
            // RightCombo
            // 
            this.RightCombo.FormattingEnabled = true;
            this.RightCombo.Location = new System.Drawing.Point(534, 558);
            this.RightCombo.Name = "RightCombo";
            this.RightCombo.Size = new System.Drawing.Size(202, 24);
            this.RightCombo.TabIndex = 7;
            this.RightCombo.SelectedIndexChanged += new System.EventHandler(this.RightCombo_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(205, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "button1";
            this.label1.UseVisualStyleBackColor = true;
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(534, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(205, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "button1";
            this.label2.UseVisualStyleBackColor = true;
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // SearchHtmlByWords
            // 
            this.SearchHtmlByWords.Location = new System.Drawing.Point(12, 589);
            this.SearchHtmlByWords.Name = "SearchHtmlByWords";
            this.SearchHtmlByWords.Size = new System.Drawing.Size(205, 52);
            this.SearchHtmlByWords.TabIndex = 10;
            this.SearchHtmlByWords.Text = "Search html files:";
            this.SearchHtmlByWords.UseVisualStyleBackColor = true;
            this.SearchHtmlByWords.Click += new System.EventHandler(this.SearchHtmlByWords_Click);
            // 
            // SearchHtmlTxt
            // 
            this.SearchHtmlTxt.Location = new System.Drawing.Point(223, 604);
            this.SearchHtmlTxt.Name = "SearchHtmlTxt";
            this.SearchHtmlTxt.Size = new System.Drawing.Size(269, 22);
            this.SearchHtmlTxt.TabIndex = 11;
            // 
            // TransportBtt
            // 
            this.TransportBtt.Location = new System.Drawing.Point(12, 648);
            this.TransportBtt.Name = "TransportBtt";
            this.TransportBtt.Size = new System.Drawing.Size(205, 59);
            this.TransportBtt.TabIndex = 12;
            this.TransportBtt.Text = "Transport selected files ->";
            this.TransportBtt.UseVisualStyleBackColor = true;
            this.TransportBtt.Click += new System.EventHandler(this.TransportBtt_Click);
            // 
            // ProcesseBtt
            // 
            this.ProcesseBtt.Location = new System.Drawing.Point(15, 713);
            this.ProcesseBtt.Name = "ProcesseBtt";
            this.ProcesseBtt.Size = new System.Drawing.Size(202, 37);
            this.ProcesseBtt.TabIndex = 13;
            this.ProcesseBtt.Text = "Clean Extra Rows";
            this.ProcesseBtt.UseVisualStyleBackColor = true;
            this.ProcesseBtt.Click += new System.EventHandler(this.ProcesseBtt_Click);
            // 
            // checkDeleteProcesse
            // 
            this.checkDeleteProcesse.AutoSize = true;
            this.checkDeleteProcesse.Location = new System.Drawing.Point(252, 729);
            this.checkDeleteProcesse.Name = "checkDeleteProcesse";
            this.checkDeleteProcesse.Size = new System.Drawing.Size(163, 21);
            this.checkDeleteProcesse.TabIndex = 14;
            this.checkDeleteProcesse.Text = "Delete processed file";
            this.checkDeleteProcesse.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1047, 28);
            this.menuStrip1.TabIndex = 15;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(59, 24);
            this.helpToolStripMenuItem.Text = "Help!";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.HelpToolStripMenuItem_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(224, 558);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Select folder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(742, 558);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "Select folder";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(739, 57);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Right files";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(531, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(111, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "Right directories";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(220, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "Left files";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 17);
            this.label8.TabIndex = 21;
            this.label8.Text = "Left directories";
            // 
            // DeleteBttLeft
            // 
            this.DeleteBttLeft.Location = new System.Drawing.Point(384, 54);
            this.DeleteBttLeft.Name = "DeleteBttLeft";
            this.DeleteBttLeft.Size = new System.Drawing.Size(108, 23);
            this.DeleteBttLeft.TabIndex = 22;
            this.DeleteBttLeft.Text = "Delete Left";
            this.DeleteBttLeft.UseVisualStyleBackColor = true;
            this.DeleteBttLeft.Click += new System.EventHandler(this.DeleteBttLeft_Click);
            // 
            // DeleteBttRight
            // 
            this.DeleteBttRight.Location = new System.Drawing.Point(903, 54);
            this.DeleteBttRight.Name = "DeleteBttRight";
            this.DeleteBttRight.Size = new System.Drawing.Size(108, 23);
            this.DeleteBttRight.TabIndex = 23;
            this.DeleteBttRight.Text = "Delete Right";
            this.DeleteBttRight.UseVisualStyleBackColor = true;
            this.DeleteBttRight.Click += new System.EventHandler(this.DeleteBttRight_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(15, 747);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(202, 41);
            this.button1.TabIndex = 24;
            this.button1.Text = "Clean Extra spaces/tab";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // EditLeftBtt
            // 
            this.EditLeftBtt.Location = new System.Drawing.Point(288, 54);
            this.EditLeftBtt.Name = "EditLeftBtt";
            this.EditLeftBtt.Size = new System.Drawing.Size(90, 23);
            this.EditLeftBtt.TabIndex = 25;
            this.EditLeftBtt.Text = "Edit Left";
            this.EditLeftBtt.UseVisualStyleBackColor = true;
            this.EditLeftBtt.Click += new System.EventHandler(this.EditLeftBtt_Click);
            // 
            // EditRightBtt
            // 
            this.EditRightBtt.Location = new System.Drawing.Point(807, 54);
            this.EditRightBtt.Name = "EditRightBtt";
            this.EditRightBtt.Size = new System.Drawing.Size(90, 23);
            this.EditRightBtt.TabIndex = 26;
            this.EditRightBtt.Text = "Edit Right";
            this.EditRightBtt.UseVisualStyleBackColor = true;
            this.EditRightBtt.Click += new System.EventHandler(this.EditRightBtt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 800);
            this.Controls.Add(this.EditRightBtt);
            this.Controls.Add(this.EditLeftBtt);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.DeleteBttRight);
            this.Controls.Add(this.DeleteBttLeft);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.checkDeleteProcesse);
            this.Controls.Add(this.ProcesseBtt);
            this.Controls.Add(this.TransportBtt);
            this.Controls.Add(this.SearchHtmlTxt);
            this.Controls.Add(this.SearchHtmlByWords);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RightCombo);
            this.Controls.Add(this.LeftCombo);
            this.Controls.Add(this.RightFiles);
            this.Controls.Add(this.RightDirs);
            this.Controls.Add(this.LeftFiles);
            this.Controls.Add(this.LeftDirs);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "File Manager";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox LeftDirs;
        private System.Windows.Forms.ListBox LeftFiles;
        private System.Windows.Forms.ListBox RightDirs;
        private System.Windows.Forms.ListBox RightFiles;
        private System.Windows.Forms.ComboBox LeftCombo;
        private System.Windows.Forms.ComboBox RightCombo;
        private System.Windows.Forms.Button label1;
        private System.Windows.Forms.Button label2;
        private System.Windows.Forms.Button SearchHtmlByWords;
        private System.Windows.Forms.TextBox SearchHtmlTxt;
        private System.Windows.Forms.Button TransportBtt;
        private System.Windows.Forms.Button ProcesseBtt;
        private System.Windows.Forms.CheckBox checkDeleteProcesse;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button DeleteBttLeft;
        private System.Windows.Forms.Button DeleteBttRight;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button EditLeftBtt;
        private System.Windows.Forms.Button EditRightBtt;
    }
}

