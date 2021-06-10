using Microsoft.AspNetCore.Identity;
using Snackis.Areas.Identity.Data;
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
            //if (result.Succeeded)
            //{
            //    _logger.LogInformation("User logged in.");
            //    return LocalRedirect(returnUrl);
            //}
            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //}
            //if (result.IsLockedOut)
            //{
            //    _logger.LogWarning("User account locked out.");
            //    return RedirectToPage("./Lockout");
            //}
            //else
            //{
            //    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            //    return Page();
            //}
        }
        public Models.InputModel GetModel()
        {
            var model = new Models.InputModel();
            return model;
        }
    }
    
}
