using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MTA.Core.Common.Helpers;

namespace MTA.API.AppConfigs
{
    public static class AuthenticationAppConfig
    {
        public static AuthenticationBuilder ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
            => services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>(AppSettingsKeys.Token))),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
    }
}