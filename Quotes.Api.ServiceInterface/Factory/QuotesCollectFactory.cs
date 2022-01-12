using Quotes.Api.ServiceInterface.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
