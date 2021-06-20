using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public List<string> GroupMembers { get; set; } = new();
        public string GroupAdminId { get; set; }
        public string GroupChatTitle { get; set; }
        public string ParentPost { get; set; }

    }
}
