using System;
using System.IO;
using System.Windows.Forms;

using Downloader;

namespace 绿色软件
{
	public partial class OfflineForm : Form
	{
		public string path = Directory.GetCurrentDirectory() + "\\Download";

		public OfflineForm()
		{
			InitializeComponent();
		}

		private void OfflineForm_Load(object sender,EventArgs e)
		{
			refreshItems();
		}

		public void refreshItems()
		{
			button2.Enabled = false;
			button3.Enabled = false;
			button4.Enabled = false;
			button5.Enabled = false;
			listtView1.BeginUpdate();
			listtView1.Items.Clear();
			Directory.CreateDirectory(path);
			string[] dirs = Directory.GetDirectories(path);
			foreach(string dir in dirs)
			{
				if(File.Exists(dir + "\\info.ini"))
				{
					string[] spl = dir.Split('\\');
					ListViewItem item = new ListViewItem();
					item.Text = spl[spl.Length - 1];
					INIFile ini = new INIFile(dir + "\\info.ini");
					item.SubItems.Add(ini.ReadBool("INFO","finish",false) ? "已完成" : "未完成");
					listtView1.Items.Add(item);
				}
			}
			listtView1.EndUpdate();
			return;
		}

		private void button1_Click(object sender,EventArgs e)
		{
			refreshItems();
		}

		private void listView1_SelectedIndexChanged(object sender,EventArgs e)
		{
			changeButton();
		}

		public bool changeButton()
		{
			if(listtView1.SelectedItems.Count != 0)
			{
				button2.Enabled = true;
				button3.Enabled = true;
				button5.Enabled = true;
				if(listtView1.SelectedItems[0].SubItems[1].Text == "未完成")
				{
					button4.Enabled = true;
				}
				else
				{
					button4.Enabled = false;
				}
				return true;
			}
			else
			{
				button2.Enabled = false;
				button3.Enabled = false;
				button4.Enabled = false;
				button5.Enabled = false;
			}
			return false;
		}

		private void button2_Click(object sender,EventArgs e)
		{
			if(changeButton())
			{
				if(MessageBox.Show("确定要删除" + listtView1.SelectedItems[0].Text + "吗？","警告",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) == DialogResult.OK)
				{
					DeleteDirectory(path + "\\" + listtView1.SelectedItems[0].Text);
					MessageBox.Show("删除完成","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
					refreshItems();
				}
			}
		}

		public void DeleteDirectory(string path)
		{
			DirectoryInfo dir = new DirectoryInfo(path);
			if(dir.Exists)
			{
				DirectoryInfo[] childs = dir.GetDirectories();
				foreach(DirectoryInfo child in childs)
				{
					child.Delete(true);
				}
				dir.Delete(true);
			}
		}

		private void button5_Click(object sender,EventArgs e)
		{
			if(changeButton())
			{
				if(MessageBox.Show("确定要重新下载" + listtView1.SelectedItems[0].Text + "吗？","警告",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning) == DialogResult.OK)
				{
					INIFile ini = new INIFile(path + "\\" + listtView1.SelectedItems[0].Text + "\\info.ini");
					new DownloadForm().GoDownload(listtView1.SelectedItems[0].Text,ini.ReadString("INFO","URL",""));
					refreshItems();
				}
			}
		}

		private void button4_Click(object sender,EventArgs e)
		{
			INIFile ini = new INIFile(path + "\\" + listtView1.SelectedItems[0].Text + "\\info.ini");
			new DownloadForm().GoDownload(listtView1.SelectedItems[0].Text,ini.ReadString("INFO","URL",""),true,ini.ReadInteger("INFO","page",0),ini.ReadInteger("INFO","item",0),ini.ReadBool("INFO","bigmode",false));
			refreshItems();
		}

		private void OfflineForm_FormClosing(object sender,FormClosingEventArgs e)
		{
			e.Cancel = true;
			this.Visible = false;
		}
	}
}
