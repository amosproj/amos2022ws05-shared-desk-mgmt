/**
 * Program
 *
 * Version 1.0
 *
 * 2023-01-03
 *
 * MIT License
 */
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Net.Http.Headers;

namespace Deskstar.Core;

public class RequestInteractions
{
    public static Guid ExtractIdFromRequest(HttpRequest request)
    {
        var accessToken = request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", string.Empty);
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(accessToken);
        return new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.NameId).Value);
    }
}