using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Business.Concrete;
namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Pay()
        {
            int userId = 123; // Kullanıcı kimliği
            int cardId = 456; // Kredi kartı kimliği
            decimal amount = 100.0m; // Ödeme miktarı

            // Ödeme yöneticisini oluşturun
            IPaymentService paymentManager = new PaymentManager(userService, cardService, paypalAdapter);

            // Ödeme işlemi gerçekleştirin
            var paymentResult = paymentManager.Pay(userId, cardId, amount);

            if (paymentResult.Success)
            {
                Console.WriteLine("Ödeme başarılı!");
            }
            else
            {
                Console.WriteLine("Ödeme başarısız: " + paymentResult.Message);
            }

        }
    }
}
