using Newtonsoft.Json;
using Spark.MessengerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using static Spark.MessengerApi.AuthorizationUtils;

namespace Spark.MessengerApi.Controllers
{
    public class ContactController : ApiController
    {
        SessionInfo _SessionInfo = HttpContext.Current.GetSessionInfo();
        User _User = (HttpContext.Current.User as ClaimsPrincipal).ResolveIdentity();

        [Route("api/user/contact")]
        [Authorize]
        public async Task<IHttpActionResult> Get()
        {
            return Ok(_User);
        }
    }
}
