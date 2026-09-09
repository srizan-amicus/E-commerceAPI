namespace EcommerceAPI.DTOs.Checkout
{
    public class CheckoutRequestDto
    {
        public string ShippingName { get; set; } = string.Empty;

        public string ShippingAddress { get; set; } = string.Empty;

        public string ShippingCity { get; set; } = string.Empty;

        public string ShippingState { get; set; } = string.Empty;

        public string ShippingPostalCode { get; set; } = string.Empty;

        public string ShippingMethod { get; set; } = string.Empty;

        public string? PaymentMethod { get; set; }
    }
}