using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 绿色软件
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.Run(new MainForm());
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            if (e.Exception.HResult == -2147024894)
            {
                try
                {
                    MainForm.ActiveForm.Hide();
                    ConfigForm.ActiveForm.Hide();
                    PasswordInput.ActiveForm.Hide();
                    LoadAskForm.ActiveForm.Hide();
                }
                catch (Exception) { }
                MessageBox.Show("无法加载应用程序，未找到所需模块(-2147024894)\n请不要删除任何dll文件！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.ActiveForm.Close();
                Application.Exit();
            }
            
        }
    }
}
