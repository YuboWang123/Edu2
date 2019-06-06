using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Edu.Service.Stmp
{
    public class Postman : IEmail
    {
        public Postman()
        {
            
        }

        public EmailModel ObjModelMail { get; set; }

        public Task Send(string content)
        {

            byte[] lenBytes;
            try
            {
                using (FileStream fs = new FileStream(new FileInfo( string.Format("{0}report_{1}.log", content, DateTime.Now.Day)).FullName,FileMode.Open ))
                {
                    lenBytes = new byte[fs.Length];
                    fs.Read(lenBytes, 0, lenBytes.Length);
                    MailMessage mail = new MailMessage();
                    mail.To.Add("40396383@qq.com");
                    mail.From = new MailAddress("40396383@qq.com");
                    mail.Subject = "weekly report";
                    mail.Attachments.Add(new Attachment(fs,"log"));
                    string Body = content;
                    mail.Body = Body;
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.qq.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("user", "pwd"); // Enter seders User name and password   
                    smtp.EnableSsl = true;
                    

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }

            return null;
        }
    }
}
