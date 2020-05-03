using System;
using System.Windows.Forms;

namespace 绿色软件
{
    public partial class ConfigForm : Form
    {
        MainForm Main = null;
        public ConfigForm(MainForm frm)
        {
            InitializeComponent();
            Main = frm;
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Main.no_picture;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Main.Config.WriteBool("Config", "no_picture", checkBox1.Checked);
            Main.loadConfig();
            Main.refreshDisplay();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!MainForm.checkPassword(textBox1.Text))
            {
                MessageBox.Show("原密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("密码不能为空", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text != textBox3.Text)
            {
                MessageBox.Show("两次输入的密码不相同", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MainForm.setPassword(textBox2.Text);
            MessageBox.Show("密码修改成功，新密码:\"" + textBox2.Text + "\",请牢记", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!MainForm.checkPassword(textBox1.Text))
            {
                MessageBox.Show("原密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MainForm.delPassword();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            MessageBox.Show("密码删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
