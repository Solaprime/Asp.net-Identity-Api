using Asp.netShared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
    /// <summary>
    /// YOu can add more Propertiesd to the User Manager Respos 
    /// </summary>
    public interface IUserService
    {
        Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model);
        Task<UserManagerResponse> LoginUserAsync(LoginViewModel model);
        // start workflow
        Task<UserManagerResponse> CreateRole(string name);
        Task<IEnumerable<IdentityRole>> GetAllRole();
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<UserManagerResponse> AddUserToRole(RoleEmail roleEmail);
        Task<UserManagerResponse> RemoveUserFromRole(RoleEmail roleEmail);
        //Task<IdentityRole> GetUserRoles(string email);


    }
    public class UserService : IUserService
    {

        // recall we registerd identtiy in our startup
        // Identity by default provides with 2 class
        // user manager and Role Mangaer
        private UserManager<IdentityUser> _userManager;
        private IConfiguration _configuration;
        private RoleManager<IdentityRole> _roleManager;


        public UserService(UserManager<IdentityUser> userManager,   IConfiguration configuration,
             RoleManager<IdentityRole> roleManager)
        {
            // the identity is used to manage our user, i.e create a user,
            // update a user passwo4d e.t.c
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }


        public async Task<UserManagerResponse> RegisterUserAsync(RegisterViewModel model)
        {
            if (model == null)
            {
                throw new NullReferenceException("our regsiter model is null");
                // we check if our two entered passsword are the same

            }

            //if (model.Password != model.ConfirmPassword)
            //{
            //    return new UserManagerResponse
            //    {
            //        Message = "Confirm password is not matching with password",
            //        IsSuccess = false,
            //    };
            //}
            //if (Modetate.IsValid)
            //{

            //}
            var identityUser = new IdentityUser()
            {
                // we are pasing some of the defined property in our RegisterviewModel class
                // to the property defined in our  Identityuser CLASS
                // 
                Email = model.Email,
                UserName = model.Email
            };
            // the above method creates the user with the password harsh
            var result = await _userManager.CreateAsync(identityUser, model.Password);
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

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "No user with this Email exist in our Databse",
                    IsSuccess = false
                };
            }
            var result = await _userManager.CheckPasswordAsync(user, model.PassWord);
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

        //Create A role

        public  async Task<UserManagerResponse> CreateRole(string rolename)
        {
            var roleExist = await _roleManager.RoleExistsAsync(rolename);
            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(rolename));
                if (roleResult.Succeeded)
                {
                    return new UserManagerResponse
                    {
                           Message = $"The role {rolename} has been succeede succesfully",
                           IsSuccess = true,
                    };
                }
                else
                {
                    return new UserManagerResponse
                    {
                        IsSuccess = false,
                        Message = $"The role {rolename} was not  created, Kindly try Again"
                    };
                }
            }
            return new UserManagerResponse
            {
                IsSuccess = false,
                Message = "Role already exist"
            };
          
        }

        public async  Task<IEnumerable<IdentityRole>> GetAllRole()
        {
            var allRoles = await  _roleManager.Roles.ToListAsync();

            return allRoles;
        }

        public async  Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users;
        }

        public  async Task<UserManagerResponse> AddUserToRole(RoleEmail roleEmail)
        {
            var user = await _userManager.FindByEmailAsync(roleEmail.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User can not be Found",
                    IsSuccess = false
                };
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleEmail.RoleName);
            if (!roleExist)
            {
                return new UserManagerResponse
                {
                    Message = "Role can not be Found",
                    IsSuccess = false
                };
            }

            var roleToBeAdded = await _userManager.AddToRoleAsync(user, roleEmail.RoleName);
            if (roleToBeAdded.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = $"{roleEmail.RoleName} role was added to {roleEmail.Email} ",
                    IsSuccess = true
                };
            }
            return new UserManagerResponse
            {
                Message = "Something Bad happenes Found",
                IsSuccess = false
            };

        }

        public  async Task<UserManagerResponse> RemoveUserFromRole(RoleEmail roleEmail)
        {

            var user = await _userManager.FindByEmailAsync(roleEmail.Email);
            if (user == null)
            {
                return new UserManagerResponse
                {
                    Message = "User can not be Found",
                    IsSuccess = false
                };
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleEmail.RoleName);
            if (!roleExist)
            {
                return new UserManagerResponse
                {
                    Message = "Role can not be Found",
                    IsSuccess = false
                };
            }
            var result = await _userManager.RemoveFromRoleAsync(user, roleEmail.RoleName);
            if (result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "User was succesfully Remove from  role",
                    IsSuccess = true
                };
            }

            return new UserManagerResponse
            {
                Message = "Some shit happedned Found",
                IsSuccess = false
            };

        }

        //public  async Task<IdentityRole> GetUserRoles(string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //    {
        //        return new UserManagerResponse
        //        {
        //            Message = "User can not be Found",
        //            IsSuccess = false
        //        };
        //    }
        //    var roles = await _userManager.GetRolesAsync(user);

        //}
    }
}




/// the first methpd to create and Account second is to lOGIn



//since we haave to be authenticated to acces weattherforecast controller now, we implement to login function
// to generate acess token..
// then before we can acces the weatherforecast controoller we put the access tokenm genratd 
// when u login to the header of the get Method