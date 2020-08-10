using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Factory;
using CrawlerWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

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
        public IEnumerable<CrawlDataJobListDto> CrawlPanel(string database)
        {
            crawlFactory = new CrawlFactory(database);
            var response = crawlFactory.CrawlDataJobListRepository
           .GetCrawlDataJobListDtos(2, "");
            return response;
        }

        [HttpPost]       
        public IActionResult CrawlPanelGetDiffStatus([FromBody] AskRequest data)
        {
            crawlFactory = new CrawlFactory(data.database);
            var response = crawlFactory.CrawlDataJobListRepository.GetCrawlDataJobDiffStatus();
            return Json(response);
        }

        [HttpPost]        
        public IActionResult CrawlPanel([FromBody] AskRequest data)
        {
            crawlFactory = new CrawlFactory(data.database);
            var command = "1==1";
            if (string.IsNullOrEmpty(data.command)) { command = data.command; }
            var response = crawlFactory.CrawlDataJobListRepository
                            .GetCrawlDataJobListDtos(data.command, data.count);
            return Json(response);
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
