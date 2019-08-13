using BookSys.BLL.Contracts;
using BookSys.BLL.Helpers;
using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BookSys.BLL.Services
{
    public class UserService :  IUserService<UserVM, LoginVM, string>
    {
        private ToViewModel toViewModel;
        private ToModel toModel;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly ApplicationSettingsVM _applicationSettings;

        public UserService(UserManager<User> userManager, SignInManager<User>signInManager, IOptions<ApplicationSettingsVM> appSettings)
        {
            toViewModel = new ToViewModel();
            toModel = new ToModel();
            _userManager = userManager;
            _signInManager = signInManager;
            _applicationSettings = appSettings.Value;
        }

        public async Task<ResponseVM> Register(UserVM userVM)
        {
            var user = new User()
            {
                UserName = userVM.UserName,
                FirstName = userVM.Firstname,
                Middlename = userVM.MiddleName,
                LastName = userVM.LastName
            };

            try
            {
                var result = await _userManager.CreateAsync(user, userVM.Password);
                await _userManager.AddToRoleAsync(user, userVM.Role);

                if (result.Succeeded)
                    return new ResponseVM("created", true, "User");
                else
                    return new ResponseVM("created", false, "User", "", "",error: result.Errors);
            }
            catch (Exception ex)
            {
                return new ResponseVM("created", false, "User", ResponseVM.SOMTHING_WENT_WRONG, "", ex);
            }
        }

        public async Task<ResponseVM> Delete (string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseVM> Update(UserVM userVM)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseVM> Login(LoginVM loginVM)
        {
            var user = await _userManager.FindByNameAsync(loginVM.UserName);
            var userFound = await _userManager.CheckPasswordAsync(user, loginVM.Password);
            if (user != null && userFound)
            {
                //Get role assigned to the user
                var role = await _userManager.GetRolesAsync(user);
                IdentityOptions _options = new IdentityOptions();

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(_options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_applicationSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return new ResponseVM("authenticated", true, "User", "Login succcess!", token);
            }
            else
                return new ResponseVM("authenticated", false, "User", "Username or password is incorrect.");
        }


        public async Task<ResponseVM> Validate(UserVM userVM)
        {
            throw new NotImplementedException();
        }
        public async Task<ResponseVM> Deactivate(UserVM userVM)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserVM>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<UserVM> GetSingleBy(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<UserVM>UserProfile(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userVm = toViewModel.User(user);
            return userVm;
        }
    }
}
