using System.Collections.Generic;
using System.Linq;

namespace CrawlerEngine.Models
{
    public class Options
    {

        public Dictionary<string, object> Info { get; set; }
        protected string GetString(string key)
        {

            if (!Info.Keys.Contains(key))
            {
                return null;
            }
            return Info[key].ToString();
        }
        protected void Put(string key, object value)
        {
            Info[key] = value;
        }

    }
}
