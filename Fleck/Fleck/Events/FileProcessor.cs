using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fleck.Events
{

    /// <summary>
    /// get file from rtsp server folder.
    /// </summary>
    public class FileProcessor
    {
        private readonly string uid;
        //private readonly ISocket _ws;
        private FileInfo startingFile;
        private FileInfo[] files;
        private DirectoryInfo directoryInfo;
        public event EventHandler<FileInfoEventArg> NewFileCome;

        public delegate byte[] TransCode(FileInfo info);

        protected virtual void RaiseEvent(FileInfo file)
        {
            EventHandler<FileInfoEventArg> newFile = NewFileCome;
            if (newFile!=null)
            {
                FleckLog.Info("new file name is "+file.Name);
                newFile(this, new FileInfoEventArg(file));
            }
            else
            {
                FleckLog.Warn("no event subscriber found,pls check the program.");
            }
        }

        public string FileName
        {
            get
            {
                return "D:\\Projects\\NetEdu\\Edu.UI\\Upload\\Live"
                       + "\\t.sdp\\"
                       + DateTime.Now.Year
                       + DateTime.Now.Month.ToString("00")
                       + DateTime.Now.Day.ToString("00") + "\\";
                       //+uid;
            }
        }

        public FileProcessor(string uid)
        {
            this.uid = uid;
            CreateFolder();
            GetLastFile();
        }


        private void CreateFolder()
        {
            if (!Directory.Exists(FileName))
            {
                Directory.CreateDirectory(FileName);
            }
        }

        //stranscode ts file to mp4 
        private FileInfo GetLastFile()
        {
            directoryInfo = new DirectoryInfo(FileName);
            files = directoryInfo.GetFiles();
            if (files.Length > 0)
            {
                return files.OrderByDescending(a => a.CreationTime).FirstOrDefault();
            }


            return null;

        }

        /// <summary>
        /// get latest file info.
        /// </summary>
        /// <returns></returns>
        public void FileMonitor()
        {
            FileInfo fileNow;
            if (Directory.Exists(FileName))
            {
                while (true)
                {
                    fileNow = GetLastFile();
                    if (fileNow!=null && fileNow!=startingFile)
                    {
                       RaiseEvent(fileNow);
                        startingFile = fileNow;
                    }
                    Thread.Sleep(2000);
                }
            }
        }
    }
}
