using CrawlerEngine.Common;
using CrawlerEngine.Common.Extansion;
using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.JobWorker.WorkClass;
using CrawlerEngine.Models;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker
{
    public class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo)
        {

            RuleString ruleString = new RuleString();

            var jobType = jobInfo.JobType.ToUpper();

            #region Momo           

            if (jobType == Platform.MomoShopStore.GetDescription())
            {
                return new MomoShopStoreJobWorker(jobInfo);
            }
            if (jobType == Platform.MomoShopLgrpCategory.GetDescription())
            {
                return new MomoShopLgrpCategoryJobWorker(jobInfo);
            }           
            if (jobType == Platform.MomoShopDgrpCategory.GetDescription())
            {
                return new MomoShopDgrpCategoryJobWorker(jobInfo);
            }
            if (jobType == Platform.MomoShopProduct.GetDescription())
            {
                return new MomoShopProductJobWorker(jobInfo);
            }

            #endregion

            #region Pchome 24h
            if (jobType == Platform.Pchome24hProduct.GetDescription())
            {
                return new Pchome24hProductJobWorker(jobInfo);
            }
            if (jobType == Platform.Pchome24hRegion.GetDescription())
            {
                return new Pchome24hRegionJobWorker(jobInfo);
            }
            if (jobType == Platform.Pchome24hStore.GetDescription())
            {
                return new Pchome24hStoreJobWorker(jobInfo);
            }
            if (jobType == Platform.Pchome24hSign.GetDescription())
            {
                return new Pchome24hSignJobWorker(jobInfo);
            }
            if (jobType == Platform.Pchome24hSearch.GetDescription())
            {
                return new Pchome24hSearchJobWorker(jobInfo);
            }

            #endregion

            #region yahoo

            if (jobType == Platform.YahooMallProduct.GetDescription())
            {
                return new YaooMallProductJobWorker(jobInfo);
            }
            #endregion
            return null;

            // todo list
            /*
             PC Home 陣營電商通路平台：
PC Home 24h 購物中心（入倉）
https://24h.pchome.com.tw/index/
PC Home購物中心
https://mall.pchome.com.tw/index/
PC Home 商店街
http://www.pcstore.com.tw/
露天拍賣市集
https://www.ruten.com.tw/

momo 陣營電商通路平台：
momo購物網（入倉）
https://www.momoshop.com.tw/main/Main.jsp
momo摩天商城
https://www.momomall.com.tw/main/Main.jsp

蝦皮陣營電商通路平台：
蝦皮拍賣
https://shopee.tw/
蝦皮商城
https://shopee.tw/shopee24h
蝦皮24小時（入倉）
https://shopee.tw/shopee24h

Yahoo陣營電商通路平台
Yahoo奇摩購物中心
https://tw.buy.yahoo.com/
Yahoo超級商城
https://tw.mall.yahoo.com/
Yahoo奇摩拍賣
https://tw.bid.yahoo.com/

森森購物網
http://www.u-mall.com.tw/Pages/HomePage.aspx

udn買東西
https://shopping.udn.com/mall/Cc1a00.do

樂天電商通路平台
https://www.rakuten.com.tw/

博客來電商通路平台
http://www.books.com.tw/

Pinkoi 品設計電商通路平台
https://www.pinkoi.com/

生活市集
https://www.buy123.com.tw/

好吃宅配網
https://www.food123.com.tw/

             */

        }
    }
}
