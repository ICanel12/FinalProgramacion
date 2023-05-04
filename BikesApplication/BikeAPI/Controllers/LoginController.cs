using BikesApplicationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace BikeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {


        BikesApplicationModel.JWTResult jWTResult;


        [Route("Login")]
        [HttpPost]
        public BikesApplicationModel.Token Login(BikesApplicationModel.Token tokenRequest)
        {
            BikesApplicationModel.Token tokenResult = new BikesApplicationModel.Token();

            if (tokenRequest.token == "adfadsfadsfasd")
            {
                string applicationName = "BikesAPI";
                tokenResult.expirationTime = DateTime.Now.AddMinutes(30);
                tokenResult.token = CustomTokenJWT(applicationName, tokenResult.expirationTime);
            }
            return tokenResult;
        }

        private string CustomTokenJWT(string ApplicationName, DateTime token_expiration)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            BikesApplicationModel.JWTResult jWTResult = config.GetRequiredSection("JWT").Get<BikesApplicationModel.JWTResult>();

            var _symmetricSecurityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jWTResult.SecretKey)
            );

            var _signingCredentials = new SigningCredentials(
                _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
            );

            var _Header = new JwtHeader(_signingCredentials);

            var _Claims = new[] {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.NameId, ApplicationName)
        };

            var _Payload = new JwtPayload(
                issuer: jWTResult.Issuer,
                audience: jWTResult.Audience,
                claims: _Claims,
                notBefore: DateTime.Now,
                expires: token_expiration
            );

            var _Token = new JwtSecurityToken(
                _Header,
                _Payload
            );

            return new JwtSecurityTokenHandler().WriteToken(_Token);

        }

    }
}
