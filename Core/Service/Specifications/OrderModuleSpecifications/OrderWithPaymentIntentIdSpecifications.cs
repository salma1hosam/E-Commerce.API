using DomainLayer.Models.OrderModule;

namespace Service.Specifications.OrderModuleSpecifications
{
    internal class OrderWithPaymentIntentIdSpecifications : BaseSpecifications<Order, Guid>
    {
        public OrderWithPaymentIntentIdSpecifications(string paymentIntentId) : base(order => order.PaymentIntentId == paymentIntentId)
        {

        }
    }
}
