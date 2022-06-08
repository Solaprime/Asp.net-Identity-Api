using Asp.netShared;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Asp.netIdentityApi.Services
{
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
    }
    public class UserService : IUserService
    {

        // recall we registerd identtiy in our startup
        // Identity by default provides with 2 class
        // user manager and Role Mangaer
        private UserManager<IdentityUser> _userManger;
        private IConfiguration _configuration;

        public UserService(UserManager<IdentityUser> userManager,   IConfiguration configuration)
        {
            // the identity is used to manage our user, i.e create a user,
            // update a user passwo4d e.t.c
            _userManger = userManager;
            _configuration = configuration;
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


        public  async Task<UserManagerResponse> LoginUserAsync(LoginViewModel model)
        {
            // this function will take the email and check if any user with that email exist
            // and then check password, if email and password pair exist in the database 
            // an access token will then be genreated and sent to the user with the usermanger response
            // inside the message property of the usermanager response

            var user = await _userManger.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "No user with this Email exist in our Databse",
                    IsSuccess = false
                };
            }
            var result = await _userManger.CheckPasswordAsync(user, model.PassWord);
            if (!result )
            {
                return new UserManagerResponse
                {
                    Message = "Invalid Password ",
                    IsSuccess = false
                };
            }
            // if user credentilas are right thennn we need to generate the access token,
            // the acces token will hold the claims{More specfialcally an Array of Claims},
            //the claims is basically just a user related information, like th allowed permission of the user

            var claims = new[]
            {
                // there are bunch if already [predefine types in the Claim types
                new Claim ("Email", model.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
            };
            //Enc

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["AuthSettings:Key"]));

            // time to genrate the token
            var token = new JwtSecurityToken(
                issuer: _configuration["AuthSettings:Issuer"],
                audience: _configuration["AuthSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );


            // Convert the token to string 
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);

            return new UserManagerResponse
            {
                Message = tokenAsString,
                IsSuccess = true,
                ExpiredDate = token.ValidTo
            };

        }
    }
}




/// the first methpd to create and Account second is to lOGIn



//since we haave to be authenticated to acces weattherforecast controller now, we implement to login function
// to generate acess token..
// then before we can acces the weatherforecast controoller we put the access tokenm genratd 
// when u login to the header of the get Method