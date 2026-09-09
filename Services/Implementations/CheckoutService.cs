using EcommerceAPI.DTO;
using EcommerceAPI.DTOs.Checkout;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Interfaces;

namespace EcommerceAPI.Services.Implementations
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IProductPriceService _productPriceService;
        private readonly IProductAvailabilityService _productAvailabilityService;
        private readonly IOrderService _orderService;

        public CheckoutService(
            ICheckoutRepository checkoutRepository,
            IProductPriceService productPriceService,
            IProductAvailabilityService productAvailabilityService,
            IOrderService orderService)
        {
            _checkoutRepository = checkoutRepository;
            _productPriceService = productPriceService;
            _productAvailabilityService = productAvailabilityService;
            _orderService = orderService;
        }

        public async Task<CheckoutResponseDto> CalculateCheckoutAsync(
            int customerId,
            CheckoutRequestDto request,
            CancellationToken cancellationToken)
        {
            // Get cart
            var cartItems =
                await _checkoutRepository.GetCartItemsAsync(
                    customerId,
                    cancellationToken);

            var priceChanges = new List<PriceChangeDto>();
            var stockIssues = new List<StockIssueDto>();

            // Check stock
            var inventoryItems = cartItems
                .Select(item => new InventoryCheckItemDto
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                })
                .ToList();

            var inventoryResults =
                await _productAvailabilityService.CheckInventoryAsync(
                    inventoryItems,
                    cancellationToken);

            foreach (var result in inventoryResults)
            {
                if (!result.IsAvailable)
                {
                    var cartItem = cartItems.First(
                        item => item.ProductId == result.ProductId);

                    stockIssues.Add(new StockIssueDto
                    {
                        ProductId = result.ProductId,
                        ProductName = cartItem.ProductName,
                        RequestedQuantity = result.RequestedQuantity,
                        AvailableQuantity = result.AvailableQuantity
                    });
                }
            }

            // Check price changes
            foreach (var item in cartItems)
            {
                var currentPrice =
                    await _productPriceService
                        .GetCurrentPriceByProductIdAsync(
                            item.ProductId);

                if (currentPrice != null &&
                    currentPrice.Price != item.Price)
                {
                    priceChanges.Add(new PriceChangeDto
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        OldPrice = item.Price,
                        CurrentPrice = currentPrice.Price
                    });
                }
            }

            // Stop checkout if stock or price has an issue
            if (priceChanges.Any() || stockIssues.Any())
            {
                return new CheckoutResponseDto
                {
                    PriceChanges = priceChanges,
                    StockIssues = stockIssues
                };
            }

            // Calculate totals
            var subtotal = cartItems.Sum(
                item => item.ItemSubtotal);

            decimal shippingCharge = request.ShippingMethod
                .ToLower() switch
            {
                "standard" => 100,
                "express" => 250,
                "same day" => 400,
                _ => 0
            };

            var tax = subtotal * 0.18m;

            var totalAmount =
                subtotal +
                shippingCharge +
                tax;

            // Create Order
            var order = new EcommerceAPI.Models.Order.Order
            {
                CustomerId = customerId,
                Subtotal = subtotal,
                ShippingCharge = shippingCharge,
                Tax = tax,
                TotalAmount = totalAmount,
                OrderStatus = "Pending",
                CreatedBy = customerId
            };

            // Create Order Items
            var orderItems = cartItems
                .Select(item =>
                    new EcommerceAPI.Models.Order.OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        ItemSubtotal = item.ItemSubtotal,
                        CreatedBy = customerId
                    })
                .ToList();

            // Create Shipping
            var shipping =
                new EcommerceAPI.Models.Order.OrderShipping
                {
                    ShippingName = request.ShippingName,
                    ShippingAddress = request.ShippingAddress,
                    ShippingCity = request.ShippingCity,
                    ShippingState = request.ShippingState,
                    ShippingPostalCode = request.ShippingPostalCode,
                    ShippingMethod = request.ShippingMethod,
                    ShippingCharge = shippingCharge,
                    CreatedBy = customerId
                };

            // Create complete order
            var orderId =
                await _orderService.CreateCompleteOrderAsync(
                    order,
                    orderItems,
                    shipping,
                    cancellationToken);

            return new CheckoutResponseDto
            {
                OrderId = orderId,
                Subtotal = subtotal,
                ShippingCharge = shippingCharge,
                Tax = tax,
                TotalAmount = totalAmount,
                PriceChanges = priceChanges,
                StockIssues = stockIssues
            };
        }
    }
}