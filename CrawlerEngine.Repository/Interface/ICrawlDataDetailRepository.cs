using CrawlerEngine.Model.DTO;
using CrawlerEngine.Models;
using System.Collections.Generic;

namespace CrawlerEngine.Repository.Interface
{
    public interface ICrawlDataDetailRepository
    {

         IEnumerable<CrawlDataDetailDto> GetDataDetailDtos();

         int InsertDataDetail(CrawlDataDetailDto crawlDataDetailDto);
    }
}
