using System;
using System.Collections.Generic;
using System.IO;
using GameStore.Domain.Entities;
using GameStore.WEB.Services.Payment;
using Moq;
using NUnit.Framework;

namespace GameStore.WEB.Tests.PaymentService.Payment
{
    [TestFixture]
    public class BankPaymentTests
    {
        private Order _order;

        [SetUp]
        public void SetUp()
        {
            _order = new Order
            {
                CrossId = "M",
                IsDeleted = false,
                CustomerId = "Customer",
                OrderDate = DateTime.UtcNow,
                OrderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        Game = new Game
                        {
                            Name = "Test",
                            Price = 100,
                        },
                        Quantity = 2,
                    }

                },
            };
        }

        [Test]
        public void MakePayment_WhenOrderDetailNotNull_ReturnTypeMemoryStream()
        {
            BankPayment payment = new BankPayment(_order);

            var result = payment.MakePayment() as MemoryStream;

            Assert.AreEqual(result.GetType(), typeof(MemoryStream));
        }
    }
}
