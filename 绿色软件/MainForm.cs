using System;
using System.IO;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Microsoft.Win32;
using Microsoft.VisualBasic;

namespace 绿色软件
{
	public partial class MainForm : Form
	{
		public int page = 0;
		public string serch = "";
		public bool no_picture = false;

		public Web WEB = new Web();
		public INIFile Config = null;
		public EH_Item[] _Items = null;
		public ConfigForm cfgForm = null;
		public GroupBox[] _groupbox = null;
		public PictureBox[] _picture = null;
		public TextBox[] _title = null, _tags = null;
		public OfflineForm offline = new OfflineForm();
		public Label[] _time = null, _type = null, _text1 = null, _star = null;
		public Regex regex_mainItem = new Regex("<div class=\"ig\"><table><tr><td class=\"ii\"><a href=\"(.{0,200})\"><img src=\"(.{0,500})\" alt=\"Cover Image\" /></a></td><td><table class=\"it\"><tr><td colspan=\"2\"><a class=\"b\" href=\".{0,100}\">(.{0,500})</a></td></tr><tr><td class=\"ik ip\">Posted:</td><td class=\"ip\">(....)-(..)-(..) (..?):(..?) by (.{0,50})</td></tr><tr><td class=\"ik\">Category:</td><td>(.{0,50})</td></tr><tr><td class=\"ik\">Tags:</td><td>(.{0,500})</td></tr><tr><td class=\"ik\">Rating:</td><td class=\"ir\">(.{0,30})</td></tr><tr><td colspan=\"2\" class=\"fp\"><a href=\"(.{0,200})\">Go To First Page</a></td></tr></table></td></tr></table></div>");

		public void loadConfig()
		{
			no_picture = Config.ReadBool("Config","no_picture",false);
			if(!Config.ReadBool("Config","No_Ask_Again",false))
			{
				this.Visible = false;
				new LoadAskForm(this).Show();
			}
		}

		private void vScrollBar1_Scroll(object sender,ScrollEventArgs e)
		{
			groupBox2.Top = -vScrollBar1.Value - 8;
		}

		private void button3_Click(object sender,EventArgs e)
		{
			refreshItems(page);
		}

		private void button6_Click(object sender,EventArgs e)
		{
			page++;
			refreshItems(page);
		}

