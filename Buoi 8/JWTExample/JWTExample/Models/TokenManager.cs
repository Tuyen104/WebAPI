using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JWTExample.Models
{
    public class TokenManager
    {
        private static string secret = "XCAP05H6LoKvbRRa/QkqLNMI7cOHguaRyHzyg7n5qEkGjQmtBhz4SzYh4Fqwjyi3KJHlSXKPwVu2+bXr6CtpgQ==";
        
        public static string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(securityKey,
                  SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(descriptor);
            return tokenHandler.WriteToken(token);
        }
        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwttoken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwttoken == null) return null;
                byte[] key = Convert.FromBase64String(secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null) return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identities;

            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }
    }
}