using GameStore.WEB.Services.Payment;
using NUnit.Framework;

namespace GameStore.WEB.Tests.PaymentService.Payment
{
    [TestFixture]
    public class BoxPaymentTests
    {
        [Test]
        public void MakePayment_Always_ReturnNameView()
        {
            BoxPayment payment = new BoxPayment();

            var result = payment.MakePayment() as string;

            Assert.AreEqual(result, "BoxPayment");
        }
    }
}
