using Microsoft.AspNetCore.Identity;
using Store.Data.Entity.IdentityEntity;
using Store.Service.Services.TokenService;
using Store.Service.Services.UserService.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.UserService
{
    public class UserServic : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;

        public UserServic(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService token)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
        }
        public async Task<UserDto> LogIn(LoginDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);

            if (user is  null)
                return null;

            var result = await _signInManager.CheckPasswordSignInAsync(user,input.Password,false);

            if (!result.Succeeded)
            {
                throw new Exception("LogIn Failed");
            }
            return new UserDto
            {
                Id = Guid.Parse(user.Id),
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _token.GenerateToken(user)
            };
        }

        public async Task<UserDto> Register(RegisterDto input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);

            if (user is not null)
                return null;

            var Appuser = new AppUser
            {
                DisplayName = input.DisplayName,
                Email = input.Email,
                UserName = input.DisplayName,
            };

            var result = await _userManager.CreateAsync(Appuser, input.Password);

            if (!result.Succeeded)
                throw new Exception(result.Errors.Select(x => x.Description).FirstOrDefault());

            return new UserDto
            {
                Id = Guid.Parse(Appuser.Id),
                DisplayName = Appuser.DisplayName,
                Email = Appuser.Email,
                Token = _token.GenerateToken(Appuser)
            };
        }
    }
}