		private void button1_Click(object sender,EventArgs e)
		{
			page--;
			if(page < 0)
			{
				MessageBox.Show("已翻到第一页","警告",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				page = 0;
			}
			refreshItems(page);
		}

		private void button4_Click(object sender,EventArgs e)
		{
			string input = "";
			int tmp = 0;
			do
			{
				input = Interaction.InputBox("请输入要跳到的页码","跳页",page.ToString(),-1,-1);
				if(input == "")
				{
					return;
				}
			} while(!int.TryParse(input,out tmp) || tmp < 0);
			if(input != "")
			{
				page = tmp - 1;
				refreshItems(page);
			}
		}

		private void button5_Click(object sender,EventArgs e)
		{
			if(button5.Text == "返回主页")
			{
				button5.Text = "搜索";
				serch = "";
				page = 0;
			}
			else
			{
				string input = "";
				do
				{
					input = Interaction.InputBox("请输入要搜索的内容(最少3字符)","搜索","",-1,-1);
					if(input == "")
					{
						return;
					}
				} while(input.Length <= 2);
				if(input != "")
				{
					serch = input;
					page = 0;
					button5.Text = "返回主页";
				}
			}
			refreshItems(page);
		}

		private void ItemClick(object sender,EventArgs e)
		{
			int ID = -1;
			if(sender is PictureBox)
			{
				PictureBox obj = (PictureBox)sender;
				ID = int.Parse(obj.Name.Split('_')[2]);
			}
			if(sender is GroupBox)
			{
				GroupBox obj = (GroupBox)sender;
				ID = int.Parse(obj.Name.Split('_')[2]);
			}
			if(ID == -1)
			{
				MessageBox.Show("出现严重错误，请联系开发者","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			new MenuForm(_Items[ID].Http_Menu,_title[ID].Text).Show();
		}

		public MainForm()
		{
			InitializeComponent();
			try
			{
				RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\FENGberd\\" + MD5.HashString("绿色软件"));
				if(reg.GetValue(MD5.HashString("password")) != null)
				{
					new PasswordInput(this).Show();
				}
			}
			catch(Exception e)
			{
				MessageBox.Show("无法访问注册表:" + e.ToString(),"错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
				Environment.Exit(0);
			}
		}

		private void button2_Click(object sender,EventArgs e)
		{
			cfgForm.Show();
		}

		private void button7_Click(object sender,EventArgs e)
		{
			offline.Show();
			offline.refreshItems();
		}

		public void asyncLoad(object param)
		{
			int[] data = (int[])param;
			string res = Web.HttpGet(serch == "" ? "https://e-hentai.org/lofi/?page=" + data[0].ToString() : "https://e-hentai.org/e-hentai.org/lofi/?page=" + data[0].ToString() + "&f_search=" + serch + "&f_sname=1&f_stags=1&f_apply=Apply+Filter");
			MatchCollection matches = regex_mainItem.Matches(res);
			_Items = new EH_Item[matches.Count];
			Invoke(new Action(delegate
			{
				for(int i = 0;i < matches.Count;i++)
				{
					_Items[i] = new EH_Item();
					_Items[i].Http_Menu = matches[i].Groups[1].Value;
					_Items[i].Http_Picture = matches[i].Groups[2].Value;
					_Items[i].Name = matches[i].Groups[3].Value;
					_Items[i].Posted = matches[i].Groups[4].Value + "-" + matches[i].Groups[5].Value + "-" + matches[i].Groups[6].Value + " " + matches[i].Groups[7].Value + ":" + matches[i].Groups[8].Value + " by " + matches[i].Groups[9].Value;
					_Items[i].Category = matches[i].Groups[10].Value;
					_Items[i].Tags = matches[i].Groups[11].Value;
					_Items[i].Star = matches[i].Groups[12].Value.Replace("*","★").Replace(" ","");
				}
				this.Text = "绿色软件 - " + (serch == "" ? "" : "搜索:" + serch + " - ") + "第" + (page + 1).ToString() + "页 - 项目数:" + (matches.Count).ToString();
				if(data[1]!=1)
				{
					refreshDisplay();
				}
				label6.Visible = false;
			}));
		}

		public void refreshItems(int page,bool dontChange = false)
		{
			label6.Visible = true;
			Thread t = new Thread(new ParameterizedThreadStart(asyncLoad));
			t.IsBackground = true;
			t.Start(new int[]
			{
				page,
				dontChange ? 1 : 0
			});
		}

		public void refreshDisplay()
		{
			clearDisplay();
			_groupbox = new GroupBox[_Items.Length];
			_title = new TextBox[_Items.Length];
			_tags = new TextBox[_Items.Length];
			_picture = new PictureBox[_Items.Length];
			_time = new Label[_Items.Length];
			_type = new Label[_Items.Length];
			_text1 = new Label[_Items.Length];
			_star = new Label[_Items.Length];
			int i = 0;
			foreach(EH_Item item in _Items)
			{
				//TextBox - 标题
				_title[i] = new TextBox();
				_title[i].BorderStyle = BorderStyle.None;
				_title[i].Font = new Font("黑体",12F);
				_title[i].Location = new Point(162,15);
				_title[i].Multiline = true;
				_title[i].Name = "_title" + i.ToString();
				_title[i].ReadOnly = true;
				_title[i].Size = new Size(500,51);
				_title[i].TabIndex = 1;
				_title[i].Text = item.Name;
				//Label - 投稿时间
				_time[i] = new Label();
				_time[i].Font = new Font("新宋体",11.25F,FontStyle.Regular,GraphicsUnit.Point,134);
				_time[i].Location = new Point(159,69);
				_time[i].Name = "_time" + i.ToString();
				_time[i].Size = new Size(503,15);
				_time[i].TabIndex = 2;
				_time[i].Text = "投稿时间:" + item.Posted;
				//Label - 分类
				_type[i] = new Label();
				_type[i].Font = new Font("新宋体",11.25F,FontStyle.Regular,GraphicsUnit.Point,134);
				_type[i].Location = new Point(159,85);
				_type[i].Name = "_type" + i.ToString();
				_type[i].Size = new Size(503,15);
				_type[i].TabIndex = 3;
				_type[i].Text = "分类    :" + item.Category;
				//Label - Tags
				_text1[i] = new Label();
				_text1[i].AutoSize = true;
				_text1[i].Font = new Font("新宋体",11.25F,FontStyle.Regular,GraphicsUnit.Point,134);
				_text1[i].Location = new Point(159,101);
				_text1[i].Name = "_text1" + i.ToString();
				_text1[i].Size = new Size(79,15);
				_text1[i].TabIndex = 4;
				_text1[i].Text = "Tags    :";
				//TextBox - Tags
				_tags[i] = new TextBox();
				_tags[i].BorderStyle = BorderStyle.None;
				_tags[i].Font = new Font("新宋体",11.25F);
				_tags[i].Location = new Point(236,103);
				_tags[i].Multiline = true;
				_tags[i].Name = "_tags" + i.ToString();
				_tags[i].ReadOnly = true;
				_tags[i].Size = new Size(426,51);
				_tags[i].TabIndex = 5;
				_tags[i].Text = item.Tags;
				//Label - 评分
				_star[i] = new Label();
				_star[i].Font = new Font("新宋体",11.25F,FontStyle.Regular,GraphicsUnit.Point,134);
				_star[i].Location = new Point(159,155);
				_star[i].Name = "_star" + i.ToString();
				_star[i].Size = new Size(503,15);
				_star[i].TabIndex = 6;
				_star[i].Text = "评分    :" + item.Star;
				//PictureBox - 图片
				_picture[i] = new PictureBox();
				_picture[i].InitialImage = null;
				_picture[i].Location = new Point(6,17);
				_picture[i].Name = "_picture_" + i.ToString();
				_picture[i].Size = new Size(150,150);
				_picture[i].TabIndex = 0;
				_picture[i].TabStop = false;
				if(no_picture)
				{
					_picture[i].Image = Properties.Resources.No_picture;
				}
				else
				{
					_picture[i].ImageLocation = item.Http_Picture;
				}
				_picture[i].InitialImage = Properties.Resources.Loading_1;
				_picture[i].Click += new EventHandler(ItemClick);
				//GrouupBox - 主容器
				_groupbox[i] = new GroupBox();
				_groupbox[i].Controls.Add(_star[i]);
				_groupbox[i].Controls.Add(_tags[i]);
				_groupbox[i].Controls.Add(_text1[i]);
				_groupbox[i].Controls.Add(_type[i]);
				_groupbox[i].Controls.Add(_time[i]);
				_groupbox[i].Controls.Add(_title[i]);
				_groupbox[i].Controls.Add(_picture[i]);
				_groupbox[i].Location = new Point(1,5 + i * 180);
				_groupbox[i].Name = "_groupbox_" + i.ToString();
				_groupbox[i].Size = new Size(670,180);
				_groupbox[i].TabIndex = 0;
				_groupbox[i].TabStop = false;
				_groupbox[i].Click += new EventHandler(ItemClick);
				this.groupBox2.Controls.Add(_groupbox[i]);
				i++;
			}
			label5.Visible = _Items.Length == 0;
			this.groupBox2.Height = i * (180 + 2) + 8;
			//vScrollBar1.Minim = 8;
			vScrollBar1.Maximum = this.groupBox2.Height - this.Height;
		}

		public void clearDisplay()
		{
			if(_groupbox == null)
			{
				return;
			}
			int i = 0;
			foreach(GroupBox g in _groupbox)
			{
				g.Controls.Remove(_title[i]);
				g.Controls.Remove(_tags[i]);
				g.Controls.Remove(_picture[i]);
				g.Controls.Remove(_time[i]);
				g.Controls.Remove(_type[i]);
				g.Controls.Remove(_text1[i]);
				g.Controls.Remove(_star[i]);
				this.groupBox2.Controls.Remove(g);
				i++;
			}
		}

		private void Form1_Load(object sender,EventArgs e)
		{
			cfgForm = new ConfigForm(this);
			Config = new INIFile(Directory.GetCurrentDirectory() + "\\config.ini");
			loadConfig();
			textBox1.BackColor = SystemColors.Control;
			textBox2.BackColor = SystemColors.Control;
			pictureBox1.BackColor = SystemColors.Control;
			refreshItems(0);
		}

		public static bool checkPassword(string psw)
		{
			RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\FENGberd\\" + MD5.HashString("绿色软件"));
			Console.WriteLine((string)reg.GetValue(MD5.HashString("password")));
			if(reg.GetValue(MD5.HashString("password")) == null || (string)reg.GetValue(MD5.HashString("password")) == MD5.HashString(psw))
			{
				return true;
			}
			return false;
		}

		public static void setPassword(string psw)
		{
			RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\FENGberd\\" + MD5.HashString("绿色软件"));
			reg.SetValue(MD5.HashString("password"),MD5.HashString(psw));
		}

		public static void delPassword()
		{
			RegistryKey reg = Registry.LocalMachine.CreateSubKey("SOFTWARE\\FENGberd\\" + MD5.HashString("绿色软件"));
			reg.DeleteValue(MD5.HashString("password"));
		}
	}
}
