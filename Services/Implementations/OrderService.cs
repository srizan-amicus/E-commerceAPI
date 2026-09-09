using EcommerceAPI.DTO;
using EcommerceAPI.DTOs.Order;
using EcommerceAPI.Models.Order;
using EcommerceAPI.Repositories.Interfaces;
using EcommerceAPI.Services.Interfaces;

namespace EcommerceAPI.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartService _cartService;

        public OrderService(
            IOrderRepository orderRepository,
            ICartService cartService)
        {
            _orderRepository = orderRepository;
            _cartService = cartService;
        }
        public async Task<int> CreateOrderAsync(
            Order order,
            CancellationToken cancellationToken)
        {
            return await _orderRepository.CreateOrderAsync(
                order,
                cancellationToken);
        }

        public async Task<int> CreateOrderItemAsync(
            OrderItem orderItem,
            CancellationToken cancellationToken)
        {
            return await _orderRepository.CreateOrderItemAsync(
                orderItem,
                cancellationToken);
        }

        public async Task<int> CreateOrderShippingAsync(
            OrderShipping orderShipping,
            CancellationToken cancellationToken)
        {
            return await _orderRepository.CreateOrderShippingAsync(
                orderShipping,
                cancellationToken);
        }

        public async Task<int> CreateCompleteOrderAsync(
            Order order,
            List<OrderItem> orderItems,
            OrderShipping shipping,
            CancellationToken cancellationToken)
        {
            var orderId =
                await _orderRepository.CreateOrderAsync(
                    order,
                    cancellationToken);

            foreach (var item in orderItems)
            {
                item.OrderId = orderId;

                await _orderRepository.CreateOrderItemAsync(
                    item,
                    cancellationToken);
            }

            shipping.OrderId = orderId;

            await _orderRepository.CreateOrderShippingAsync(
                shipping,
                cancellationToken);

            return orderId;
        }

        public async Task UpdateOrderStatusAsync(
            int orderId,
            string orderStatus,
            int updatedBy,
            CancellationToken cancellationToken)
        {
            await _orderRepository.UpdateOrderStatusAsync(
                orderId,
                orderStatus,
                updatedBy,
                cancellationToken);
        }
        public async Task<Order?> GetByIdAsync(
    int orderId,
    CancellationToken cancellationToken)
        {
            return await _orderRepository.GetByIdAsync(
                orderId,
                cancellationToken);
        }

        public async Task<(List<Order> Orders, int TotalRecords)> GetOrderHistoryAsync(
    int customerId,
    string? status,
    int page,
    int pageSize,
    CancellationToken cancellationToken)
        {
            return await _orderRepository.GetOrderHistoryAsync(
                customerId,
                status,
                page,
                pageSize,
                cancellationToken);
        }

        public async Task ReorderAsync(
    int customerId,
    int orderId,
    CancellationToken cancellationToken)
        {
            var order =
                await _orderRepository.GetByIdAsync(
                    orderId,
                    cancellationToken);

            if (order == null)
            {
                throw new InvalidOperationException(
                    "Order not found.");
            }

            if (order.CustomerId != customerId)
            {
                throw new UnauthorizedAccessException(
                    "You are not authorized to reorder this order.");
            }

            var items =
                await _orderRepository.GetOrderItemsAsync(
                    orderId,
                    cancellationToken);

            foreach (var item in items)
            {
                await _cartService.AddItemAsync(
                    customerId,
                    new DTO.AddCartItemDto
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    }, cancellationToken);
            }
        }
        public async Task<OrderDetailsDto?> GetOrderDetailsAsync(
    int orderId,
    CancellationToken cancellationToken)
        {
            var order =
                await _orderRepository.GetByIdAsync(
                    orderId,
                    cancellationToken);

            if (order == null)
            {
                return null;
            }

            var items =
                await _orderRepository.GetOrderItemsAsync(
                    orderId,
                    cancellationToken);

            var shipping =
                await _orderRepository.GetOrderShippingAsync(
                    orderId,
                    cancellationToken);

            var payment =
                await _orderRepository.GetOrderPaymentAsync(
                    orderId,
                    cancellationToken);

            return new OrderDetailsDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                Subtotal = order.Subtotal,
                ShippingCharge = order.ShippingCharge,
                Tax = order.Tax,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                CreatedAt = order.CreatedAt,

                Items = items.Select(item => new OrderItemDto
                {
                    OrderItemId = item.OrderItemId,
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    ItemSubtotal = item.ItemSubtotal
                }).ToList(),

                Shipping = shipping == null
                    ? null
                    : new OrderShippingDto
                    {
                        ShippingName = shipping.ShippingName,
                        ShippingAddress = shipping.ShippingAddress,
                        ShippingCity = shipping.ShippingCity,
                        ShippingState = shipping.ShippingState,
                        ShippingPostalCode = shipping.ShippingPostalCode,
                        ShippingMethod = shipping.ShippingMethod,
                        ShippingCharge = shipping.ShippingCharge
                    },

                Payment = payment == null
                    ? null
                    : new EcommerceAPI.DTOs.Payment.OrderPaymentDto
                    {
                        PaymentId = payment.PaymentId,
                        OrderId = payment.OrderId,
                        PaymentStatus = payment.PaymentStatus,
                        PaymentMethod = payment.PaymentMethod,
                        Amount = payment.Amount,
                        TransactionReference = payment.TransactionReference,
                        CreatedAt = payment.CreatedAt
                    }
            };
        }
    }
}