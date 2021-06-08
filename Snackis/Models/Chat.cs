using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Models
{
    public class Chat
    {
        public Guid Id { get; set; }
        public string ReceverId { get; set; }
        public string SenderId { get; set; }
        public List<string> GroupMembers { get; set; } = new();
        public string GroupAdminId { get; set; }
    }
}
