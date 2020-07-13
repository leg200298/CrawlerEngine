using System.ComponentModel;

namespace CrawlerEngine.Common.Enums
{
    public static class ElectronicBusiness
    {
        public enum Platform
        {


            #region MOMOShop
            [Description("MOMOSHOP-PRODUCT")]
            MomoShopProduct = 0101,
            [Description("MOMOSHOP-DGRPCATEGORY")]
            MomoShopDgrpCategory = 0102,            
            [Description("MOMOSHOP-LGRPCATEGORY")]
            MomoShopLgrpCategory = 0103,
            [Description("MOMOSHOP-STORE")]
            MomoShopStore = 0104,
            #endregion


            #region Pchome
            [Description("PCHOME24H-PRODUCT")]
            Pchome24hProduct = 0201,
            [Description("PCHOME24H-REGION")]
            Pchome24hRegion = 0202,
            [Description("PCHOME24H-STORE")]
            Pchome24hStore = 0203,
            [Description("PCHOME24H-SIGN")]
            Pchome24hSign = 0204,
            [Description("PCHOME24H-SEARCH")]
            Pchome24hSearch = 0205,

            #endregion
            #region Yahoo
            [Description("YAHOOMALL-PRODUCT")]
            YahooMallProduct = 0301,
            #endregion
        }
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
