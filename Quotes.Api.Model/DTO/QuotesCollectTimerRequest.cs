using ServiceStack;

namespace Quotes.Api.Model.DTO
{
    [Route("/QuotesCollectTimer")]
    public class QuotesCollectTimerRequest
    {
        public string Platforms { get; set; }
    }
}