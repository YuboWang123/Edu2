using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Edu.Entity.Live;
using Edu.UI.Service.Live;
using Microsoft.AspNet.Identity;

namespace Edu.UI.Controllers.api
{
    public class LiveShowController : ApiController
    {
        private readonly HostShowRepository _reps;

        public LiveShowController(HostShowRepository repository)
        {
            _reps = repository;
        }

        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var mdl = _reps.Single(id);
            return Ok(mdl);
        }

        public IHttpActionResult Post([FromBody] LiveHostShow show)
        {
            if (ModelState.IsValid)
            {
                var uid = User.Identity.GetUserId();

                if (_reps.Exist(uid))
                {

                }
                show.Id = _reps.GenNewId();
                show.MakeDay = DateTime.Now;
                show.UserId =uid;
                _reps.Add(show);
            }
            return Ok(show.Id);
        }

    }
}
