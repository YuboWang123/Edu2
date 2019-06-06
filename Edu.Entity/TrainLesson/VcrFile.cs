

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Wyb.General;

namespace Edu.Entity.TrainLesson
{

    /// <summary>
    /// vcr 资源文件
    /// </summary>
    [Serializable]
    public class VcrFile:IFileExist,ITester
    {
        [Key]
        public  string Id { get; set; }
        public string VcrId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public DateTime MakeDay { get; set; }
        public string Maker { get; set; }
        public bool? FileOk { get; set; }
        public float FileSize { get; set; }
        public string Memo { get; set; }

        public bool DelFile()
        {
            if (this.Exist())
            {
                return Utility.DeleteFile(this.Path,false);
            }
            return true;          
        }

        public bool Exist()
        {
            return Utility.FileExists(this.Path);

        }
    }
}
