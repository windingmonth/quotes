using Newtonsoft.Json;
using Quotes.Api.Model;
using Quotes.Api.Model.Config;
using Quotes.Api.Model.Entity;
using Quotes.Api.ServiceInterface.Interfaces;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Quotes.Api.ServiceInterface.Impl
{
    public class JinTouGoldSilverCollect : IQuotesCollect
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(JinTouGoldSilverCollect));
        public QuotesCollectConfig Config { get; set; }

        public JinTouGoldSilverCollect()
        {
            string collectConfigJson = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "\\config\\JinTouGoldSilverCollectConfig.json");
            Config = JsonConvert.DeserializeObject<QuotesCollectConfig>(collectConfigJson);
        }

        public List<QuotesRecord> QueryLatestQuotes()
        {
            var JoUrl = Config.ServiceUrl;
            var symbols = Config.CollectSymbols.ToList();
            var map = Config.RemoteSymbolMap;

            List<QuotesRecord> res = new List<QuotesRecord>();
            try
            {
                var JoResp = $"{JoUrl}{symbols.Join(",")}".GetStringFromUrl();

                var JoArr = JoResp.Split("=")[1].Trim();
                var RowJos = JsonConvert.DeserializeObject<JOResponse>(JoArr);
                logger.Info($"拉取金银行情数据成功 => length:{JoResp.Length}, message: {JoResp.Substring(0, 500)}");

                List<JOResponseData> Jos = new List<JOResponseData> {
                    RowJos.JO_92233,
                    RowJos.JO_92232,
                    RowJos.JO_92229,
                    RowJos.JO_92230,
                    RowJos.JO_42760,
                    RowJos.JO_42761,
                    RowJos.JO_42762,
                    RowJos.JO_52644
                };

                logger.Info($"开始整理金银行情数据...");

                Jos.ForEach(Jo =>
                {
                    try
                    {
                        res.Add(new QuotesRecord
                        {
                            Symbol = map[Jo.showCode],  //代码
                            LatestPrice = Jo.q63,       //最新价
                            OpeningPrice = Jo.q1,       //开盘价
                            HighestPrice = Jo.q3,       //最高价
                            LowestPrice = Jo.q4,        //最低价
                            YesterdayPrice = Jo.q2,     //昨收价
                            QuoteChange = Jo.q80,       //涨跌幅
                            CreatedOn = DateTime.Now,
                            UpdatedOn = DateTime.Now
                        });
                    }
                    catch (Exception e)
                    {
                        logger.Error($"整理金银行情数据,数据解析异常：{e.Message},{e.StackTrace}");
                    }
                });

                logger.Info($"结束整理金银行情数据...");
            }
            catch (Exception e)
            {
                logger.Error($"拉取金银行情数据异常：{e.Message},{e.StackTrace}");
            }

            return res;
        }
    }
}