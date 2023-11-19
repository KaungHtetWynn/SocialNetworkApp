using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService : ITokenService
{
    private readonly SymmetricSecurityKey _key;

    // get configuration from config file and inject it into the service using IConfiguration
    public TokenService(IConfiguration config)
    {
        // encode into byte because SSK takes in byte and reads from config file (key-value pair)
        // symmetric key where the same key is used to encrypt the data as is used to decrypt the data
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    }
    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
        };

        // signing credentials
        // 2nd parameter is what algorithm do we want to use to encrypt this particular key?
        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        // describe the token that we are going to include/return
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7), // token expiry
            SigningCredentials = credentials
        };

        // token handler
        var tokenHandler = new JwtSecurityTokenHandler();

        // create token
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

