using CrawlerEngine.Common;
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

        public JobHub(string uri = "https://localhost:44361/jobhub")
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

        public async Task SendJobInfo(Guid seq, string jobType, string url)
        {
            if (connection.State == HubConnectionState.Disconnected)
            {
                await connection.StartAsync();
            }            
            await connection.InvokeAsync("SendJobInfo", seq, jobType, url);
        }        
    }
}
