using Quotes.Api.Model.Entity;
using Quotes.Api.Model.Types;
using Quotes.Api.ServiceInterface.Factory;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Timers;

namespace Quotes.Api.ServiceInterface
{
    public class QuotesTimer : AutoQueryServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(QuotesTimer));

        public void TimerExec(object source, ElapsedEventArgs e)
        {
            try
            {
                List<QuotesRecord> res = new List<QuotesRecord>();
                var sinaExchangeCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.SINA);
                res.AddRange(sinaExchangeCollect.QueryLatestQuotes());
                var jinTouGoldSilverCollect = QuotesCollectFactory.CreateQuotesCollect(QuotesTypes.JINTOU);
                res.AddRange(jinTouGoldSilverCollect.QueryLatestQuotes());
                logger.Info(res.ToJson());
            }
            catch (Exception ex)
            {
                logger.Error($"{ex.Message},{ex.StackTrace}");
            }
        }

        public void Timer()
        {
            logger.Info($"开始执行任务......");
            Timer t = new Timer(3 * 60 * 1000);//实例化Timer类，设置间隔时间；
            t.Elapsed += new ElapsedEventHandler((s, e) => TimerExec(s, e));
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }
    }
}