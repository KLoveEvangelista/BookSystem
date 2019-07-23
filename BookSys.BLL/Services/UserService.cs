using BookSys.BLL.Contacts;
using BookSys.BLL.Helpers;
using BookSys.DAL.Models;
using BookSys.VeiwModel.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookSys.BLL.Services
{
    public class UserService :  IUserService<UserVM, string>
    {
        private ToViewModel toViewModel = new ToViewModel();
        private ToModel toModel = new ToModel();
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public UserService(UserManager<User> userManager, SignInManager<User>signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

        public async Task<ResponseVM> Login(UserVM userVM)
        {
            throw new NotImplementedException();
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

    
    }
}
