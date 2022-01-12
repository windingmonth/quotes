using Newtonsoft.Json;
using Quotes.Api.Model.Config;
using Quotes.Api.Model.Entity;
using Quotes.Api.ServiceInterface.Interfaces;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quotes.Api.ServiceInterface
{
    public class SinaExchangeCollect : IQuotesCollect
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(SinaExchangeCollect));
        public QuotesCollectConfig Config { get; set; }

        public SinaExchangeCollect()
        {
            string collectConfigJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\config\\SinaExchangeCollectConfig.json");
            Config = JsonConvert.DeserializeObject<QuotesCollectConfig>(collectConfigJson);
        }

        public List<QuotesRecord> QueryLatestQuotes()
        {
            var FxUrl = Config.ServiceUrl;
            var symbols = Config.CollectSymbols.ToList();
            var map = Config.RemoteSymbolMap;

            List<QuotesRecord> res = new List<QuotesRecord>();
            var rn = Guid.NewGuid();
            logger.Info($"收集版本：{rn}, 开始收集外汇行情...");
            var respList = new List<string>();
            try
            {
                while (!symbols.IsEmpty())
                {
                    var pickSymbols = symbols.Take(100);
                    var resp = $"{FxUrl}{pickSymbols.Join(",")}".GetStringFromUrl();
                    respList.AddRange(resp.Split(";").ToList());
                    symbols.RemoveRange(0, pickSymbols.Count());

                    logger.Info($"收集版本：{rn}, 收集外汇行情成功 => length:{resp.Length}, message: {resp.Substring(0, 500)}");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"收集外汇行情异常：{ex.Message},{ex.StackTrace}");
                return res;
            }
            logger.Info($"收集版本：{rn}, 结束收集外汇行情...");
            logger.Info($"开始整理外汇行情数据...");

            respList.ForEach(resp =>
            {
                var fxArr = resp.Split(",");
                try
                {
                    res.Add(new QuotesRecord
                    {
                        Symbol = map[fxArr[0].Split("=")[0].Replace("var hq_str_", "").Trim()],         //代码
                        LatestPrice = fxArr[3],    //最新价
                        OpeningPrice = fxArr[5],   //开盘价
                        HighestPrice = fxArr[6],   //最高价
                        LowestPrice = fxArr[7],    //最低价
                        YesterdayPrice = fxArr[8], //昨收价
                        QuoteChange = fxArr[11],   //涨跌幅
                    });
                }
                catch (Exception e)
                {
                    logger.Error($"整理外汇行情数据,数据解析异常：{e.Message},{e.StackTrace}");
                }
            });

            logger.Info($"结束整理外汇行情数据...");

            return res;
        }
    }
}