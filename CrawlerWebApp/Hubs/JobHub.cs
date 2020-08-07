using CrawlerEngine.Common;
using CrawlerEngine.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CrawlerWebApp.Hubs
{
    public class JobHub : Hub
    {
        public async Task SendJobInfo(Guid seq, string jobType, string url)
        {
            string startTime = DateTime.UtcNow.ToString(RuleString.DateTimeFormat);
            await Clients.All.SendAsync("ReceiveJobInfo", seq, jobType, url, startTime);
        }
    }
}
