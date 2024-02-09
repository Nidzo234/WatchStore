using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchStore.Domain;

namespace WatchStore.Service
{
    public interface IJwtService
    {
        public string Generate(Guid id);
        public JwtSecurityToken Verify(string jwt);
        Guid GetUser(string jwt);
    }
}
