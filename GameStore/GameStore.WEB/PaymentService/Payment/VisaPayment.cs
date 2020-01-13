using GameStore.WEB.Services.Interfaces;

namespace GameStore.WEB.Services.Payment
{
    public class VisaPayment : IPayment
    {
        public object MakePayment()
        {
            return "VisaPayment";
        }
    }
}