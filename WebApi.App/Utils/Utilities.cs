using DB.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebApi.App.Utils
{
    public class Utilities
    {
        // allows to access the appsettings configurations.
        private IConfiguration _configuration;

        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // method to encrypt password
        public string Encrypt(string text)
        {
            // create the array of bytes using the text in parameter, with HashData.
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));

            // convert the array in string
            StringBuilder sBuilder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                sBuilder.Append(bytes[i].ToString("x2"));
            }
            return sBuilder.ToString(); // return the text encrypted.
        }

        // generate the JWT
        public string GenerateJWT(User model)
        {
            // create the user information for Token, using Claims
            var userClaims = new[]
            {
                // create the information with Claims to create the JWT.
                new Claim(ClaimTypes.NameIdentifier, model.UserId.ToString()),
                new Claim(ClaimTypes.Name, model.UserName),
            };

            // get the key in appsettings and convert it in bytes to create a new SymmetricSecurityKey.
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            // create credentials to authenticate
            var credentals = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create the token configuration
            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: credentals
                );

            // return the token
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
