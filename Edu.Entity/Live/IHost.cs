using System;

namespace Edu.Entity.Live
{
    public interface IHost:IDbData<LiveHostShow>
    {
        bool StreamSuccess();
        bool EnterLive(string uid, string pwd);
        AppConfigs.OperResult CreateLive(string uid);
    }
}