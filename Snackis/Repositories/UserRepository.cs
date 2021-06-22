using Microsoft.AspNetCore.Identity;
using Snackis.Areas.Identity.Data;
using Snackis.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<SnackisUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<SnackisUser> _signInManager;

        public UserRepository(UserManager<SnackisUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPostRepository postRepository,
            SignInManager<SnackisUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }
        public async Task Login(Models.InputModel Input)
        {
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
        }
        public Models.InputModel GetModel()
        {
            var model = new Models.InputModel();
            return model;
        }
        public bool GetUserReactions(Post p, string userId ,string reaction)
        {
            foreach (var postId in p.UserReactions)
            {
                if (postId[0]==userId && postId[1]==reaction)
                {
                    //om användaren har reagerat och värdet är samma "Like", "Dislike"=>avvaiktivera knappen
                    return true;
                }

            }
            return false;
        }
    }
    
}
