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
        public List<string> Categories { get; set; }
        [BindProperty(SupportsGet =true)]
        public string Category { get; set; }
        private readonly UserManager<SnackisUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IPostRepository _postRepository;

        public IndexModel(ILogger<IndexModel> logger, UserManager<SnackisUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IPostRepository postRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _postRepository = postRepository;
        }

        public UserManager<SnackisUser> UserManager { get; }

        public async Task<IActionResult> OnGetAsync()
        {

            MyUser = await _userManager.GetUserAsync(User);
            AdminExists = await _roleManager.RoleExistsAsync("Admin");
            AllPosts = await _postRepository.GetPosts();
            Categories = await _postRepository.GetCategories();

            if (Category!=null)
            {
                Response.Cookies.Append("MyCategoryCookie", $"{Category}");
                return RedirectToPage("CategoryView");
            }
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
        
    }
}
