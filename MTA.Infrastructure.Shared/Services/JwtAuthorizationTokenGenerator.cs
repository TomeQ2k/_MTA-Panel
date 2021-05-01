using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MTA.Core.Application.Extensions;
using MTA.Core.Application.Services;
using MTA.Core.Application.ServicesUtils;
using MTA.Core.Common.Enums;
using MTA.Core.Common.Helpers;
using MTA.Core.Domain.Entities;

namespace MTA.Infrastructure.Shared.Services
{
    public class JwtAuthorizationTokenGenerator : IJwtAuthorizationTokenGenerator
    {
        public IConfiguration Configuration { get; }

        public JwtAuthorizationTokenGenerator(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var roleClaims = RolesServiceUtils.GetRoleClaims(user);

            foreach (var roleClaim in roleClaims)
                claims.Add(roleClaim);

            if (Configuration.IsDev(user.Id))
                claims.Add(new Claim(ClaimTypes.Role, Utils.EnumToString(RoleType.Owner)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Configuration.GetValue<string>(AppSettingsKeys.Token)));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(Constants.JwtTokenExpireTimeInDays),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}