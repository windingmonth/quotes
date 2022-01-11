using Quotes.Api.Model;
using Quotes.Api.Model.DTO;
using ServiceStack;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace Quotes.Api.ServiceInterface
{
    public class PullDataDemo : AutoQueryServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PullDataDemo));

        public static List<QuotesSymbolResponseData> symbols()
        {
            JsonServiceClient client = new JsonServiceClient("http://vip.stock.finance.sina.com.cn");

            try
            {
                var response = client.Get<List<QuotesSymbolResponseData>>(new QuotesSymbolRequest
                {
                    node = "cny_forex"
                });

                logger.Info($"拉取symbol列表成功");
                return response;
            }
            catch (Exception e)
            {
                logger.Error($"拉取symbol列表失败, {e.Message},{e.StackTrace}");
            }
            return null;
        }

        public static void fxPull()
        {
            var symbols = PullDataDemo.symbols();

            var url = "https://hq.sinajs.cn/list=";

            var respList = new List<string>();
            try
            {
                var rn = Guid.NewGuid();
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

                    logger.Info($"外汇行情拉取版本：{rn}, 拉取外汇行情数据成功 => length:{resp.Length}, message: {resp.Substring(0, 500)}");
                }
            }
            catch (Exception ex)
            {
                logger.Error($"拉取外汇行情数据异常：{ex.Message},{ex.StackTrace}");
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
                    logger.Error($"数据解析异常：{e.Message},{e.StackTrace}");
                }
            });
        }

        public static void JOPull()
        {
            var url = "https://api.jijinhao.com/quoteCenter/realTime.htm?codes=JO_92233,JO_92232,JO_92229,JO_92230,JO_92231,JO_38495";
            try
            {
                var jsonstr = url.GetStringFromUrl();

                logger.Info($"拉取金银行情数据成功 => length:{jsonstr.Length}, message: {jsonstr.Substring(0, 500)}");
            }
            catch (Exception e)
            {
                logger.Error($"拉取金银行情数据异常：{e.Message},{e.StackTrace}");
            }
        }

        public static void exec(object source, ElapsedEventArgs e)
        {
            fxPull();
            JOPull();
        }

        public static void timer()
        {
            logger.Info($"开始执行任务......");
            //Timer t = new Timer(5000);//实例化Timer类，设置间隔时间；
            Timer t = new Timer(3 * 60 * 1000);//实例化Timer类，设置间隔时间；
            t.Elapsed += new ElapsedEventHandler((s, e) => exec(s, e));
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
        }
    }
}