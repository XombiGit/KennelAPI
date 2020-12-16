using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace KennelAPI.Controllers.Helpers
{
    public class ControllerHelper
    {
        public static Claim getUserFromToken(HttpRequest request)
        {
            var bearerToken = request.Headers["Authorization"];
            var actualToken = bearerToken.FirstOrDefault();
            var tokenSplit = actualToken?.Split(' '); //nullable operator

            if (tokenSplit == null || tokenSplit.Length < 2)
            {
                return null;
            }
            var jwtToken = new JwtSecurityToken(actualToken.Split(' ')[1]);
            var claims = jwtToken.Claims;
            return claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName);
        }
    }
}
