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
using Snackis.Models;
using Snackis.Repositories;

namespace Snackis.Pages
{
    public class CategoryViewModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<SnackisUser> _userManager;

        public SnackisUser MyUser { get; set; }

        [BindProperty]
        public Post PostModel { get; set; }
        [BindProperty]
        public string Category { get; set; }

        [BindProperty(SupportsGet =true)]
        public string Thread { get; set; }
        [BindProperty]
        public List<Post> AllPosts { get; set; }
        public CategoryViewModel(IPostRepository postRepository, UserManager<SnackisUser> userManager)
        {
            _postRepository = postRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);

            AllPosts = await _postRepository.GetPosts();
            
            Category= Request.Cookies["MyCategoryCookie"];
            
            if (Thread != null)
            {
                Response.Cookies.Append("MyThreadCookie", $"{Thread}");
                return RedirectToPage("ThreadView");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddPostAsync()
        {
            var client = new HttpClient();

            if (ModelState.IsValid)
            {
                var post = new Post
                {
                    Id = Guid.NewGuid(),
                    Category = PostModel.Category,
                    Title = PostModel.Title,
                    Text = PostModel.Text,
                    DateTime = DateTime.Now,
                    AbuseReport = false,
                    PostParent = PostModel.PostParent,
                    UserId = PostModel.UserId,
                    Nickname = PostModel.Nickname

                };
                await client.PostAsJsonAsync("https://snackis-api.azurewebsites.net/api/post", post);
            }
            return RedirectToPage("Index");
        }
    }
}
