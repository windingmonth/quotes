namespace Quotes.Api.Model
{
    public class JOResponse
    {
        public string flag { get; set; }
        public JOResponseData JO_92233 { get; set; }   //现货黄金
        public JOResponseData JO_92232 { get; set; }   //现货白银
        public JOResponseData JO_92229 { get; set; }   //现货铂金
        public JOResponseData JO_92230 { get; set; }   //现货钯金

        public JOResponseData JO_42760 { get; set; }   //工行纸黄金(人民币)
        public JOResponseData JO_42761 { get; set; }   //工行纸白银(人民币)
        public JOResponseData JO_42762 { get; set; }   //工行纸铂金(人民币)
        public JOResponseData JO_52644 { get; set; }   //工行纸钯金(人民币)
    }

    public class JOResponseData
    {
        public string code { get; set; } //代码
        public string time { get; set; } //更新时间  时间戳
        public string q1 { get; set; }   //开盘价
        public string q2 { get; set; }   //昨收价
        public string q3 { get; set; }   //最高价
        public string q4 { get; set; }   //最低价
        public string q5 { get; set; }   //买入价
        public string q6 { get; set; }   //卖出价
        public string q70 { get; set; }  //涨跌额
        public string q80 { get; set; }   //涨跌幅
        public string q63 { get; set; }   //最新价
        public string unit { get; set; }       //单位
        public string showName { get; set; }   //名称
        public string showCode { get; set; }   //代码
    }
}