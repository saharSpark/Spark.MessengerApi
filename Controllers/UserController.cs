﻿using Newtonsoft.Json;
using Spark.MessengerApi.Data;
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

namespace Spark.MessengerApi.Controllers
{
    public class UserController : ApiController
    {
        SessionInfo _SessionInfo = HttpContext.Current.GetSessionInfo();
        UserDTO _User = (HttpContext.Current.User as ClaimsPrincipal).ResolveIdentity();

        [AllowAnonymous]
        public async Task<IHttpActionResult> Post(RegistrationDTO item)
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
                IdentityUtils.ValidateIdentity(v);

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
        
        public async Task<IHttpActionResult> Get()
        {
            var error = string.Empty;
            LogManager.WriteLog("info", _User.Id, this.Request.RequestUri.PathAndQuery, "").Forget();
            var users = DataClassesManager.GetUsers(out error);

            if (!string.IsNullOrEmpty(error))
            {
                LogManager.WriteLog("error", _User.Id, this.Request.RequestUri.PathAndQuery, error).Forget();
                return BadRequest();
            }
            return Ok(users);
        }

        public async Task<IHttpActionResult> Get(string id)
        {
            var error = string.Empty;
            LogManager.WriteLog("info", _User.Id, this.Request.RequestUri.PathAndQuery, "").Forget();
            var user = DataClassesManager.GetUser(id, out error);

            if (!string.IsNullOrEmpty(error))
            {
                LogManager.WriteLog("error", _User.Id, this.Request.RequestUri.PathAndQuery, error).Forget();
                return BadRequest();
            }
            return Ok(user);
        }
    }
}
