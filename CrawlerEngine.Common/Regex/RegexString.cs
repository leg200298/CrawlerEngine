using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlerEngine.Common
{
    public class RegexString
    {
        public string isPrice = @"\d+";
        public string isPriceMatch = @"\D{1,3}(價|價格)\${0,1}\d+,{0,1}\d+";
        public string isXZeroFee = @"\d{1,2}期0利率";
        public string nBank = @"\d{1,2}家";
        public string isATM = @"ATM";
        public string getPayment = @"\D{2}(付款|付清|分期)|\D{0,10}Pay|\D{2}卡|\D{2}支付|ibon|FamiPort";
        public string isBank = @"\D{2,4}銀行\D{0,5}";
        public string splitPay = @"每期\d+元";
        public string getWarranty = @"\d{1,2}(年|月)保(固|固期)";
        public string getSplitPayPer = @"\${0,1}\d{0,3},{0,1}\d{0,3},{0,1}\d+\/(期)";
    }
}
