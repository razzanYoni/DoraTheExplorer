
namespace Tubes2_DoraTheExplorer
{
    partial class Form
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
            this.label1 = new System.Windows.Forms.Label();
            this.inputFile = new System.Windows.Forms.Button();
            this.outputFilename = new System.Windows.Forms.Label();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.bfsChoice = new System.Windows.Forms.RadioButton();
            this.dfsChoice = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 0;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // inputFile
            // 
            this.inputFile.Location = new System.Drawing.Point(67, 80);
            this.inputFile.Name = "inputFile";
            this.inputFile.Size = new System.Drawing.Size(104, 23);
            this.inputFile.TabIndex = 1;
            this.inputFile.Text = "Masukkan file";
            this.inputFile.UseVisualStyleBackColor = true;
            this.inputFile.Click += new System.EventHandler(this.inputFile_Click);
            // 
            // outputFilename
            // 
            this.outputFilename.AutoSize = true;
            this.outputFilename.ForeColor = System.Drawing.Color.White;
            this.outputFilename.Location = new System.Drawing.Point(76, 60);
            this.outputFilename.Name = "outputFilename";
            this.outputFilename.Size = new System.Drawing.Size(0, 17);
            this.outputFilename.TabIndex = 2;
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(315, 138);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 51;
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(240, 150);
            this.dataGridView.TabIndex = 3;
            // 
            // bfsChoice
            // 
            this.bfsChoice.AutoSize = true;
            this.bfsChoice.Checked = true;
            this.bfsChoice.ForeColor = System.Drawing.Color.DarkOrange;
            this.bfsChoice.Location = new System.Drawing.Point(67, 193);
            this.bfsChoice.Name = "bfsChoice";
            this.bfsChoice.Size = new System.Drawing.Size(55, 21);
            this.bfsChoice.TabIndex = 4;
            this.bfsChoice.TabStop = true;
            this.bfsChoice.Text = "BFS";
            this.bfsChoice.UseVisualStyleBackColor = true;
            this.bfsChoice.CheckedChanged += new System.EventHandler(this.bfsChoice_CheckedChanged);
            // 
            // dfsChoice
            // 
            this.dfsChoice.AutoSize = true;
            this.dfsChoice.ForeColor = System.Drawing.Color.DarkOrange;
            this.dfsChoice.Location = new System.Drawing.Point(67, 220);
            this.dfsChoice.Name = "dfsChoice";
            this.dfsChoice.Size = new System.Drawing.Size(56, 21);
            this.dfsChoice.TabIndex = 5;
            this.dfsChoice.Text = "DFS";
            this.dfsChoice.UseVisualStyleBackColor = true;
            this.dfsChoice.CheckedChanged += new System.EventHandler(this.dfsChoice_CheckedChanged);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dfsChoice);
            this.Controls.Add(this.bfsChoice);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.outputFilename);
            this.Controls.Add(this.inputFile);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "Form";
            this.Text = "Treasure Hunter";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion


        private string filepath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button inputFile;
        private System.Windows.Forms.Label outputFilename;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.RadioButton bfsChoice;
        private string algorithm;
        private System.Windows.Forms.RadioButton dfsChoice;
    }
}

