using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Edu.Entity.Live;

namespace Edu.UI.Models.LiveViewModels
{
    public class PagedLiveShow
    {
        public string pager { get; set; }
        public IEnumerable<LiveHostShow> LiveHostShows { get; set; }
    }
}