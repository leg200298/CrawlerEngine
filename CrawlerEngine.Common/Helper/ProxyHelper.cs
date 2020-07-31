using System;
using System.Collections.Generic;

namespace CrawlerEngine.Common.Helper
{
    public static class ProxyHelper
    {
        private static List<string> proxylist = new List<string>() {
        $"http://165.225.102.50:10605",
$"https://165.225.102.49:10605",
$"https://165.225.102.54:10605",
$"http://165.225.102.51:10605",
$"http://165.225.102.56:10605",
$"SOCKS4://114.35.188.117:4145",
$"http://118.167.177.104:8080",
  };


        //        private static List<string> proxylist = new List<string>() {
        //        $"http://68.232.175.107:8080",
        ////$"https://140.227.229.208:3128",
        ////$"https://140.227.237.154:1000",
        //$"http://52.179.231.206:80",
        //$"http://80.241.222.137:80",
        ////$"https://148.251.153.226:3128",
        //$"http://196.27.119.131:80",
        //$"http://91.205.174.26:80",
        ////$"https://95.217.120.170:8888"
        //  };
        public static string GetUsableProxy()
        {
            var getRandomOne = new Random().Next(0, proxylist.Count - 1);
            return proxylist[getRandomOne];
        }
    }
}
