using Edu.Entity;
using Edu.Entity.TrainLesson;
using Edu.UI.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Edu.BLL.TrainLesson;
using System.IO;
using System.Threading.Tasks;

namespace Edu.UI.Areas.School.Service
{
    public class TrainVcrSv
    {
        private  ApplicationDbContext applicationDbContext;
        private string _ffmpegExe=HttpContext.Current.Server.MapPath("~/mpeg/ffmpeg.exe");

        public TrainVcrSv()
        {
        }

        public TrainVcrSv(string schoolId):this()
        {
            SchoolId = schoolId;
             
        }

        public TrainVcrSv(string schoolid,string lessonId):this(schoolid)
        {
            LessonId = lessonId;
        }

        protected string SchoolId { get; private set; }
        public string LessonId { get; }

       

        public bool Exists(string key)
        {
            using(applicationDbContext=new ApplicationDbContext())
            {
               return applicationDbContext.TrainVcrs.Find(key)==null;
            }           
        }

        public IEnumerable<Vcr> Query(string fk,int pg=1)
        {
            using (applicationDbContext = new ApplicationDbContext())
            {
                return applicationDbContext.TrainVcrs
                    .Where(a => a.LessonId == fk)
                    .OrderBy(a=>a.MakeDay)
                    . ToPagedList(pg,15);
            }
            
        }

        public int Add(Vcr mdl)
        {
            using (applicationDbContext = new ApplicationDbContext())
            {
                applicationDbContext.Entry(mdl).State = System.Data.Entity.EntityState.Added;
                return applicationDbContext.SaveChanges();
            }
                 
        }


        public int Update(Vcr mdl)
        {
            using (applicationDbContext = new ApplicationDbContext())
            {
                applicationDbContext.Entry(mdl).State = System.Data.Entity.EntityState.Modified;
                return applicationDbContext.SaveChanges();

            }
        }

        public int Del(string id)
        {
            using(applicationDbContext =new ApplicationDbContext())
            {
                var mdl = applicationDbContext.TrainVcrs.Find(id);
                if (mdl != null)
                {
                    applicationDbContext.Entry(mdl).State = System.Data.Entity.EntityState.Deleted;
                    return applicationDbContext.SaveChanges();
                }
                else
                {
                    return 0;
                }
            }          
            
        }

        public Vcr Single(string k)
        {
            using (applicationDbContext = new ApplicationDbContext())
            {
                return applicationDbContext.TrainVcrs.Find(k);
            }
        }  

        /// <summary>
        /// get video path.
        /// </summary>
        /// <param name="vcrId"></param>
        /// <returns></returns>
        public string VcrPath(string vcrId)
        {
            return Single(vcrId).VideoPath;
        }

        public async Task<double> VideoDuration(string sourceFile)
        {
            return await Task.Run<double>(() =>
            {
                return GetVideoDuration(_ffmpegExe, sourceFile);
            });
        }

        public static double GetVideoDuration(string ffmpegExefile, string sourceFile)
        {
            try
            {
                using (System.Diagnostics.Process ffmpeg = new System.Diagnostics.Process())
                {
                    String duration;  // soon will hold our video's duration in the form "HH:MM:SS.UU"  
                    String result;  // temp variable holding a string representation of our video's duration  
                    StreamReader errorreader;  // StringWriter to hold output from ffmpeg  

                    // we want to execute the process without opening a shell  
                    ffmpeg.StartInfo.UseShellExecute = false;
                    //ffmpeg.StartInfo.ErrorDialog = false;  
                    ffmpeg.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    // redirect StandardError so we can parse it  
                    // for some reason the output comes through over StandardError  
                    ffmpeg.StartInfo.RedirectStandardError = true;
                    // set the file name of our process, including the full path  
                    // (as well as quotes, as if you were calling it from the command-line)  
                    ffmpeg.StartInfo.FileName = ffmpegExefile;

                    // set the command-line arguments of our process, including full paths of any files  
                    // (as well as quotes, as if you were passing these arguments on the command-line)  
                    ffmpeg.StartInfo.Arguments = "-i " + sourceFile;

                    // start the process  
                    ffmpeg.Start();

                    // now that the process is started, we can redirect output to the StreamReader we defined  
                    errorreader = ffmpeg.StandardError;

                    // wait until ffmpeg comes back  
                    ffmpeg.WaitForExit();

                    // read the output from ffmpeg, which for some reason is found in Process.StandardError  
                    result = errorreader.ReadToEnd();

                    // a little convoluded, this string manipulation...  
                    // working from the inside out, it:  
                    // takes a substring of result, starting from the end of the "Duration: " label contained within,  
                    // (execute "ffmpeg.exe -i somevideofile" on the command-line to verify for yourself that it is there)  
                    // and going the full length of the timestamp  

                    duration = result.Substring(result.IndexOf("Duration: ") + ("Duration: ").Length, ("00:00:00").Length);

                    string[] ss = duration.Split(':');
                    int h = int.Parse(ss[0]);
                    int m = int.Parse(ss[1]);
                    int s = int.Parse(ss[2]);
                    return h * 3600 + m * 60 + s;
                }
            }
            catch (Exception ex)
            {
                return 60L;
            }
        }

    }
}