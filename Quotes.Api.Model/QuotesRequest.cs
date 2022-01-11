using ServiceStack;

namespace Quotes.Api.Model
{
    [Route("/quotes_service/api/json_v2.php/Market_Center.getNameList")]
    public class QuotesSymbolRequest
    {
        public string node { get; set; }
    }
}