using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace QuizMasterAPI
{
    public class JwtManager
    {
        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";



        public static string GenerateToken(string username, int expireMinutes = 20)

        {

            var symmetricKey = Convert.FromBase64String(Secret);

            var tokenHandler = new JwtSecurityTokenHandler();



            var now = DateTime.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor

            {

                Subject = new ClaimsIdentity(new[]

                        {

                            new Claim(ClaimTypes.Name, username)

                        }),



                Expires = now.AddMinutes(Convert.ToInt32(expireMinutes)),



                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)

            };



            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            

            var token = tokenHandler.WriteToken(stoken);



            return token;

        }
        public static String DecodeToken(String token)
        {
            String username = null;



            var simplePrinciple = JwtManager.GetPrincipal(token);

            var identity = simplePrinciple?.Identity as ClaimsIdentity;
            var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim?.Value;
            return username;
        }


        public static ClaimsPrincipal GetPrincipal(string token)

        {

            try

            {

                var tokenHandler = new JwtSecurityTokenHandler();

                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;



                if (jwtToken == null)

                    return null;



                var symmetricKey = Convert.FromBase64String(Secret);



                var validationParameters = new TokenValidationParameters()

                {

                    RequireExpirationTime = true,

                    ValidateIssuer = false,

                    ValidateAudience = false,

                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)

                };



                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);



                return principal;

            }



            catch (Exception)

            {

                return null;

            }

        }
    }
}