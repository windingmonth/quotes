using System;

namespace Quotes.Api.Model.Entity
{
    public class QuotesRecord
    {
        public Guid? Id { get; set; }
        public string Symbol { get; set; }           //名称
        public string LatestPrice { get; set; }      //最新价
        public string OpeningPrice { get; set; }     //开盘价
        public string HighestPrice { get; set; }     //最高价
        public string LowestPrice { get; set; }      //最低价
        public string YesterdayPrice { get; set; }   //昨收价
        public string QuoteChange { get; set; }      //涨跌幅
    }
}