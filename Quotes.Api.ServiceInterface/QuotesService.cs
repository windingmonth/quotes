using Quotes.Api.Model;
using Quotes.Api.Model.DTO;
using Quotes.Api.Model.Entity;
using Quotes.Api.Model.Types;
using Quotes.Api.ServiceInterface.Factory;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quotes.Api.ServiceInterface
{
    public class QuotesService : AutoQueryServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(QuotesService));

        //行情列表
        public object Get(QuotesDto request)
        {
            List<QuotesRecord> res = new List<QuotesRecord>();
            var sinaExchangeCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.SINA);
            res.AddRange(sinaExchangeCollect.QueryLatestQuotes());
            var jinTouGoldSilverCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.JINTOU);
            res.AddRange(jinTouGoldSilverCollect.QueryLatestQuotes());
            return res.ToJson();
        }

        public object Post(QuotesDto request)
        {
            List<QuotesRecord> res = new List<QuotesRecord>();
            var sinaExchangeCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.SINA);
            res.AddRange(sinaExchangeCollect.QueryLatestQuotes());
            var jinTouGoldSilverCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.JINTOU);
            res.AddRange(jinTouGoldSilverCollect.QueryLatestQuotes());
            return res;
        }
    }
}