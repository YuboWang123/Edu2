using System;
using System.Collections.Generic;
using System.Text;

namespace Edu.Entity.Message
{
    public interface IMsg
    {
        void Send(string uid);
        void View(string key);
        IEnumerable<UserMessage> ListMsg(int pg,out int ttl);
    }
}
