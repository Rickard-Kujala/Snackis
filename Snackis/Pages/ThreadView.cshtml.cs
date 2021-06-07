using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Areas.Identity.Data;
using Snackis.Models;
using Snackis.Repositories;


namespace Snackis.Pages
{
    public class ThreadViewModel : PageModel
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<SnackisUser> _userManager;
        [BindProperty]
        public bool IsReply { get; set; }
        private PostFormService _PostFormService { get; }
        [BindProperty]
        public SnackisUser MyUser { get; set; }
        [BindProperty]
        public Post PostModel { get; set; }
        public string Category { get; set; }
        [BindProperty]
        public List<Post> AllPosts { get; set; }
        [BindProperty]
        public Post OriginPost { get; set; }

        public ThreadViewModel(IPostRepository postRepository, UserManager<SnackisUser> userManager, PostFormService postFormService)
        {
            _PostFormService = postFormService;
            _postRepository = postRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            AllPosts = await _postRepository.GetPosts();
            OriginPost = AllPosts.FirstOrDefault(p => p.Id.ToString() == Request.Cookies["MyThreadCookie"]);
            Category = OriginPost.Category;
           
            return Page();
        }
        public async Task<IActionResult> OnPostAddPostAsync()
        {
            //var client = new HttpClient();
            //MyUser = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                //var post = new Post
                //{
                //    Id = Guid.NewGuid(),
                //    Category = PostModel.Category,
                //    Title = PostModel.Title,
                //    Text = PostModel.Text,
                //    DateTime = DateTime.Now,
                //    AbuseReport = false,
                //    PostParent = PostModel.PostParent,
                //    UserId = PostModel.UserId,
                //    Nickname = PostModel.Nickname
                //};
                //await client.PostAsJsonAsync("https://snackis-api.azurewebsites.net/api/post", post);
                await _postRepository.AddPostAsync(PostModel);
            }
            return RedirectToPage("ThreadView");
        }
        public async Task<IActionResult> OnPostReactAsync(Guid id, string value)
        {
            _PostFormService.Form = Request.Form;
            var updated = await _postRepository.GetPostsById(id);
            switch (value)
            {
                case "Likes":
                    updated.Likes += 1;
                    break;
                case "Dislikes":
                    updated.DisLikes += 1;
                    break;
                case"Abuse":
                    updated.AbuseReport = true;
                    break;
            }
            var result=await _postRepository.UpdatePost(id, updated);
            if (! result.IsSuccessStatusCode)
            {
                return BadRequest();

            }
            return RedirectToPage("ThreadView");
        }
        
    }
}