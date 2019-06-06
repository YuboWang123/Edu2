using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Edu.Entity.Message;

namespace Edu.UI.Models
{
    public class UserMsg
    {
        [Key] public string Id { get; set; }
        public ApplicationUser ToUser { get; set; }
        public string ToUserId { get; set; }
        public UserMessage UserMessage { get; set; }
    }
}