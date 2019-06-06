

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edu.LivePush.Services;

namespace Edu.LivePush
{
    public partial class FormMain : Form
    {
        private string mediaServerIP;
        private int mediaServerPort;
        private string rtspSource;
        IntPtr tempPusher = IntPtr.Zero;
        public FormMain()
        {
            InitializeComponent();

            Visible = false;
            rtspSource = "rtsp://184.72.239.149/vod/mp4://BigBuckBunny_175k.mov";//rtsp地址
           // mediaServerIP = "121.40.46.250";//流媒体IP
            mediaServerIP = "localhost";//流媒体IP
            mediaServerPort = 554;//流媒体端口
        }

        private void BtnGo_click(object sender, EventArgs e)
        {
            if (!(string.IsNullOrWhiteSpace(TBUrl.Text) || string.IsNullOrWhiteSpace(TBPort.Text)))
            {
               
            }
            start();
        }

       
    

        private void BtnStop_click(object sender, EventArgs e)
        {
            //LBStatus.Text = "Stoped";
            //PusherSDK.ClosePush(tempPusher);
        }

        private void start()
        {
            //返回推流对象的地址：由C++内存对象托管到C#
            //tempPusher = PusherSDK.CreateStartPush(mediaServerIP, mediaServerPort, "t.sdp", rtspSource);

            //if (tempPusher != IntPtr.Zero)
            //{
            //    LBStatus.Text= "推流成功";//终端会打印结果的
            //}

          
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible == false)
            {
                this.Show();
            }
            else
            {
                Hide();
            }
           
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void OpenFileOToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void MenuExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MenuRecordScreen_Click(object sender, EventArgs e)
        {

        }



        private void FormMain_Shown(object sender, EventArgs e)
        {
            if (!DbConfigs.IsOnLine())
            {
                MessageBox.Show("没有联网", "system message", MessageBoxButtons.AbortRetryIgnore);
                return;
            }

        }
    }
}
