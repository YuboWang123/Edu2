using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Edu.Entity.Message
{
    public class UserMessage
    {
        public enum Type
        {
            sys,
            user,
            anonymouse //msg content in the default page ,of which need to send to sys admin.
        }
        public string  FromUid { get; set; }
        public Type MsgType { get; set; }
        public DateTime MakeDay { get; set; }
        public DateTime ExpiredDay {
            get
                {
                    if (MsgType == Type.sys)
                    {
                        return MakeDay.AddDays(30);
                    }

                    return MakeDay.AddDays(10);

                }
        }
        public string MsgContent { get; set; }


    
    }
}
