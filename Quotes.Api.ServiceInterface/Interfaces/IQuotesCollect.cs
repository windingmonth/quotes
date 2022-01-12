using Quotes.Api.Model.Config;
using Quotes.Api.Model.Entity;
using System.Collections.Generic;

namespace Quotes.Api.ServiceInterface.Interfaces
{
    public interface IQuotesCollect
    {
        QuotesCollectConfig Config { get; set; }
        abstract List<QuotesRecord> QueryLatestQuotes();
    }
}