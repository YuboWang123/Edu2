using Edu.Entity;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


namespace Edu.UI.Controllers.api
{
    public class FileController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            string path = string.Empty;
            switch (name)
            {
                case "pusher":
                    path = AppConfigs.pusherApk;
                    break;
                case "testTemp":
                    path = AppConfigs.templatepath;
                    break;
                //if any other file need to be downloaded,add below:
                //-----------↓↓↓-------------------//
                //
                //
                default:
                        break;
            }

            if (path == String.Empty)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            if (!File.Exists(HttpContext.Current.Server.MapPath(path)))
            {
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }

            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.Open);
            HttpResponseMessage rsp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StreamContent(fs)
            };

            rsp.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = name
            };
            rsp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            rsp.Content.Headers.ContentLength = fs.Length;

            return rsp;
        }


  

    }





}
