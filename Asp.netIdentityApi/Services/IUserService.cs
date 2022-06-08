using Asp.netShared;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
    }
    public class UserService : IUserService
    {

        // recall we registerd identtiy in our startup
        // Identity by default provides with 2 class
        // user manager and Role Mangaer
        private UserManager<IdentityUser> _userManger;
        public UserService(UserManager<IdentityUser> userManager)
        {
            // the identity is used to manage our user, i.e create a user,
            // update a user passwo4d e.t.c
            _userManger = userManager;
        }
        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("our regsiter model is null");
                // we check if our two entered passsword are the same

            }

            if (model.Password != model.ConfirmPassword)
            {
                return new UserManagerResponse
                {
                    Message = "Confirm password is not matching with password",
                    IsSuccess = false,
                };
            }
            var identityUser = new IdentityUser()
            {
                // we are pasing some of the defined property in our RegisterviewModel class
                // to the property defined in our  Identityuser CLASS
                // 
                Email = model.Email,
                UserName = model.Email
            };
            // the above method creates the user with the password harsh
            var result = await _userManger.CreateAsync(identityUser, model.Password);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = " user created succesfully",
                    IsSuccess = true,
                    
                };
            }
            return new UserManagerResponse
            {
                Message = "Unable to create user",
                IsSuccess = false,
                Error = result.Errors.Select(e => e.Description)
            };


            // remenber to resgister thi service in  onu startup
        }
    }
}
