using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Models.ViewModel
{
    public class AskRequest
    {
        public string database { get; set; }
        public string command { get; set; }
        public int count { get; set; }
    }
}
