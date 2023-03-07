using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Tubes2_DoraTheExplorer
{
    public partial class Form : System.Windows.Forms.Form
    {
        public Form()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            algorithm = "bfs";
            Console.WriteLine(algorithm);
        }

        private void inputFile_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.ShowDialog();
            filepath = dialog.FileName;
            outputFilename.Text = filepath.Substring(filepath.LastIndexOf('\\')+1);
            dataGridView.ColumnCount = 3;
            dataGridView.Rows.Add(3);
        }

        private void bfsChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (bfsChoice.Checked)
            {
                algorithm = "bfs";
                Console.WriteLine(algorithm);
            }
            
            
        }

        private void dfsChoice_CheckedChanged(object sender, EventArgs e)
        {
            if (dfsChoice.Checked)
            {
                algorithm = "dfs";
                Console.WriteLine(algorithm);
            }
            
        }
    }
}
