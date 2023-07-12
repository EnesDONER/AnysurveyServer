using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Card = Entities.Concrete.Card;

namespace Business.ThirdPartyServices.PaymentServices.IyziPay
{
    public class IyzipayAdapter:IThirdPartyPaymentService
    {
        public IResult Pay(User user, Card card, decimal amount)
        {
            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.EN.ToString();
            request.ConversationId = "123456789";
            request.Price = amount.ToString();
            request.PaidPrice = amount.ToString();
            request.Currency = Currency.USD.ToString();
            request.Installment = 1;
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = card.HolderName;
            paymentCard.CardNumber = card.CardNumber;
            paymentCard.ExpireMonth = card.ExpireMonth;
            paymentCard.ExpireYear = card.ExpireYear;
            paymentCard.Cvc = card.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = user.Id.ToString();
            buyer.Name = user.FirstName;
            buyer.Surname = user.LastName;
            buyer.Email = user.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.City = "Istanbul";
            buyer.Country = "Turkey";
            request.Buyer = buyer;

            Address address = new Address();
            address.ContactName = $"{user.FirstName}  {user.LastName}";
            address.City = "Istanbul";
            address.Country = "Turkey";
            address.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";

            request.ShippingAddress = address;
            request.BillingAddress = address;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = "BI101";
            firstBasketItem.Name = "Basket";
            firstBasketItem.Category1 = "Ad";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = amount.ToString();
            basketItems.Add(firstBasketItem);

            request.BasketItems = basketItems;

            Options options = new Options();
            options.ApiKey = "sandbox-17gWONtSoMgaOeFSPcs0T8MZTkORWtmB";
            options.SecretKey = "sandbox-9ECYbcJwyGV5RIzqiPf00hSC8PdRoflA";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";
            Payment payment = Payment.Create(request, options);
            if (payment.ErrorMessage != null)
            {
                return new ErrorResult(payment.ErrorMessage);
            }
            return new SuccessResult();

        }
    }
}
