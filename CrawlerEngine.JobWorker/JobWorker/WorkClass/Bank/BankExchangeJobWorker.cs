using CrawlerEngine.Common.Extansion;
using CrawlerEngine.Common.Helper;
using CrawlerEngine.Crawler.Interface;
using CrawlerEngine.Crawler.WorkClass;
using CrawlerEngine.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading;
using static CrawlerEngine.Common.Enums.ElectronicBusiness;

namespace CrawlerEngine.JobWorker.WorkClass
{

    /// <summary>
    /// 館分類頁
    /// </summary>
    class BankExchangeJobWorker : JobWorkerBase
    {
        private List<JobInfo> jobInfos = new List<JobInfo>();
        private HtmlDocument htmlDoc = new HtmlDocument();
        public BankExchangeJobWorker(JobInfo jobInfo)
        {
            this.jobInfo = jobInfo;
            crawler = new HttpCrawler(jobInfo);
        }
        public override JobInfo jobInfo { get; set; }
        public override ICrawler crawler { get; set; }

        protected override bool GotoNextPage(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            else
            {
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfo.Url = $"https:{url}";
                }

                return true;
            }
        }

        protected override bool Crawl()
        {
            var success = false;
            try
            {
                responseData = crawler.DoCrawlerFlow();
                success = true;
            }
            catch (Exception ex)
            {
                LoggerHelper._.Error(ex);
            }
            return success;
        }
        protected override bool Validate()
        {
            if (string.IsNullOrEmpty(responseData))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override bool Parse()
        {

            htmlDoc.LoadHtml(responseData);
            #region region
            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"ToothContainer\"]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hRegion.GetDescription(), Url = $"https:{url}" });
                }
            }
            #endregion store
            nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"BLK10\"]/dl/dd[2]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hStore.GetDescription(), Url = $"https:{url}" });
                }
            }
            nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"BLK09\"]/dl/dd[2]/div/ul/li/a");
            if (nodes is null) { return false; }
            foreach (var data in nodes)
            {
                var url = data.Attributes["href"].Value;
                if (url.StartsWith("//24h.pchome.com.tw"))
                {
                    jobInfos.Add(new JobInfo() { JobType = Platform.Pchome24hStore.GetDescription(), Url = $"https:{url}" });
                }
            }
            return true;
        }

        protected override bool SaveData()
        {

            foreach (var d in jobInfos)
            {
                Repository.Factory.CrawlFactory.CrawlDataJobListRepository.InsertOne(d);
            }
            return true;

        }

        protected override (bool, string) HasNextPage()
        {

            //htmlDoc.LoadHtml(responseData);

            //var nodes = htmlDoc.DocumentNode.SelectNodes("//*[@id=\"PaginationContainer\"]/ul/li/a");

            //foreach (var node in nodes)
            //{
            //    if (node.InnerText == "下一頁")
            //    {
            //        return (true, node.Attributes["href"].Value);
            //    }
            //}
            return (false, "");
        }

        protected override void SleepForAWhile(decimal sleepTime)
        {
            Thread.Sleep((int)(sleepTime * 1000));
        }


    }


    public class Rootobject
    {
        public USDECS USDECS { get; set; }
        public USDNAD USDNAD { get; set; }
        public USDGTQ USDGTQ { get; set; }
        public USDCOP USDCOP { get; set; }
        public USDCAD USDCAD { get; set; }
        public USDJOD USDJOD { get; set; }
        public USDHTG USDHTG { get; set; }
        public USDUAH USDUAH { get; set; }
        public USDMMK USDMMK { get; set; }
        public USDETB USDETB { get; set; }
        public USDAMD USDAMD { get; set; }
        public USDUYU USDUYU { get; set; }
        public USDXAF USDXAF { get; set; }
        public USDDKK USDDKK { get; set; }
        public USDKPW USDKPW { get; set; }
        public USDSSP USDSSP { get; set; }
        public USDXAU USDXAU { get; set; }
        public USDGBP USDGBP { get; set; }
        public USDPGK USDPGK { get; set; }
        public USDNOK USDNOK { get; set; }
        public USDTHB USDTHB { get; set; }
        public USDGMD USDGMD { get; set; }
        public USDRUB USDRUB { get; set; }
        public USDILS USDILS { get; set; }
        public USDPKR USDPKR { get; set; }
        public USDSBD USDSBD { get; set; }
        public USDHNL USDHNL { get; set; }
        public USDOMR USDOMR { get; set; }
        public USDERN USDERN { get; set; }
        public USDBGN USDBGN { get; set; }
        public USDIMP USDIMP { get; set; }
        public USDEGP USDEGP { get; set; }
        public USDNGN USDNGN { get; set; }
        public USDITL USDITL { get; set; }
        public USDBDT USDBDT { get; set; }
        public USDYER USDYER { get; set; }
        public USDKGS USDKGS { get; set; }
        public USDTMT USDTMT { get; set; }
        public USDLTC USDLTC { get; set; }
        public USDTWD USDTWD { get; set; }
        public USDLAK USDLAK { get; set; }
        public USDCDF USDCDF { get; set; }
        public USDJMD USDJMD { get; set; }
        public USDAOA USDAOA { get; set; }
        public USDSDG USDSDG { get; set; }
        public USDLVL USDLVL { get; set; }
        public USDHKD USDHKD { get; set; }
        public USDBZD USDBZD { get; set; }
        public USDVES USDVES { get; set; }
        public USDBRX USDBRX { get; set; }
        public USDXPD USDXPD { get; set; }
        public USDXCD USDXCD { get; set; }
        public USDZMW USDZMW { get; set; }
        public USDMRO USDMRO { get; set; }
        public USDBYN USDBYN { get; set; }
        public USDNPR USDNPR { get; set; }
        public USDHRK USDHRK { get; set; }
        public USDSGD USDSGD { get; set; }
        public USDTRY USDTRY { get; set; }
        public USDDZD USDDZD { get; set; }
        public USD USD { get; set; }
        public USDXOF USDXOF { get; set; }
        public USDUGX USDUGX { get; set; }
        public USDKYD USDKYD { get; set; }
        public USDKES USDKES { get; set; }
        public USDIEP USDIEP { get; set; }
        public USDBHD USDBHD { get; set; }
        public USDKHR USDKHR { get; set; }
        public USDSOS USDSOS { get; set; }
        public USDIDR USDIDR { get; set; }
        public USDMOP USDMOP { get; set; }
        public USDAFN USDAFN { get; set; }
        public USDMGA USDMGA { get; set; }
        public USDBYR USDBYR { get; set; }
        public USDBRL USDBRL { get; set; }
        public USDSAR USDSAR { get; set; }
        public USDHUF USDHUF { get; set; }
        public USDXAG USDXAG { get; set; }
        public USDGIP USDGIP { get; set; }
        public USDMDL USDMDL { get; set; }
        public USDCRC USDCRC { get; set; }
        public PLATINUM1UZ999 PLATINUM1UZ999 { get; set; }
        public USDDOGE USDDOGE { get; set; }
        public USDCHF USDCHF { get; set; }
        public USDBOB USDBOB { get; set; }
        public USDNIO USDNIO { get; set; }
        public USDGEL USDGEL { get; set; }
        public USDSTN USDSTN { get; set; }
        public USDANG USDANG { get; set; }
        public USDMNT USDMNT { get; set; }
        public USDDOP USDDOP { get; set; }
        public USDTND USDTND { get; set; }
        public USDBWP USDBWP { get; set; }
        public USDMVR USDMVR { get; set; }
        public USDLSL USDLSL { get; set; }
        public USDSLL USDSLL { get; set; }
        public USDLRD USDLRD { get; set; }
        public USDSCR USDSCR { get; set; }
        public USDSVC USDSVC { get; set; }
        public USDPYG USDPYG { get; set; }
        public USDAUD USDAUD { get; set; }
        public USDSHP USDSHP { get; set; }
        public USDHUX USDHUX { get; set; }
        public USDVEF USDVEF { get; set; }
        public USDJEP USDJEP { get; set; }
        public USDMXN USDMXN { get; set; }
        public USDAZN USDAZN { get; set; }
        public USDXPT USDXPT { get; set; }
        public USDMWK USDMWK { get; set; }
        public USDMZN USDMZN { get; set; }
        public USDCNH USDCNH { get; set; }
        public USDBSD USDBSD { get; set; }
        public USDCZK USDCZK { get; set; }
        public USDWST USDWST { get; set; }
        public USDGYD USDGYD { get; set; }
        public USDKWD USDKWD { get; set; }
        public USDTOP USDTOP { get; set; }
        public USDKRW USDKRW { get; set; }
        public USDGGP USDGGP { get; set; }
        public USDINR USDINR { get; set; }
        public USDCUP USDCUP { get; set; }
        public USDVND USDVND { get; set; }
        public COPPERHIGHGRADE COPPERHIGHGRADE { get; set; }
        public USDBBD USDBBD { get; set; }
        public USDBMD USDBMD { get; set; }
        public USDLBP USDLBP { get; set; }
        public USDSTD USDSTD { get; set; }
        public USDMKD USDMKD { get; set; }
        public USDPAB USDPAB { get; set; }
        public USDSEK USDSEK { get; set; }
        public USDMXV USDMXV { get; set; }
        public USDVUV USDVUV { get; set; }
        public USDCLF USDCLF { get; set; }
        public USDFJD USDFJD { get; set; }
        public USDSRD USDSRD { get; set; }
        public USDIQD USDIQD { get; set; }
        public USDUZS USDUZS { get; set; }
        public USDARS USDARS { get; set; }
        public USDDJF USDDJF { get; set; }
        public XAUX XAUX { get; set; }
        public USDXPF USDXPF { get; set; }
        public USDMRU USDMRU { get; set; }
        public USDCVE USDCVE { get; set; }
        public USDSZL USDSZL { get; set; }
        public USDISK USDISK { get; set; }
        public USDTJS USDTJS { get; set; }
        public USDLYD USDLYD { get; set; }
        public USDPEN USDPEN { get; set; }
        public USDKMF USDKMF { get; set; }
        public USDUSD USDUSD { get; set; }
        public USDIRR USDIRR { get; set; }
        public USDKZT USDKZT { get; set; }
        public USDBTC USDBTC { get; set; }
        public USDRSD USDRSD { get; set; }
        public USDSYP USDSYP { get; set; }
        public USDQAR USDQAR { get; set; }
        public USDBAM USDBAM { get; set; }
        public USDGHS USDGHS { get; set; }
        public USDLTL USDLTL { get; set; }
        public USDDEM USDDEM { get; set; }
        public USDJPY USDJPY { get; set; }
        public USDALL USDALL { get; set; }
        public USDLKR USDLKR { get; set; }
        public USDZWL USDZWL { get; set; }
        public USDFKP USDFKP { get; set; }
        public USDBND USDBND { get; set; }
        public USDAED USDAED { get; set; }
        public USDAWG USDAWG { get; set; }
        public USDPHP USDPHP { get; set; }
        public USDCUC USDCUC { get; set; }
        public USDFRF USDFRF { get; set; }
        public PALLADIUM1OZ PALLADIUM1OZ { get; set; }
        public USDNZD USDNZD { get; set; }
        public SILVER1OZ999NY SILVER1OZ999NY { get; set; }
        public USDGNF USDGNF { get; set; }
        public USDMAD USDMAD { get; set; }
        public USDBTN USDBTN { get; set; }
        public USDMYR USDMYR { get; set; }
        public USDRON USDRON { get; set; }
        public USDEUR USDEUR { get; set; }
        public USDCLP USDCLP { get; set; }
        public USDMUR USDMUR { get; set; }
        public USDCNY USDCNY { get; set; }
        public USDRWF USDRWF { get; set; }
        public USDZAR USDZAR { get; set; }
        public USDPLN USDPLN { get; set; }
        public USDBIF USDBIF { get; set; }
        public USDXDR USDXDR { get; set; }
        public USDTTD USDTTD { get; set; }
        public USDTZS USDTZS { get; set; }
        public USDSIT USDSIT { get; set; }
    }

    public class USDECS
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDNAD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGTQ
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCOP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCAD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDJOD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHTG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDUAH
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMMK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDETB
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAMD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDUYU
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXAF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDKK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKPW
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDSSP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXAU
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGBP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPGK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDNOK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTHB
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGMD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDRUB
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDILS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPKR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSBD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHNL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDOMR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDERN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBGN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDIMP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDEGP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDNGN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDITL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBDT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDYER
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKGS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTMT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLTC
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTWD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLAK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCDF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDJMD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAOA
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSDG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLVL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHKD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBZD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDVES
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBRX
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXPD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXCD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDZMW
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMRO
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDBYN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDNPR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHRK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSGD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTRY
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDZD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USD
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDXOF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDUGX
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKYD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKES
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDIEP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBHD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKHR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSOS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDIDR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMOP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAFN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMGA
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBYR
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDBRL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSAR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHUF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXAG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGIP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMDL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCRC
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class PLATINUM1UZ999
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDOGE
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCHF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBOB
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDNIO
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGEL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSTN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDANG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMNT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDOP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTND
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBWP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMVR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLSL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSLL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLRD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSCR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSVC
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPYG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAUD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSHP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDHUX
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDVEF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDJEP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMXN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAZN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXPT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMWK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMZN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCNH
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBSD
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDCZK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDWST
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGYD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKWD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTOP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKRW
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGGP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDINR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCUP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDVND
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class COPPERHIGHGRADE
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBBD
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDBMD
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDLBP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSTD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMKD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPAB
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDSEK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMXV
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDVUV
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCLF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDFJD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSRD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDIQD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDUZS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDARS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDJF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class XAUX
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXPF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMRU
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCVE
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSZL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDISK
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTJS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLYD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPEN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDKMF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDUSD
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDIRR
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDKZT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBTC
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDRSD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDSYP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDQAR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBAM
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGHS
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLTL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDDEM
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDJPY
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDALL
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDLKR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDZWL
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDFKP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBND
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAED
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDAWG
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPHP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCUC
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDFRF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class PALLADIUM1OZ
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDNZD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class SILVER1OZ999NY
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDGNF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMAD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBTN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMYR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDRON
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDEUR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCLP
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDMUR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDCNY
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDRWF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDZAR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDPLN
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDBIF
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDXDR
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTTD
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

    public class USDTZS
    {
        public string UTC { get; set; }
        public int Exrate { get; set; }
    }

    public class USDSIT
    {
        public string UTC { get; set; }
        public float Exrate { get; set; }
    }

}
