using CrawlerEngine.Model.DTO;
using CrawlerEngine.Repository.Common.Interface;
using Dapper;
using System;

namespace CrawlerEngine.Repository.Crawl
{
    public class StockPriceDailyRepository : BulkInsert<CrawlDataJobListDto>, IDisposable
    {
        private bool disposedValue = false;
        private IDatabaseConnectionHelper _DatabaseConnection;

        internal StockPriceDailyRepository(IDatabaseConnectionHelper databaseConnectionHelper)
        {
            _DatabaseConnection = databaseConnectionHelper;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    return;
                }

                disposedValue = true;
            }
        }

        ~StockPriceDailyRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public int InsertOne(StockPriceDailyDto stockPriceDailyDto)
        {
            string sqlCommand = $@"
INSERT INTO [dbo].[StockPriceDaily]
           ([Code]
           ,[share_price]
           ,[share_price_unit]
           ,[five_year_surplus_compound_annual_growth_rate]
           ,[five_year_surplus_compound_annual_growth_rate_unit]
           ,[three_year_surplus_compound_annual_growth_rate]
           ,[three_year_surplus_compound_annual_growth_rate_unit]
           ,[annual_surplus_growth_rate_in_the_last_quarter]
           ,[annual_surplus_growth_rate_in_the_last_quarter_unit]
           ,[recent_Four_Seasons_EPS]
           ,[Recent_Four_Seasons_EPS_unit]
           ,[PE_ratio]
           ,[PE_ratio_unit]
           ,[PEG_5_year_compound_growth_rate]
           ,[PEG_5_year_compound_growth_rate_unit]
           ,[Probability_to_fill_interest_five_years]
           ,[Probability_to_fill_interest_five_years_unit])
     VALUES
           (@Code
           ,@share_price
           ,@share_price_unit
           ,@five_year_surplus_compound_annual_growth_rate
           ,@five_year_surplus_compound_annual_growth_rate_unit
           ,@three_year_surplus_compound_annual_growth_rate
           ,@three_year_surplus_compound_annual_growth_rate_unit
           ,@annual_surplus_growth_rate_in_the_last_quarter
           ,@annual_surplus_growth_rate_in_the_last_quarter_unit
           ,@recent_Four_Seasons_EPS
           ,@Recent_Four_Seasons_EPS_unit
           ,@PE_ratio
           ,@PE_ratio_unit
           ,@PEG_5_year_compound_growth_rate
           ,@PEG_5_year_compound_growth_rate_unit
           ,@Probability_to_fill_interest_five_years
           ,@Probability_to_fill_interest_five_years_unit)


                                ";
            using (var conn = _DatabaseConnection.Create())
            {
                return conn.Execute(sqlCommand, stockPriceDailyDto);
            }
        }
    }
}
