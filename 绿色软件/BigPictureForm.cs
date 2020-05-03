using System;
using System.Drawing;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using Downloader;

namespace 绿色软件
{
    public partial class BigPictureForm : Form
    {
        Regex regex_mainItem = new Regex("<a href=\".{0,666}\"><img id=\"sm\" src=\"(.{0,666})\" alt=\".{0,666}\" title=\".{0,666}\" /></a>"),
            regex_Next = new Regex("<a href=\"(.{0,100})\">Next Page &gt;</a>"),
            regex_Last = new Regex("<a href=\"(.{0,100})\">&lt; Prev Page</a>");
        string Http = "", Http_last = "", Http_next = "", menuUrl = "";

        public BigPictureForm(string Http_, string Name, string menuUrl_)
        {
            InitializeComponent();
            menuUrl = menuUrl_;
            Http = Http_;
            this.Text = Name;
        }

        private void BigPictureForm_Load(object sender, EventArgs e)
        {
            refreshPicture();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            refreshSize();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sdf = new SaveFileDialog();
            sdf.Filter = "图片文件(*.jpg)|*.jpg";
            sdf.Title = "保存单图...";
            sdf.AddExtension = true;
            if (sdf.ShowDialog() == DialogResult.OK)
            {
                Image img = pictureBox1.Image;
                img.Save(sdf.FileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\Download\\" + this.Text);
            new DownloadForm().GoDownload(this.Text, menuUrl);
        }

        public void refreshPicture()
        {
            string res = Web.HttpGet(Http);
            MatchCollection matches = regex_mainItem.Matches(res);
            MatchCollection matches_last = regex_Last.Matches(res);
            MatchCollection matches_next = regex_Next.Matches(res);
            Console.Write(res);
            if (matches.Count == 0)
            {
                label5.Visible = true;
                button_last.Enabled = false;
                button_next.Enabled = false;
                pictureBox1.Visible = false;
            }
            else
            {
                label5.Visible = false;
                pictureBox1.Visible = true;
                pictureBox1.ImageLocation = matches[0].Groups[1].Value;
                if (matches_last.Count != 0)
                {
                    button_last.Enabled = true;
                    Http_last = matches_last[0].Groups[1].Value;
                    Console.Write("last:"+Http_last);
                }
                else
                {
                    button_last.Enabled = false;
                    Http_last = "";
                }

                if (matches_next.Count != 0)
                {
                    button_next.Enabled = true;
                    Http_next = matches_next[0].Groups[1].Value;
                    Console.Write("next:" + Http_next);
                }
                else
                {
                    button_next.Enabled = false;
                    Http_next = "";
                }
            }
        }

        public void refreshSize()
        {
            this.Width = pictureBox1.Width + 15;
            this.Height = pictureBox1.Height + 60 + 23;

            button_last.Top = pictureBox1.Height;
            button_next.Top = pictureBox1.Height;
            button1.Top = pictureBox1.Height + 22;
            button2.Top = pictureBox1.Height + 22;

            button_last.Width = pictureBox1.Width / 2;
            button_next.Width = pictureBox1.Width / 2;
            button1.Width = pictureBox1.Width / 2;
            button2.Width = pictureBox1.Width / 2;

            button_next.Left = button_last.Width;
            button1.Left = button2.Width;
        }

        private void button_last_Click(object sender, EventArgs e)
        {
            Http = Http_last;
            refreshPicture();
        }

        private void button_next_Click(object sender, EventArgs e)
        {
            Http = Http_next;
            refreshPicture();
        }
    }
}