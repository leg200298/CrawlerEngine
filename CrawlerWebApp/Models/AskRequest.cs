using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrawlerWebApp.Models
{
    public class AskRequest
    {
        public string database { get; set; }
        public string command { get; set; }
        public int count { get; set; }
    }
}
