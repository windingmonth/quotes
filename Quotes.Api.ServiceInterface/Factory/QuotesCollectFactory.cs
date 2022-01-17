using Quotes.Api.ServiceInterface.Impl;
using Quotes.Api.ServiceInterface.Interfaces;

namespace Quotes.Api.ServiceInterface.Factory
{
    public class QuotesCollectFactory
    {
        public static IQuotesCollect CreateQuotesCollect(string label)
        {
            IQuotesCollect quotesCollect = label switch
            {
                "sina" => new SinaExchangeCollect(),
                "jintou" => new JinTouGoldSilverCollect(),
                _ => null,
            };
            return quotesCollect;
        }
    }
}