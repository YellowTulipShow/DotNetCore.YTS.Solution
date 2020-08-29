using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using YTS.Shop.Models;

namespace YTS.WebApi
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(LoginRequestDTO request, out string token);
    }

    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(IUserService userService, IOptions<TokenManagement> tokenManagement)
        {
            _userService = userService;
            _tokenManagement = tokenManagement.Value;
        }

        public bool IsAuthenticated(LoginRequestDTO request, out string token)
        {
            token = string.Empty;
            Managers manager = _userService.IsValid(request);
            if (manager == null)
                return false;
            var claims = ShopClainInfos(manager);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresTime = DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration);
            var jwtToken = new JwtSecurityToken(_tokenManagement.Issuer, _tokenManagement.Audience, claims,
                expires: expiresTime,
                signingCredentials: credentials);
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }

        public Claim[] ShopClainInfos(Managers manager)
        {
            return new Claim[]
            {
                new Claim(ApiConfig.ClainKey_ManagerID, manager.ID.ToString()),
                new Claim(ApiConfig.ClainKey_ManagerName, manager.Account),
            };
        }
    }
}
