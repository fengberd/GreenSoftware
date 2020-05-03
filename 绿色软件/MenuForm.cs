using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace 绿色软件
{
	public partial class MenuForm : Form
	{
		PictureBox[] _picture = null;
		Regex regex_mainItem = new Regex("<a href=\"([^\"]+)\" rel=\"nofollow\"><img src=\"([^\"]+)\" /></a></div><div class=\"gi\" style=\"height:\\d{0,5}px\">");
		EH_Menu_Item[] _Items = null;
		string Http = "";
		int page = 0;

		public MenuForm(string Http_,string Name)
		{
			InitializeComponent();

			Http = Http_;
			this.Text = Name;
			refreshItems(Http);
		}

		public void refreshItems(string Http)
		{
			label6.Visible = true;
			clearDisplay();
			Thread t = new Thread(new ThreadStart(delegate
			{
				string res = Web.HttpGet(Http + page.ToString());
				MatchCollection matches = regex_mainItem.Matches(res);
				_Items = new EH_Menu_Item[matches.Count];
				for(int i = 0;i < matches.Count;i++)
				{
					_Items[i] = new EH_Menu_Item();
					_Items[i].Http_LargePic = matches[i].Groups[1].Value;
					_Items[i].Http_SmallPic = matches[i].Groups[2].Value;
				}
				Invoke(new Action(delegate
				{
					button_last.Enabled = (res.IndexOf("Prev Page") != -1);
					button_next.Enabled = (res.IndexOf("Next Page") != -1);
					label6.Visible = false;
					refreshDisplay();
				}));
			}));
			t.IsBackground = true;
			t.Start();
		}

		public void refreshDisplay()
		{
			_picture = new PictureBox[_Items.Length];
			int i = 0;
			foreach(EH_Menu_Item item in _Items)
			{
				//PictureBox - 小图
				_picture[i] = new PictureBox();
				_picture[i].InitialImage = null;
				_picture[i].Location = new Point(203 * i,8);
				_picture[i].Name = "_picture_" + i.ToString();
				_picture[i].Size = new Size(200,288);
				_picture[i].TabIndex = 0;
				_picture[i].TabStop = false;
				_picture[i].ImageLocation = item.Http_SmallPic;
				_picture[i].InitialImage = Properties.Resources.Loading_2;
				_picture[i].Click += new EventHandler(ItemClick);
				this.groupBox1.Controls.Add(_picture[i]);
				i++;
			}
			label5.Visible = _Items.Length == 0;
			this.groupBox1.Width = i * (200 + 5) + 8;
			hScrollBar1.Maximum = this.groupBox1.Width - this.Width;
		}

		public void clearDisplay()
		{
			if(_picture == null)
			{
				return;
			}
			for(int i = 0;i < _picture.Length;i++)
			{
				this.groupBox1.Controls.Remove(_picture[i]);
			}
			hScrollBar1.Value = hScrollBar1.Maximum = 0;
		}

		private void ItemClick(object sender,EventArgs e)
		{
			int ID = -1;
			if(sender is PictureBox)
			{
				PictureBox obj = (PictureBox)sender;
				ID = int.Parse(obj.Name.Split('_')[2]);
			}
			if(ID == -1)
			{
				MessageBox.Show("出现严重错误，请联系开发者","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			new BigPictureForm(_Items[ID].Http_LargePic,this.Text,Http).Show();
		}

		private void MenuForm_Load(object sender,EventArgs e)
		{

		}

		private void hScrollBar1_Scroll(object sender,ScrollEventArgs e)
		{
			groupBox1.Left = -hScrollBar1.Value;
		}

		private void button_last_Click(object sender,EventArgs e)
		{
			page--;
			refreshItems(Http);
		}

		private void button_next_Click(object sender,EventArgs e)
		{
			page++;
			refreshItems(Http);
		}

		private void button_refresh_Click(object sender,EventArgs e)
		{
			refreshItems(Http);
		}
	}
}
