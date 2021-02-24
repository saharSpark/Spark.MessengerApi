
using Spark.MessengerApi.Data;
using Spark.MessengerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spark.MessengerApi
{
    public class AuthorizationRepository : IDisposable
    {
        public AuthSessionInfo ValidateSession(string username)
        {
            var id = string.Empty;
            long? internalId = -1;
            var password = new byte[] { };
            var salt = new byte[] { };
            try
            {
                DataClassesManager.preAuthorizationTokenGeneration(username, out id, out internalId, out password, out salt);
                return new AuthSessionInfo { Id = id, UserId = internalId.Value, Password = password, Salt = salt };
            }
            catch (Exception)
            {
                return new AuthSessionInfo { Id = "", UserId= -1 };
            }
        }
        public void Dispose()
        {

        }
    }
}