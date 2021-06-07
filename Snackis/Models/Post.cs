using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Nickname { get; set; }
        public string Heading { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public bool AbuseReport { get; set; }
        public string PostParent { get; set; }
        public int Likes { get; set; }
        public int DisLikes { get; set; }
        public int Replyes { get; set; }
        public int Threads { get; set; }
    }
}
