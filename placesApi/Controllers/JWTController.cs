using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace placesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTController : ControllerBase
    {

        [HttpPost("/jwt",Name ="token")]
        [AllowAnonymous]
        public async Task<ActionResult> Jwt([FromForm]Login login)
        {

            string key = "AASFGHJKLKJYTEREWSDF6654ESEE4W33EER45R5TG";

           
            Token Tokenizer=null;

            List<dynamic> data=null ;

            if (!string.IsNullOrWhiteSpace(login.Correo) &&
                !string.IsNullOrWhiteSpace(login.Contrasena))
            {
                using (var plaze = new PlaceManager())
                {
                    data= await plaze.validarUsuario(login);
                }

                if (data.Count > 0)
                {
                    if (data.Cast<dynamic>().FirstOrDefault().correo.Equals(login.Correo) 
                        && data.Cast<dynamic>().FirstOrDefault().contrasena.Equals(login.Contrasena))
                    {
                        SymmetricSecurityKey security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));


                        SigningCredentials credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256Signature);

                        var exp = DateTime.Now.AddYears(10);

                        JwtSecurityToken securitytoken = new JwtSecurityToken(

                            issuer: "some",
                            audience: "some",

                            expires: exp,
                            signingCredentials: credentials


                            );


                        var token = new JwtSecurityTokenHandler().WriteToken(securitytoken);

                        Tokenizer = new Token
                        {
                            access_token = token,

                            expires_in = exp
                        };

                    }
                }

            }

          


            return Ok(Tokenizer);


        }


        [HttpPost("/registro",Name ="registro")]
        [AllowAnonymous]
        public async Task<ActionResult> Registro([FromBody] Login login)
        {

            string result = "";
                using (var plaze = new PlaceManager())
                {
                    result = await plaze.registro(login);
                }

            return Ok(result);


        }
    }
}
