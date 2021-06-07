using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snackis.Models
{

    public class PostResponse
    {
        public Class1[] Property1 { get; set; }
    }

    public class Class1
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string category { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public DateTime dateTime { get; set; }
        public bool abuseReport { get; set; }
        public string postParent { get; set; }
    }

}
