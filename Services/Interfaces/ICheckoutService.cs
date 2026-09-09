using EcommerceAPI.DTOs.Checkout;

namespace EcommerceAPI.Services.Interfaces
{
    public interface ICheckoutService
    {
        Task<CheckoutResponseDto> CalculateCheckoutAsync(
            int customerId,
            CheckoutRequestDto request,
            CancellationToken cancellationToken);
    }
}