﻿using CrawlerEngine.Common;
using CrawlerEngine.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace CrawlerEngine.SignalR
{
    public class JobHub : Hub
    {
        private static HubConnection connection;
        private static object jobHubLock = new object();

        public JobHub(string uri = "https://crawlpanel.azurewebsites.net/jobhub")
        {
            lock (jobHubLock)
            {
                if (connection is null)
                {
                    connection = new HubConnectionBuilder()
                    .WithUrl(new Uri(uri))
                    .WithAutomaticReconnect()
                    .Build();
                }
            }
        }

        public async Task SendAddJobInfo(string machineName, string vmIp, Guid seq, string jobType, string url)
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
            }
            await connection.InvokeAsync("SendAddJobInfo", machineName, vmIp, seq, jobType, url);
        }

        public async Task SendRemoveJobInfo(Guid seq)
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
            }
            await connection.InvokeAsync("SendRemoveJobInfo", seq);
        }
    }
}
