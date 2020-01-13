
namespace GameStore.BLL.Services.Tools.CurrencyModels
{
    public class ExchangeRate
    {
        public string baseCurrency { get; set; }
        public string currency { get; set; }
        public double saleRateNB { get; set; }
        public double purchaseRateNB { get; set; }
        public double saleRate { get; set; }
        public double purchaseRate { get; set; }
    }
}
