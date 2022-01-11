using ServiceStack;

namespace Quotes.Api.Model.DTO
{
    //外汇dto
    [Route("/fx/list")]
    public class FXDto
    {
    }

    public class FXResp
    {
        public string fxCode { get; set; }
        public string fxName { get; set; }
        public string latestPrice { get; set; }
        public string amplitude { get; set; }
        public string openingPrice { get; set; }
        public string highestPrice { get; set; }
        public string lowestPrice { get; set; }
        public string yesterdayPrice { get; set; }
        public string updownAmount { get; set; }
        public string quoteChange { get; set; }
    }
}