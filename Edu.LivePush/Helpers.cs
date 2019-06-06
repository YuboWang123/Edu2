using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Edu.Entity.Live;

namespace Edu.LivePush
{
    public class Helpers
    {
        public Helpers()
        {
           
        }

        /// <summary>
        /// 哪个窗体想要置顶，在Form_Load中加上
        ///SetWindowPos(this.Handle, -1, 0, 0, 0, 0, 1 | 2); //最后参数也有用1 | 4　
        ///具体说明，看API函数说明
        ///如果是用点击一个按钮后弹出新窗体，并置顶，则： 
        ///Form2 frm = new Form2();
        ///frm.Show(); 
        ///SetWindowPos(GetForegroundWindow(), -1, 0, 0, 0, 0, 1 | 2);
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="hWndInsertAfter"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        
        /// <summary> 
        /// 得到当前活动的窗口 
        /// </summary> 
        /// <returns></returns> 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern System.IntPtr GetForegroundWindow();
       

        /// <summary>  
        /// 修改程序在注册表中的键值  
        /// </summary>  
        /// <param name="isAuto">true:开机启动,false:不开机自启</param> 
        private void AutoStart(bool isAuto = true, bool showinfo = true)
        {
            try
            {
                if (isAuto == true)
                {
                    RegistryKey R_local = Registry.CurrentUser;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.SetValue("LivePusher", Application.ExecutablePath);
                    R_run.Close();
                    R_local.Close();
                }
                else
                {
                    RegistryKey R_local = Registry.CurrentUser;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.DeleteValue("LivePusher", false);
                    R_run.Close();
                    R_local.Close();
                }
            }
            catch (Exception)
            {
                if (showinfo)
                    MessageBox.Show("您需要管理员权限修改", "提示");
            }
        }

        /// <summary>
        /// get audiance user counted
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<int> GetUser(PushUser user)
        {
            throw new NotImplementedException();
        }

      
    }
}
