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
        private readonly IChatRepository _chatRepository;

        [BindProperty]
        public bool IsReply { get; set; }
        private PostFormService _PostFormService { get; }
        [BindProperty]
        public SnackisUser MyUser { get; set; }
        [BindProperty]
        public Post PostModel { get; set; }
        [BindProperty]
        public Models.Chat ChatModel { get; set; }
        public string Category { get; set; }
        [BindProperty]
        public List<Post> AllPosts { get; set; }
        [BindProperty]
        public Post OriginPost { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ImageURL { get; set; }
        [BindProperty]
        public List<SnackisUser > Users { get; set; }

        public ThreadViewModel(IPostRepository postRepository,
            UserManager<SnackisUser> userManager,
            PostFormService postFormService,
            IChatRepository chatRepository)
        {
            _PostFormService = postFormService;
            _chatRepository = chatRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            Users = _userManager.Users.ToList();
            MyUser = await _userManager.GetUserAsync(User); 
            AllPosts = await _postRepository.GetPosts();
            ViewData["AllPosts"] = AllPosts;
            OriginPost = AllPosts.FirstOrDefault(p => p.Id.ToString() == Request.Cookies["MyThreadCookie"]);
            Category = OriginPost.Category;
            if  (MyUser != null && MyUser.Image != null )
            {
                string imageBase64Data = Convert.ToBase64String(MyUser.Image);
                ImageURL = string.Format($"data:image/jpg;base64, {imageBase64Data}");
            }
            else
            {
                ImageURL = "https://bootdey.com/img/Content/avatar/avatar1.png";
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAddPostAsync()
        {

            if (ModelState.IsValid)
            {
                
                await _postRepository.AddPostAsync(PostModel);
            }
            return RedirectToPage("ThreadView");
        }
        public async Task<IActionResult> OnPostAddChatAsync()
        {
            if (ModelState.IsValid)
            {
                var result=await _chatRepository.PostAsync(ChatModel);
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
