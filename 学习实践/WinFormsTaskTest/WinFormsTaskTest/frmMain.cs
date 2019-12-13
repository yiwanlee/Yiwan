using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsTaskTest
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        // 不使用多线程，卡死3秒
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "执行中...";
            Hao5();
            label1.Text = "执行结束了";
        }
        private bool Hao5()
        {
            for (int i = 1; i <= 3; i++)
            {
                System.Threading.Thread.Sleep(1000); // 模拟耗时任务
                label1.Text = $"执行中({i})";
            }
            return true;
        }





        //////////////////////////////
        // 使用异步耗时3秒，界面不卡死
        private async void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "执行中...";
            await Hao5Async();
            label1.Text = "执行结束了";
        }



        private async Task<bool> Hao5Async()
        {
            for (int i = 1; i <= 3; i++)
            {
                await System.Threading.Tasks.Task.Delay(1000); // 模拟耗时任务
                label1.Text = $"执行中({i})";
            }

            return true;
        }
    }
}
