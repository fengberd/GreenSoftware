using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Downloader
{
    public partial class DownloadForm : Form
    {
        string name = "", url = "";
        int page = 0, item = 0;
        bool continueMode = false;
        Thread downloadThread = null;
        WebClient webClient = new WebClient();
        Regex regex_mainItem = new Regex("<div class=\"gi\" style=\"height:..?.?.?px\"><a href=\"(.{0,200})\" rel=\"nofollow\"><img src=\"(.{0,200})\" alt=\"\" /></a></div>"),
            regex_BigPicture = new Regex("<a href=\".{0,66666}\"><img id=\"sm\" src=\"(.{0,66666})\" alt=\".{0,66666}\" title=\".{0,66666}\" />");

        //托盘菜单
        private MenuItem Menu_showForm = new MenuItem("显示窗口");
        private MenuItem Menu_exit = new MenuItem("退出");

        public DownloadForm()
        {
            InitializeComponent();
        }

        public void GoDownload(string iname, string iurl, bool icontinueMode = false, int ipage = 0, int iitem = 0, bool ibigmode = false)
        {
            name = iname;
            label_name.Text = name;
            url = iurl;
            continueMode = icontinueMode;
            if (continueMode)
            {
                page = ipage;
                item = iitem;
                radioButton2.Checked = ibigmode;
                radioButton1.Checked = !ibigmode;
                startDownload();
            }
            this.Show();
            changeDownloadStatus("等待开始...");
        }

        private void startDownload()
        {
            button1.Text = "暂停保存";
            if (groupBox1.Enabled)
            {
                if (radioButton1.Checked)
                {
                    name += "(小图)";
                }
                changeDownloadStatus("创建目录...");
                Directory.CreateDirectory(System.IO.Directory.GetCurrentDirectory() + "\\Download\\" + name);
                groupBox1.Enabled = false;
            }
            stopDownload();
            downloadThread = new Thread(new ThreadStart(download));
            downloadThread.IsBackground = true;
            downloadThread.Start();
            changeDownloadStatus("下载开始...");
        }

        private void stopDownload(bool isok = false)
        {
            button1.Text = "开始保存";
            try
            {
                if (downloadThread != null)
                {
                    downloadThread.Abort();
                    downloadThread = null;
                    changeDownloadStatus("下载暂停...");
                }
                if(isok)
                {
                    this.Invoke(new Action(() =>
                    {
                        this.Show();
                        this.ShowInTaskbar = true;
                        notifyIcon1.Visible = false;
                        changeDownloadStatus("下载完成");
                    }));
                }
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void changeDownloadStatus(string text)
        {
            try
            { 
                this.Invoke(new Action(() =>
                {
                    notifyIcon1.Text = "绿色软件 - " + name.Substring(0, 20) + "\n" + text;
                    label1.Text = text;
                }));
            }
            catch (Exception e)
            {
                e.ToString();
            }
        }

        private void Menu_showForm_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void Menu_exit_Click(object sender, EventArgs e)
        {
            stopDownload();
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Menu_showForm.Click += new EventHandler(Menu_showForm_Click);
            Menu_exit.Click += new EventHandler(Menu_exit_Click);
            notifyIcon1.ContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIcon1.ContextMenu.MenuItems.Add(Menu_showForm);
            notifyIcon1.ContextMenu.MenuItems.Add(Menu_exit);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "开始保存")
            {
                startDownload();
            }
            else
            {
                stopDownload();
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }
        
        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopDownload();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            notifyIcon1.Visible = true;
        }

        private void download()
        {
            bool continue_ = true;
            string data = "";
            INIFile ini = new INIFile(System.IO.Directory.GetCurrentDirectory() + "\\Download\\" + name + "\\info.ini");
            ini.WriteString("INFO", "URL", url);
            ini.WriteBool("INFO", "finish", false);
            ini.WriteBool("INFO", "bigmode", radioButton2.Checked);
            ini.WriteInteger("INFO", "item", item);
            while(continue_)
            {
                ini.WriteInteger("INFO", "page", page);
                changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页\n开始读入页面数据...");
                do
                {
                    data = new Web().HttpGet(url + page.ToString());
                } while (data == "");
                changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页\n正在进行正则分析...");
                MatchCollection matches = regex_mainItem.Matches(data);
                for ( ; item < matches.Count; item++)
                {
                    changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页,第" + (item + 1).ToString() + "图\n正在读取数据...");
                    ini.WriteInteger("INFO", "item", item);
                    if(radioButton2.Checked)
                    {
                        changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页,第" + (item + 1).ToString() + "图\n正在解析大图地址...");
                        string big = getBigPictureHttpUrl(matches[item].Groups[1].Value);
                        changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页,第" + (item + 1).ToString() + "图\n正在下载大图...");
                        webClient.DownloadFile(big, System.IO.Directory.GetCurrentDirectory() + "\\Download\\" + name + "\\" + (page * 8 + item).ToString() + ".jpg");
                    }
                    else
                    {
                        changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页,第" + (item + 1).ToString() + "图\n正在下载小图...");
                        webClient.DownloadFile(matches[item].Groups[2].Value, System.IO.Directory.GetCurrentDirectory() + "\\Download\\" + name + "\\" + (page * 8 + item).ToString() + ".jpg");
                    }
                }
                item = 0;
                changeDownloadStatus("下载中 - 第" + (page + 1).ToString() + "页\n正在检测下一页...");
                continue_ = data.IndexOf("Next Page") != -1;
                page++;
            }
            ini.WriteBool("INFO", "finish", true);
            MessageBox.Show(name + "下载完成,\n共" + page.ToString() + "页", "下载完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            stopDownload(true);
        }

        private string getBigPictureHttpUrl(string url)
        {
            string data = "";
            do
            {
                data = new Web().HttpGet(url);
            } while (data == "");
            MatchCollection matches = regex_BigPicture.Matches(data);
            if (matches.Count == 0 || matches[0].Length == 0)
            {
                return "";
            }
            return matches[0].Groups[1].Value;
        }
    }
}