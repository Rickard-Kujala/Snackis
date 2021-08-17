using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Snackis.Areas.Identity.Data;
using Snackis.Repositories;

namespace Snackis.Pages.Chat
{
    public class ChatViewModel : PageModel
    {
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<SnackisUser> _userManager;

        [BindProperty]
        public string SenderId { get; set; }
        public SelectList Users { get; set; }
        public List<Models.Chat> AllChats { get; set; }
        public List<Models.Chat> AllMessages { get; set; }

        public SnackisUser MyUser { get; set; }
        [BindProperty]
        public Models.Chat ChatModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public string GroupChatId { get; set; }

        public ChatViewModel(IChatRepository chatRepository, UserManager<SnackisUser> userManager)
           
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            MyUser = await _userManager.GetUserAsync(User);
            SenderId = Request.Cookies["MyChatCookie"];

            AllChats = await _chatRepository.GetAllChats();
            AllMessages=AllChats.Where(c => c.SenderId == SenderId || c.ReceiverId == SenderId).ToList();
            foreach (var message in AllChats)
            {
                if (message.IsRead!=true)
                {
                    message.IsRead = true;
                   var response=await _chatRepository.UpdateChatAsync(message.Id, message);
                }
            }
            if (GroupChatId != null)
            {
                Response.Cookies.Append("MyGroupChatIdCookie", $"{GroupChatId}");
                return RedirectToPage("Groupchat");
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAddChatAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _chatRepository.PostAsync(ChatModel);
            }
            return RedirectToPage("ChatView");
        }
        public async Task<IActionResult> OnPostNewGroupChat(Guid groupAdminId)
        {
            if (ModelState.IsValid)
            {
                var result = await _chatRepository.PostAsync(ChatModel);
            }
            Response.Cookies.Append("MyChatCookie", $"{SenderId}");

            return RedirectToPage("Index");
        }
    }
}
