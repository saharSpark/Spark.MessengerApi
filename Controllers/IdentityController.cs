using Newtonsoft.Json;
using Spark.MessengerApi.Data;
using Spark.MessengerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Spark.MessengerApi.Controllers
{
    public class IdentityController : ApiController
    {
        SessionInfo _SessionInfo = HttpContext.Current.GetSessionInfo();

        [HttpGet]
        public async Task<IHttpActionResult> Validate(string token, string code)
        {
            _SessionInfo.Details = Request.UserAgentInfo();

            LogManager.WriteLog("info", token, this.Request.RequestUri.PathAndQuery, "", "", JsonConvert.SerializeObject(_SessionInfo)).Forget();
            var error = string.Empty;
            long? userId = 0;
            string user_Id = string.Empty;
            var content = string.Empty;
            var updates = DataClassesManager.Validate(token, code, out userId, out user_Id, out content, out error);

            if (!string.IsNullOrEmpty(error))
            {
                LogManager.WriteLog("error", token, this.Request.RequestUri.PathAndQuery, error).Forget();
                return BadRequest(error);
            }
            LogManager.WriteLog("info", token, this.Request.RequestUri.PathAndQuery,"success").Forget();
            return Ok();

            //foreach (var update in updates)
            //{
            //    if (update.Recipients != null && update.Recipients.Count > 0)
            //        Task.Factory.StartNew(() => ChatUtils.BroadcastUpdate((ChatUtils.NotificationTypeEnum)update.TypeId, update.Id, user_Id, update.Recipients.Select(x => x.Id).ToList(), ""));
            //}

        }
    }
}
