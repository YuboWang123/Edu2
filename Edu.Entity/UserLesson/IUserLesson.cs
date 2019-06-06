using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edu.Entity.UserLesson
{
    public interface IUserLesson:IDbData<UserLesson>
    {
        Task LessonStart(string lsnId);

    }
}
