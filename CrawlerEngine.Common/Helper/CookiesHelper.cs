using CrawlerEngine.Common.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.Common.Helper
{
    public class CookiesHelper
    {
        private static Dictionary<Platform, CookiesCache> cookies = new Dictionary<Platform, CookiesCache>();
        private static object cookieLock = new object();

        public static CookieCollection GetCookies(Platform platform)
        {
            lock (cookieLock)
            {
                if (!cookies.ContainsKey(platform)) return null;
                var cookieCache = cookies[platform];
                if ((DateTime.UtcNow - cookieCache.created).Minutes >= cookieCache.expiresMinutes)
                {
                    cookies.Remove(platform); return null;
                }
                return cookieCache.cookieCollection;
            }
        }

        public static void SetCookies(Platform platform, CookieCollection cookieCollection, int expiresMinutes)
        {
            lock (cookieLock)
            {
                cookies[platform] = new CookiesCache(cookieCollection, expiresMinutes);
            }

        }

        public static void SetCookies(Platform platform, Dictionary<string, string> cookieStruct, int expiresMinutes)
        {
            CookieCollection cookieCollection = new CookieCollection();
            foreach (var item in cookieStruct)
            {
                cookieCollection.Add(new Cookie(item.Key, item.Value));
            }
            lock (cookieLock)
            {
                cookies[platform] = new CookiesCache(cookieCollection, expiresMinutes);
            }
        }      
    }

    class CookiesCache
    {
        internal CookieCollection cookieCollection;
        internal DateTime created { get; set; } = DateTime.UtcNow;

        internal int expiresMinutes;
        public CookiesCache(CookieCollection cookieCollection, int expiresMinutes)
        {
            this.cookieCollection = cookieCollection;
            this.expiresMinutes = expiresMinutes;
        }
    }

}
