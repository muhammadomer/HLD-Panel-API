using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DataAccess;
using DataAccess.EntityModel;
using DataAccess.Helper;
using DataAccess.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HLD.WebApi.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class ConfigurationController : ControllerBase
    {
        UserDataAccess userDataAccess = null;
        private readonly JwtAppSetting _jwtAppSetting;

        public ConfigurationController(IConfiguration configuration, IOptions<JwtAppSetting> jwtAppSetting, IConnectionString connectionString)
        {
            this._jwtAppSetting = jwtAppSetting.Value;
            userDataAccess = new UserDataAccess(connectionString);
        }
        [HttpPost]
        [Route("api/Configuration")]
        public IActionResult Post([FromBody]AuthenticateViewModel authenticateVM)
        {
            try
            {
                AuthenticateViewModel authenticateViewModel = null;
                authenticateViewModel = userDataAccess.AuthenticateUser_Managed(authenticateVM.Username, authenticateVM.Password);
                if (authenticateViewModel == null)
                    return BadRequest(new AuthenticateViewModel() { Message = "User Name or Password is Invalid", status = false });
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtAppSetting.SigningKey);
                DateTime expires = DateTime.UtcNow.AddYears(2);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, authenticateViewModel.Id.ToString())
                    }),
                    Expires = expires,
                    Audience = _jwtAppSetting.Site,
                    Issuer = _jwtAppSetting.Site,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return Ok(new AuthenticateViewModel()
                {
                    Id = authenticateViewModel.Id,
                    Username = authenticateViewModel.Username,
                    Method = authenticateViewModel.Method,


                    Token = tokenString,
                    expiration = token.ValidTo,
                    Message = "User is Authenticated",
                    status = true
                }) ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        [Route("api/Configurations")]
        public IActionResult Posts([FromBody] AuthenticateViewModel authenticateVM)
        {
            try
            {
                AuthenticateViewModel authenticateViewModel = null;
                authenticateViewModel = userDataAccess.AuthenticateUser_Manageds(authenticateVM.Username);
                if (authenticateViewModel == null)
                    return BadRequest(new AuthenticateViewModel() { Message = "User Name or Password is Invalid", status = false });
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtAppSetting.SigningKey);
                DateTime expires = DateTime.UtcNow.AddHours(10);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                    new Claim(ClaimTypes.Name, authenticateViewModel.Id.ToString())
                    }),
                    Expires = expires,
                    Audience = _jwtAppSetting.Site,
                    Issuer = _jwtAppSetting.Site,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);
                // return basic user info (without password) and token to store client side
                return Ok(new AuthenticateViewModel()
                {
                    Id = authenticateViewModel.Id,
                    Username = authenticateViewModel.Username,
                    Method = authenticateViewModel.Method,
                    Token = tokenString,
                    expiration = token.ValidTo,
                    Message = "User is Authenticated",
                    status = true
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]     
        [Route("api/Configuration/{Email}/{Checkboxstatus}")]
        //[Route("api/Configuration")]
        public IActionResult SaveCheckboxstatus(string Email, bool Checkboxstatus)
        {
            bool status = false;
            status = userDataAccess.SaveCheckboxstatus(Email, Checkboxstatus);
            return Ok(status);
            
        }
        [HttpGet]
        //[Authorize]
        [Route("api/Configuration/{userid}")]
        public List<Login> GetCheckboxstatus(string userid)
        {
            List<Login> List = new List<Login>();
            try
            {
                List = userDataAccess.GetCheckboxstatus(userid);
              //  return null;
                return List;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}