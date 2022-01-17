using Quotes.Api.Model.Entity;
using ServiceStack;

namespace Quotes.Api.Model.DTO
{
    [Route("/Quotes/List")]
    public class QuotesRequest : QueryDb<QuotesRecord>
    {
        public string Symbol { get; set; }
    }

    public class QuotesResp
    {
        public string Symbol { get; set; }           //名称
        public string LatestPrice { get; set; }      //最新价
        public string OpeningPrice { get; set; }     //开盘价
        public string HighestPrice { get; set; }     //最高价
        public string LowestPrice { get; set; }      //最低价
        public string YesterdayPrice { get; set; }   //昨收价
        public string QuoteChange { get; set; }      //涨跌幅
    }
}