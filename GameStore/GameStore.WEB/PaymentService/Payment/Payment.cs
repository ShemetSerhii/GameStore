using System.Collections.Generic;
using GameStore.Domain.Entities;
using GameStore.WEB.Services.Interfaces;

namespace GameStore.WEB.Services.Payment
{
    public class Payment
    {
        private readonly Dictionary<PaymentEnum, IPayment> _payments;

        public Payment(Order order)
        {
            _payments = new Dictionary<PaymentEnum, IPayment>
            {
                {PaymentEnum.BankPayment, new BankPayment(order) },
                {PaymentEnum.BoxPayment, new BoxPayment() },
                {PaymentEnum.VisaPayment, new VisaPayment() }
            };
        }

        public object MakePayment(PaymentEnum method)
        {
            return _payments[method].MakePayment();
        }
    }
}