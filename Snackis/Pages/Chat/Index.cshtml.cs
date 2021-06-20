using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Repositories;

namespace Snackis.Pages.Chat
{
    public class IndexModel : PageModel
    {
        private readonly IChatRepository _chatRepository;
        [BindProperty(SupportsGet =true)]
        public string SenderId { get; set; }
        [BindProperty]
        public Models.Chat ChatModel { get; set; } = new();
        [BindProperty]
        public string[] UserId { get; set; }
        [BindProperty(SupportsGet =true)]
        public string GroupChatId { get; set; }

        public IndexModel(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public List<Models.Chat> AllChats { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (UserId != null && UserId.Length==1)
            {
                Console.WriteLine("");
            }
            AllChats = await _chatRepository.GetAllChats(); 
            var unReadChats = AllChats.Where(x => x.IsRead == false).GroupBy(x => x.SenderId).Select(x => new { SenderId = x.Key, Count = x.Count(), Chats = x.ToList() });
            var allConversations = AllChats.GroupBy(x => x.SenderId).Select(x => x.Key).Distinct();
            if (SenderId!= null)
            {
                Response.Cookies.Append("MyChatCookie", $"{SenderId}");
                return RedirectToPage("ChatView");
            }
            if (GroupChatId!= null)
            {
                Response.Cookies.Append("MyGroupChatIdCookie", $"{GroupChatId}");
                return RedirectToPage("Groupchat");
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAddChatA(Guid groupAdminId)
        {
            if (ModelState.IsValid)
            {
                var result = await _chatRepository.PostAsync(ChatModel);
            }
            Response.Cookies.Append("MyChatCookie", $"{SenderId}");

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
