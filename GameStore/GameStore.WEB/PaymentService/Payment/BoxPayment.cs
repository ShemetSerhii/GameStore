using GameStore.WEB.Services.Interfaces;

namespace GameStore.WEB.Services.Payment
{
    public class BoxPayment : IPayment
    {   
        public object MakePayment()
        {
            return "BoxPayment";
        }
    }
}