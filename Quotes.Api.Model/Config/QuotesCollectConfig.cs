using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quotes.Api.Model.Config
{
    public class QuotesCollectConfig
    {
        public string ServiceUrl { get; set;}
        public string[] CollectSymbols { get; set;}
        public Dictionary<string, string> RemoteSymbolMap { get; set;}
    }
}
