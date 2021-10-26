using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using ArenaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ArenaApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class TokenController : Controller
    {
        private static readonly Dictionary<string, string> Users = new(){
            {"lorenzobasoc", "ciao"},
        };

        [HttpPost]
        public ObjectResult Create(User user){
            if (IsValidUserAndPass(user)){
                return new ObjectResult(GenerateToken(user.Username));
            } else {
                throw new InvalidOperationException("L'username o la password non sono corretti.");
            }
        }

        private static bool IsValidUserAndPass(User user){
            return Users.Any(u => u.Key == user.Username && u.Value == user.Password);
        }

        private static dynamic GenerateToken(string username){
            var claims = new List<Claim>{
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddMonths(1)).ToUnixTimeSeconds().ToString()),
            };
            var token = new JwtSecurityToken(
                new JwtHeader(
                    new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ciaggggggggggggggggggggggggggggggo")),
                        SecurityAlgorithms.HmacSha256)),
                new JwtPayload(claims));
            var output = new{
                Access_Token = new JwtSecurityTokenHandler().WriteToken(token),
                Username = username,
            };
            return output;
        }
    }
}