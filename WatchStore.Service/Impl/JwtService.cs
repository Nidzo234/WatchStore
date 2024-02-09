using Azure.Core;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WatchStore.Domain;

namespace WatchStore.Service
{
    public class JwtService : IJwtService
    {
        private string secureKey = "this is a very secure keyad14sfasdfgew26t2462wgvbvsdfbsdasfafaf af3efafsdgdes";

        public string Generate(Guid id)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secureKey));
            var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature);
            var header = new JwtHeader(credentials);

            var payload = new JwtPayload(id.ToString(), null, null, null, DateTime.Today.AddDays(1)); // 1 day
            var securityToken = new JwtSecurityToken(header, payload);

            return new JwtSecurityTokenHandler().WriteToken(securityToken);
        }

        public Guid GetUser(string jwt)
        {
            var token = this.Verify(jwt);
            Guid userId = Guid.Parse(token.Issuer);
            return userId;
        }

        public JwtSecurityToken Verify(string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(secureKey);
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);

                return (JwtSecurityToken)validatedToken;
            }
            return new JwtSecurityToken();
            
        }
    }
}
