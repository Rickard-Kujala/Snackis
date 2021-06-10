using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Snackis.Areas.Identity.Data;
using Snackis.Data;
using Snackis.Models;
using Snackis.Repositories;
using static System.Net.WebRequestMethods;

namespace Snackis.Pages.Admin
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string RoleName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string AddUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RemoveUserId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Role { get; set; }

        public SnackisUser CurrentUser { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public List<string> UserRoles { get; set; }
        [BindProperty]
        public CategoryModel CategoryModel { get; set; }

        public bool isUser { get; set; }
        [BindProperty]
        public string ForumName { get; set; }
        public bool isAdmin { get; set; }
        [BindProperty]
        public List<Post> Categories { get; set; }
        [BindProperty]
        public List<Post> AllPosts { get; set; }
        public IQueryable<SnackisUser> Users { get; set; }
        [BindProperty(SupportsGet =true)]
        public Guid DeleteCategoryId { get; set; }
        [BindProperty(SupportsGet =true)]
        public Guid DeleteforumId { get; set; }
        [BindProperty(SupportsGet =true)]
        public Guid DeletePostId { get; set; }
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserManager<SnackisUser> _userManager;
        private readonly IPostRepository _postRepository;

        public IndexModel(RoleManager<IdentityRole> roleManager, UserManager<SnackisUser> userManager, IPostRepository postRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _postRepository = postRepository;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            var client = new HttpClient();
            Roles = _roleManager.Roles.ToList();
            Users = _userManager.Users;
            AllPosts =await _postRepository.GetPosts();
            if (DeletePostId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                var postToBeCencured = AllPosts.FirstOrDefault(p=>p.Id==DeletePostId);
                postToBeCencured.Text = "inlägget har tagits bort av admin";
                await _postRepository.UpdatePost(DeletePostId,postToBeCencured);
            }
            if (DeleteCategoryId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                await _postRepository.DeletePostById(DeleteCategoryId);
            }
            if (DeleteforumId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                await _postRepository.DeletePostById(DeleteforumId);

            }
            if (AddUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(AddUserId);
                var roleresult = await _userManager.AddToRoleAsync(alterUser, Role);
            }

            if (RemoveUserId != null)
            {
                var alterUser = await _userManager.FindByIdAsync(RemoveUserId);
                var roleresult = await _userManager.RemoveFromRoleAsync(alterUser, Role);
            }

            CurrentUser = await _userManager.GetUserAsync(User);


            isUser = await _userManager.IsInRoleAsync(CurrentUser, "User");
            isAdmin = await _userManager.IsInRoleAsync(CurrentUser, "Admin");

            return Page();
        }
        public async Task<IActionResult> OnPostAddCategoryAsync()
        {
            if (ModelState.IsValid)
            {
                var category = new Post() { Category = CategoryModel.Name, AbuseReport=false };
                await _postRepository.AddPostAsync(category);

            }
            return RedirectToPage("./index");
        }
        public async Task<IActionResult> OnPostAddForumAsync()
        {
            if (ModelState.IsValid)
            {
                var forum = new Post() { Heading = ForumName, AbuseReport = false };
                await _postRepository.AddPostAsync(forum);

            }
            return RedirectToPage("./index");
        }
        public async Task<IActionResult> OnPostAsync()
        {
            Users = _userManager.Users;
            if (RoleName != null)
            {
                await CreateRole(RoleName);
            }
            return RedirectToPage("./index");
        }
        public async Task CreateRole(string roleName)
        {
            bool exist = await _roleManager.RoleExistsAsync(roleName);
            if (!exist)
            {
                var role = new IdentityRole
                {
                    Name = roleName
                };
                await _roleManager.CreateAsync(role);
            }
        }
       


    }
}
