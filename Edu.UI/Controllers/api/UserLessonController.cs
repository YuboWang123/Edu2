using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Edu.Entity.UserLesson;
using Edu.UI.Service.UserService;

namespace Edu.UI.Controllers.api
{
    /// <summary>
    /// user lesson api
    /// </summary>
    public class UserLessonController : ApiController
    {
        private UserLessonSv userLessonSv;

        public UserLessonController()
        {
            userLessonSv=new UserLessonSv();
        }

        [Authorize]
        public async Task<IHttpActionResult> Post([FromBody]UserLesson userLesson)
        {
            userLesson.EventDateTime=DateTime.Now;
            var r= await userLessonSv.AddTask(userLesson);
            return Ok(r>0);
            //throw new NotImplementedException();
        }


    }
}
