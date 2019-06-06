using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Edu.LivePush.Services
{
    public static class DbConfigs
    {
      
        /// <summary>
        /// check if is connected on the line
        /// </summary>
        /// <param name="connectionDescription"></param>
        /// <param name="reservedValue"></param>
        /// <returns></returns>
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        public static bool IsOnLine()
        {
            if (InternetGetConnectedState(out int i, 0))
            {
                return true;

            }
            else
            {
                return false;

            }

         
        }
    }
}
