using CrawlerEngine.JobWorker.Interface;
using CrawlerEngine.Models;

namespace CrawlerEngine.JobWorker
{
    public class JobWorkerFactory
    {
        public IJobWorker GetJobWorker(JobInfo jobInfo)
        {
            var jobType = jobInfo.JobType;

            switch (jobType.ToUpper())
            {
                #region Momo
                case "MOMO-PRODUCT":
                    return new WorkClass.Momo.ProductJobWorker(jobInfo);
                #endregion

                #region Pchome
                case "PCHOME-PRODUCT":
                    return new WorkClass.Pchome.ProductJobWorker(jobInfo);
                case "PCHOME-REGION":
                    return new WorkClass.Pchome.RegionJobWorker(jobInfo);
                case "PCHOME-STORE":
                    return new WorkClass.Pchome.StoreJobWorker(jobInfo);
                #endregion


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
                default:
                    return null;
                    break;
            }
        }
    }
}
