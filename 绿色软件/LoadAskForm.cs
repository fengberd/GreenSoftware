using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 绿色软件
{
    public partial class LoadAskForm : Form
    {
        MainForm Main = null;
        public LoadAskForm(MainForm main)
        {
            Main = main;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Main.Config.WriteBool("Config", "No_Ask_Again", true);
            }
            Main.Visible = true;
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Main.Visible = false;
        }

        private void LoadAskForm_Load(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
        }

        private void LoadAskForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
