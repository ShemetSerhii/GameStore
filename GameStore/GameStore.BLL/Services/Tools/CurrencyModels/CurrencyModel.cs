
using System.Collections.Generic;

namespace GameStore.BLL.Services.Tools.CurrencyModels
{
    public class CurrencyModel
    {
        public string date { get; set; }
        public string bank { get; set; }
        public decimal baseCurrency { get; set; }
        public string baseCurrencyLit { get; set; }
        public ICollection<ExchangeRate> exchangeRate { get; set; }
    }
}
