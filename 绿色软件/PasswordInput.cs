using System;
using System.Windows.Forms;

namespace 绿色软件
{
	public partial class PasswordInput : Form
	{
		private MainForm Main = null;
		public PasswordInput(MainForm main)
		{
			Main = main;
			InitializeComponent();
		}

		private void button1_Click(object sender,EventArgs e)
		{
			if(MainForm.checkPassword(textBox1.Text))
			{
				this.Hide();
				timer1.Enabled = false;
				Main.Show();
			}
			else
			{
				MessageBox.Show("密码错误","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
		}

		private void timer1_Tick(object sender,EventArgs e)
		{
			Main.Hide();
		}

		private void PasswordInput_FormClosed(object sender,FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void button2_Click(object sender,EventArgs e)
		{
			Application.Exit();
		}

		private void PasswordInput_Load(object sender,EventArgs e)
		{

		}
	}
}
