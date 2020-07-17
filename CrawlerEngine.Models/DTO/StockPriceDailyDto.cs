using System;

namespace CrawlerEngine.Model.DTO
{
    public class StockPriceDailyDto
    {

        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Code { get; set; }
        public float? share_price { get; set; }
        public string share_price_unit { get; set; }
        public float? five_year_surplus_compound_annual_growth_rate { get; set; }
        public string five_year_surplus_compound_annual_growth_rate_unit { get; set; }
        public float? three_year_surplus_compound_annual_growth_rate { get; set; }
        public string three_year_surplus_compound_annual_growth_rate_unit { get; set; }
        public float? annual_surplus_growth_rate_in_the_last_quarter { get; set; }
        public string annual_surplus_growth_rate_in_the_last_quarter_unit { get; set; }
        public float? recent_Four_Seasons_EPS { get; set; }
        public string Recent_Four_Seasons_EPS_unit { get; set; }
        public float? PE_ratio { get; set; }
        public string PE_ratio_unit { get; set; }
        public float? PEG_5_year_compound_growth_rate { get; set; }
        public string PEG_5_year_compound_growth_rate_unit { get; set; }
        public float? Probability_to_fill_interest_five_years { get; set; }
        public string Probability_to_fill_interest_five_years_unit { get; set; }

    }
}
