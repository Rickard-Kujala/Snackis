using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Snackis.Areas.Identity.Data;
using Snackis.Models;
using Snackis.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Snackis.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public SnackisUser MyUser { get; set; }
        public bool AdminExists { get; set; }
        [BindProperty]
        public AdminRegisterModel AdminRegister { get; set; }
        [BindProperty]
        public Post PostModel { get; set; }

        public List<Post> AllPosts { get; set; }
        public List<string> Forums { get; set; }
        [BindProperty(SupportsGet =true)]
        public string Category { get; set; }
        [BindProperty]
        public LoginModel Input { get; set; }
        [BindProperty(SupportsGet =true)]
        public string Forum { get; set; }


        private readonly UserManager<SnackisUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPostRepository _postRepository;
        private readonly Microsoft.AspNetCore.Identity.SignInManager<SnackisUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, UserManager<SnackisUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPostRepository postRepository,
            SignInManager<SnackisUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _postRepository = postRepository;
            _signInManager = signInManager;
        }

        public UserManager<SnackisUser> UserManager { get; }

        public async Task<IActionResult> OnGetAsync()
        {

            MyUser = await _userManager.GetUserAsync(User);
            AdminExists = await _roleManager.RoleExistsAsync("Admin");
            AllPosts = await _postRepository.GetPosts();
            Forums = await _postRepository.GetForums();
            foreach (var forum in Forums)
            {

            }
            ViewData["LoginModel"] = Input;
            ViewData["Forums"] = AllPosts.Where(p => p.Heading != null && p.Text == null).ToList();

            if (Forum !=null)
            {
                Response.Cookies.Append("MyForumCookie", $"{Forum}");
                return RedirectToPage("ForumView");
            }
            return Page();
        }
        public IActionResult OnPost()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostRegisterAdminAsync()
        {
            if (ModelState.IsValid)
            {
                var admin = new SnackisUser { UserName = AdminRegister.Name, Email = AdminRegister.Email };

                var result = await _userManager.CreateAsync(admin, AdminRegister.Password);

                if (result.Succeeded)
                {
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    await _roleManager.CreateAsync(role);

                    await _userManager.AddToRoleAsync(admin, role.Name);
                    return RedirectToPage("./Index");
                }

            }
            return Page();
        }
        public async Task<IActionResult> OnPostLoginAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            //ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }

}

