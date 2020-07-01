using Newtonsoft.Json.Linq;
using System;

namespace CrawlerEngine.Models
{
    public class JobInfo
    {
        public Guid Seq { get; set; }
        public JObject Info { get; set; }
    }
}
