using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis.Repositories;

namespace Snackis.Pages.Chat
{
    public class GroupChatModel : PageModel
    {
        private readonly IChatRepository _chatRepository;

        [BindProperty(SupportsGet = true)]
        public string NewGroupmemberId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string DeleteGroupmemberId { get; set; }
        [BindProperty(SupportsGet =true)]
        public string GroupChatId { get; set; }
        [BindProperty]
        public string CurrentChatId { get; set; }
        [BindProperty]
        public List<Models.Chat> AllGroupChats { get; set; }
        [BindProperty]
        public List<Models.Chat> AllChats { get; set; }
        [BindProperty]
        public Models.Chat ChatModel { get; set; }
        [BindProperty]
        public Models.Chat ParentChat { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SenderId { get; set; }


        public GroupChatModel(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }
        public async Task <IActionResult> OnGetAsync()
        {

            CurrentChatId = Request.Cookies["MyGroupChatIdCookie"];
            if (GroupChatId != null)    
            {
                Response.Cookies.Append("MyGroupChatIdCookie", $"{GroupChatId}");
                CurrentChatId = GroupChatId;

            }
            if (SenderId != null)
            {
                Response.Cookies.Append("MyChatCookie", $"{SenderId}");
                return RedirectToPage("ChatView");
            }
            AllChats = await _chatRepository.GetAllChats();
            //AllChats = AllChats.Where(x=>x.Id.ToString() == CurrentChatId).ToList();
            ParentChat = AllChats.FirstOrDefault(c => c.Id.ToString()==CurrentChatId && c.GroupAdminId != null && c.Text == null && c.ParentPost == null);
            AllGroupChats = AllChats.Where(c=>c.ParentPost != null &&  c.ParentPost.ToString()==CurrentChatId).ToList();

            if (NewGroupmemberId!= null)
            {
                var updatedChat = ParentChat;
                updatedChat.GroupMembers.Add(NewGroupmemberId);
                updatedChat.GroupMembers = updatedChat.GroupMembers.Distinct().ToList();
                await _chatRepository.UpdateChatAsync(updatedChat.Id, updatedChat);
            }
            if (DeleteGroupmemberId != null)
            {
                var updatedChat = AllChats.FirstOrDefault(c => c.Id.ToString() == GroupChatId);
                updatedChat.GroupMembers.Remove(DeleteGroupmemberId);
                await _chatRepository.UpdateChatAsync(updatedChat.Id, updatedChat);
            }
           
            return Page();
        }
        public async Task<IActionResult> OnPostAddChatAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _chatRepository.PostAsync(ChatModel);
            }
            return RedirectToPage("Groupchat");
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
