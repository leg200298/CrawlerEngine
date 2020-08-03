using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Factory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
   // [Route("api/[controller]")]
    [ApiController]
    public class JobListController : ControllerBase
    {
        //// GET: api/JobList
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET: api/JobList/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/JobList
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}



        [HttpGet]
        [Route("api/v1/CrawlPanel")]
        public IEnumerable<CrawlDataJobListDto> Get()
        {
            CrawlFactory crawlFactory = new CrawlFactory("POSTGRESSQL");
            var response = crawlFactory.CrawlDataJobListRepository
           .GetCrawlDataJobListDtos(2, "");
            //   "select * from crawl_data_job_list where start_time is not null order by start_time desc Fetch first 50 row only");
            return response;
        }

        //[HttpGet]
        //[Route("api/v1/CrawlPanelGetDiffStatus")]
        //public HttpResponseMessage CrawlPanelGetDiffStatus()
        //{
        //    var response = PLDapperHelper.Search<CrawlStatus>(
        //        connectionString,
        //        "select job_status ,count(1) as count from crawl_data_job_list group by job_status");
        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}

        //[HttpPost]
        //[Route("api/v1/CrawlPanel")]
        //public HttpResponseMessage PostCrawlPanel(AskRequest data)
        //{
        //    var command = "1==1";
        //    if (string.IsNullOrEmpty(data.command)) { command = data.command; }
        //    var response = PLDapperHelper.Search<CrawlDataJobListDto>(
        //        connectionString,
        //       $@"select * from crawl_data_job_list 
        //       where start_time is not null 
        //            and {data.command}
        //             order by start_time desc 
        //             Fetch first {data.count} row only");
        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}

        ///// <summary>
        ///// 列表桌遊 單一seqno
        ///// </summary>
        ///// <returns></returns>
        //[Route("api/v1/SamgoSearch")]
        //[HttpGet]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserInfo))]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(Error))]
        //public HttpResponseMessage Get(string keyword)
        //{
        //    var response = DapperHelper.Search<UserInfo>(connectionString, "select * from " + TableName + " where BookName like N'%" + keyword + "%'");
        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}
        ///// <summary>
        ///// 增加桌遊
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("api/v1/Samgo")]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserInfo))]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(Error))]
        //public HttpResponseMessage Post([FromBody]UserInfo value)
        //{
        //     var response = (int)DapperHelper.InsertSQL<UserInfo>(connectionString, TableName, value);
        //    value.SeqNo = response;
        //    return Request.CreateResponse(HttpStatusCode.OK, value);
        //}

        ///// <summary>
        ///// 修改桌遊資料
        ///// </summary>
        ///// <param name="value">修改的資料</param>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("api/v1/Samgo")]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserInfo))]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(Error))]
        //public HttpResponseMessage Put([FromBody]UserInfo value)
        //{
        //    var response = DapperHelper.UpdateSQL<UserInfo>(connectionString, TableName, value);
        //    return Request.CreateResponse(HttpStatusCode.OK, response);
        //}
        ///// <summary>
        ///// 刪除桌遊
        ///// </summary>
        //[HttpDelete]
        //[Route("api/v1/Samgo")]
        //[SwaggerResponse(HttpStatusCode.OK, Type = typeof(UserInfo))]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, Type = typeof(Error))]

        //public HttpResponseMessage Delete([FromBody]UserInfo value)
        //{
        //    var response = DapperHelper.DeleteSQL(connectionString, TableName, value.SeqNo);
        //    return Request.CreateResponse(HttpStatusCode.OK, response);

        //}
      


        public class CrawlStatus
        {
            public string job_status { get; set; }
            public int count { get; set; }

        }

        public class AskRequest
        {
            public string command { get; set; }
            public int count { get; set; }

        }
    }
}
