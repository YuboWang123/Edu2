using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service.Stmp
{
    public class EmailModel
    {
        public string ToAddr { get; set; }
        public string MailBody { get; set; }
        public string From { get; set; }

    }
}
