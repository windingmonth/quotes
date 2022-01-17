using Quotes.Api.Model.DTO;
using Quotes.Api.Model.Entity;
using Quotes.Api.ServiceInterface.Factory;
using ServiceStack;
using ServiceStack.Logging;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Quotes.Api.ServiceInterface
{
    public class QuotesService : AutoQueryServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(QuotesService));

        //行情列表
        public object Get(QuotesRequest request)
        {
            var q = AutoQuery.CreateQuery(request, base.Request);
            var list = AutoQuery.Execute(request, q);
            return list;
        }

        //定时收集数据
        public object Get(QuotesCollectTimerRequest request)
        {
            try
            {
                if (!request.Platforms.IsNullOrEmpty())
                {
                    var platformArr = request.Platforms.Split(",").ToList();
                    List<QuotesRecord> res = new List<QuotesRecord>();
                    platformArr.ForEach(item =>
                    {
                        try
                        {
                            var exchangeCollect = QuotesCollectFactory.CreateQuotesCollect(item);
                            res.AddRange(exchangeCollect.QueryLatestQuotes());
                            logger.Info($"平台{item}, 收集数据成功...");
                        }
                        catch (Exception e)
                        {
                            logger.Info($"平台{item}, 收集数据失败...");
                        }
                    });

                    if (!res.IsNullOrEmpty())
                    {
                        Db.DeleteAll<QuotesRecord>();
                        Db.InsertAll(res);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"{ex.Message},{ex.StackTrace}");

                return "收集异常";
            }
            return "收集成功";
        }
    }
}