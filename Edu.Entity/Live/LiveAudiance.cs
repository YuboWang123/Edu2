using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace Edu.Entity.Live
{
    public class LiveAudiance
    {
       [Key]public string Id { get; set; }
       public string UserId { get; set; }
       [Display(Name = "姓名")]public string NickName { get; set; }
       public DateTime MakeDay { get; set; }
       public LiveHostShow HostShow { get; set; }
       public string HostShowId { get; set; }

    }
}
