
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Host
{
    public class ClientModel
    {
        public enum userType
        {
            host,
            user
        }

        public string uid { get; set; }
        public userType UserType { get; set; }
        public DateTime DateTime { get; set; }
       
    }
}
