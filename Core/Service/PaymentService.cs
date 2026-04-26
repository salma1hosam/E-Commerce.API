using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models.OrderModule;
using Microsoft.Extensions.Configuration;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModule;
using Stripe;
using Product = DomainLayer.Models.ProductModule.Product;

namespace Service
{
    public class PaymentService(IConfiguration _configuration, IBasketRepository _basketRepository,
                                IUnitOfWork _unitOfWork, IMapper _mapper) : IPaymentService
    {
        public async Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId)
        {
            // Configure Stripe : Install Package Stripe.net
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            // Get Basket by basketId
            var basket = await _basketRepository.GetBasketAsync(basketId) ?? throw new BasketNotFoundException(basketId);

            // Calculate and Get Amount - Get Product + Delivery Method Cost
            var productRepo = _unitOfWork.GetRepository<Product, int>();
            foreach (var item in basket.Items)
            {
                var product = await productRepo.GetByIdAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);
                item.Price = product.Price;
            }

            ArgumentNullException.ThrowIfNull(basket.DeliveryMethodId);
            var deliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(basket.DeliveryMethodId.Value)
                ?? throw new DeliveryMethodNotFoundException(basket.DeliveryMethodId.Value);

            basket.ShippingPrice = deliveryMethod.Price;

            var basketAmount = (long)(basket.Items.Sum(x => x.Quantity * x.Price) + deliveryMethod.Price) * 100;

            // Create Payment Intent [Create - Update]
            var paymentService = new PaymentIntentService();

            if (basket.PaymentIntentId is null)  // Create
            {
                var options = new PaymentIntentCreateOptions
                {
                    Amount = basketAmount,
                    Currency = "USD",
                    PaymentMethodTypes = ["card"]
                };

                var paymentIntent = await paymentService.CreateAsync(options);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions
                {
                    Amount = basketAmount
                };
                await paymentService.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepository.CreateOrUpdateBasketAsync(basket);

            return _mapper.Map<BasketDto>(basket);
        }
    }
}
