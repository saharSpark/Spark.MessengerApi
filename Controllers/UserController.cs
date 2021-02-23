using Newtonsoft.Json;
using Spark.MessengerApi.Data;
using Spark.MessengerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Spark.MessengerApi.Controllers
{
    public class UserController : ApiController
    {
        [Route("api/user")] //Registration
        [AllowAnonymous]
        public async Task<IHttpActionResult> PostUser(RegistrationDTO item)
        {
            var userId = string.Empty;
            if (!ModelState.IsValid)
            {
                LogManager.WriteLog("error", userId, this.Request.RequestUri.PathAndQuery, string.Format("{0}:{1}", JsonConvert.SerializeObject(item), JsonConvert.SerializeObject(ModelState))).Forget();
                return BadRequest(ModelState);
            }
            var error = string.Empty;
            if (string.IsNullOrEmpty(item.Mobile) && string.IsNullOrEmpty(item.Email))
            {
                error = "Mobile or email are required";
                DataClassesManager.ControllerLog("error", userId, this.Request.RequestUri.PathAndQuery, error);
                return BadRequest(error);
            }

            long? preUserId = 0;
            var password = AuthorizationUtils.HashPassword(item.Password);

            var validation = DataClassesManager.Register(item.Username.ToLower(), password.Hash, password.Salt, item.CountryId, IdentityUtils.GetIdentitiesXML(item.Mobile, item.Email), out preUserId, out error);

            if (!string.IsNullOrEmpty(error))
            {
                LogManager.WriteLog("error", userId, this.Request.RequestUri.PathAndQuery, error).Forget();
                return BadRequest(error);
            }

            foreach (var v in validation)
                await IdentityUtils.ValidateIdentity(v);

            LogManager.WriteLog("info", userId, this.Request.RequestUri.PathAndQuery, string.Format("{0}:{1}:{2}", preUserId, JsonConvert.SerializeObject(item), JsonConvert.SerializeObject(validation))).Forget();

            return Ok(new
            {
                RefId = preUserId,
                Identity = from x in validation
                           where x.Immediate
                           select new IdentityDTO
                           {
                               TypeId = x.TypeId,
                               Value = x.Identity,
                               Token = x.Token
                           }
            }
        );
        }
    }
}
