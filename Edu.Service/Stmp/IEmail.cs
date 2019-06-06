using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  System.Net.Mail;

namespace Edu.Service.Stmp
{
    public interface IEmail
    {
        Task Send(string content);
     
    }
}
