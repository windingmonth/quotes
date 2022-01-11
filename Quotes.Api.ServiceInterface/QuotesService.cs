using Quotes.Api.Model;
using Quotes.Api.Model.DTO;
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
        private string Domain = "http://vip.stock.finance.sina.com.cn";

        private List<QuotesSymbolResponseData> getQuotesSymbolList()
        {
            //获取所有的外汇symbol
            //http://vip.stock.finance.sina.com.cn/quotes_service/api/json_v2.php/Market_Center.getNameList?node=cny_forex

            JsonServiceClient client = new JsonServiceClient(Domain);
            var response = client.Get<List<QuotesSymbolResponseData>>(new QuotesSymbolRequest
            {
                node = "cny_forex"
            });

            logger.Info("获取symbol......");

            return response;
        }

        //外汇行情
        public object Get(FXDto dto)
        {
            var symbols = getQuotesSymbolList();

            var url = "https://hq.sinajs.cn/list=";

            var respList = new List<string>();
            while (!symbols.IsEmpty())
            {
                var pickSymbols = symbols.Take(100);
                var resp = $"{url}{pickSymbols.Map(x => x.symbol).Join(",")}".GetStringFromUrl(
                    responseFilter: (response) =>
                        {
                            //response.SET = new MemoryStream(Encoding.Convert(Encoding.ASCII, Encoding.UTF8, response.GetResponseStream().ToBytes()));
                        }
                    );
                respList.AddRange(resp.Split(";").ToList());
                symbols.RemoveRange(0, pickSymbols.Count());
            }

            List<FXResp> res = new List<FXResp>();
            respList.ForEach(resp =>
            {
                var fxArr = resp.Split(",");
                try
                {
                    res.Add(new FXResp
                    {
                        fxCode = fxArr[0].Split("=")[0].Replace("var hq_str_", "").Trim(),         //外汇代码
                        latestPrice = fxArr[3],  //最新价
                        amplitude = fxArr[4],  //振幅
                        openingPrice = fxArr[5],  //开盘价
                        highestPrice = fxArr[6],  //最高价
                        lowestPrice = fxArr[7],  //最低价
                        yesterdayPrice = fxArr[8],  //昨收价
                        fxName = fxArr[9],  //外汇名称
                        updownAmount = fxArr[10],  //涨跌额
                        quoteChange = fxArr[11]  //涨跌幅
                    });
                }
                catch (Exception e)
                {
                    logger.Error($"{e.Message},{e.StackTrace}");
                }
            });

            return res;
        }

        //金银行情
    }
}