using Shared.DataTransferObjects.BasketModule;

namespace ServiceAbstraction
{
    public interface IPaymentService
    {
        Task<BasketDto> CreateOrUpdatePaymentIntentAsync(string basketId);
        Task<string?> UpdateOrderStatusAsync(string paymentIntentId, bool isPaied);
    }
}
