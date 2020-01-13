using GameStore.WEB.Services.Payment;
using NUnit.Framework;

namespace GameStore.WEB.Tests.PaymentService.Payment
{
    [TestFixture]
    public class VisaPaymentTests
    {
        [Test]
        public void MakePayment_Always_ReturnNameView()
        {
            VisaPayment payment = new VisaPayment();

            var result = payment.MakePayment() as string;

            Assert.AreEqual(result, "VisaPayment");
        }
    }
}
