using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.Temps
{
    public class Require
    {
        public enum Rtype
        {
           xin,
           gan,
           pi,
           fi,
           shen
        }

        public string UserId { get; set; }
        public Rtype RequireType { get; set; }
        public string Msg { get; set; }
        public DateTime MakeDay { get; set; }


    }
}
