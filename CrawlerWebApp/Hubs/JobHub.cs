﻿using CrawlerEngine.Common;
using CrawlerEngine.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace CrawlerWebApp.Hubs
{
    public class JobHub : Hub
    {
        public async Task SendAddJobInfo(Guid seq, string jobType, string url)
        {
            string startTime = DateTime.UtcNow.ToString(RuleString.DateTimeFormat);
            await Clients.All.SendAsync("ReceiveAddJobInfo", seq, jobType, url, startTime);
        }

        public async Task SendRemoveJobInfo(Guid seq)
        {
            string startTime = DateTime.UtcNow.ToString(RuleString.DateTimeFormat);
            await Clients.All.SendAsync("ReceiveRemoveJobInfo", seq);
        }
    }
}
