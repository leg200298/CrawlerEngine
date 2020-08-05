using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CrawlerWebApp.Models;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Factory;
using System.Net.Http;
using CrawlerEngine.Models.ViewModel;
using System.Net;
using Newtonsoft.Json;

namespace CrawlerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CrawlFactory crawlFactory;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }

        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpGet]
        [Route("api/v1/CrawlPanel")]
        public IEnumerable<CrawlDataJobListDto> Get(string database)
        {
            crawlFactory = new CrawlFactory(database);
            var response = crawlFactory.CrawlDataJobListRepository
           .GetCrawlDataJobListDtos(2, "");
            return response;
        }

        [HttpPost]
        [Route("api/v1/CrawlPanelGetDiffStatus")]
        public IActionResult CrawlPanelGetDiffStatus([FromBody] AskRequest data)
        {
            crawlFactory = new CrawlFactory(data.database);
            var response = crawlFactory.CrawlDataJobListRepository.GetCrawlDataJobDiffStatus();
            return Ok(response);
        }

        [HttpPost]
        [Route("api/v1/CrawlPanel")]
        public IActionResult PostCrawlPanel([FromBody] AskRequest data)
        {
            crawlFactory = new CrawlFactory(data.database);
            var command = "1==1";
            if (string.IsNullOrEmpty(data.command)) { command = data.command; }
            var response = crawlFactory.CrawlDataJobListRepository
                            .GetCrawlDataJobListDtos(data.command, data.count);
            return Ok(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
