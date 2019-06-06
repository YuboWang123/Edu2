using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace Edu.Entity
{
    public class AppConfigs
    {
        public static string defaultImagePath = ConfigurationManager.AppSettings.Get("defaultImage");
        public static string uploadPath = ConfigurationManager.AppSettings.Get("upload");
        public static string templatepath = ConfigurationManager.AppSettings.Get("template");
        public static string appAdmin = ConfigurationManager.AppSettings.Get("sys");
        public static string pusherApk= ConfigurationManager.AppSettings.Get("pusher");

        /// <summary>
        /// Study Card Type.
        /// </summary>
        public enum BatchType
        {
            RechargableCard=0,
            PeriodCard,
        }

        public enum OperResult
        {
            success,
            failDueToFk, //pls delete related data first .
            failDueToArgu, //arguments error.
            failDueToExist,
            failDueToAuthen,
            failVip,
            failUnknown,
            locked
        }

        public enum PayBy
        {
            alipay,
            weChat,
            yun //云闪付
        }

        public enum BatchCardStatus
        {
            Created=0, //when creating
            Outdated,//end day plus validated period.
            Deleted //manual change.
        }


        public enum SingleCardStatus
        {
            NeverUsed,
            InUse,
            Outdated,
            Deleted,
            Freezed,
            AdminFreezed
        }


        public static IEnumerable<KeyValuePair<int, string>> GetStatusSingleCard()
        {
            return Wyb.General.Utility.GenSelectListItem(typeof(SingleCardStatus));
        }
    
        public enum AppRole
        {
            sys,
            teacher,
            schoolmaster,
            student
        }

        public enum FileType
        {
            TxtFile,
            VideoFile,
            ImgFile
        }


    }
}
