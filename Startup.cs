using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Spark.MessengerApi.Startup))]

namespace Spark.MessengerApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureOAuth(app);
        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromSeconds(TokenExpiry_Seconds),
                Provider = new AuthorizationProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
        private static int tokenExpiry_Seconds;
        private static int TokenExpiry_Seconds
        {
            get
            {

                if (tokenExpiry_Seconds == 0)
                {
                    if (!int.TryParse(System.Configuration.ConfigurationManager.AppSettings["TokenExpiry_Seconds"], out tokenExpiry_Seconds))
                        tokenExpiry_Seconds = 86400;
                }
                return tokenExpiry_Seconds;
            }

        }
    }
}
