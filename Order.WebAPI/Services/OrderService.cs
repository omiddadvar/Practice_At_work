using AutoMapper;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly IInventoryGrpcClient _inventoryGrpcClient;

    public OrderService(IOrderRepository orderRepository, ICustomerRepository customerRepository,
                      IProductRepository productRepository, IMapper mapper, IInventoryGrpcClient inventoryGrpcClient)
    {
        _orderRepository = orderRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _mapper = mapper;
        _inventoryGrpcClient = inventoryGrpcClient;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersAsync()
    {
        var orders = await _orderRepository.GetOrdersWithDetailsAsync();
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderByIdAsync(int id)
    {
        var order = await _orderRepository.GetOrderWithDetailsAsync(id);
        return order == null ? null : _mapper.Map<OrderDto>(order);
    }

    public async Task<OrderDto?> GetOrderByNumberAsync(string orderNumber)
    {
        var order = await _orderRepository.GetByOrderNumberAsync(orderNumber);
        return order == null ? null : _mapper.Map<OrderDto>(order);
    }
    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        // Validate customer exists
        if (!await _customerRepository.ExistsAsync(createOrderDto.CustomerId))
        {
            throw new ArgumentException($"Customer with ID '{createOrderDto.CustomerId}' does not exist.");
        }

        // Validate order items
        if (createOrderDto.OrderItems == null || !createOrderDto.OrderItems.Any())
        {
            throw new ArgumentException("Order must have at least one item.");
        }

        // Validate products via gRPC and enrich order items with product data
        foreach (var itemDto in createOrderDto.OrderItems)
        {
            // Get product details from Inventory service
            var product = await _inventoryGrpcClient.GetProductAsync(itemDto.ProductId, itemDto.ProductSku);
            if (product == null)
            {
                throw new ArgumentException($"Product with ID '{itemDto.ProductId}' and SKU '{itemDto.ProductSku}' not found in inventory.");
            }

            // Validate that we have consistent data
            if (itemDto.ProductId != product.Id)
            {
                throw new ArgumentException($"Product ID mismatch for SKU '{itemDto.ProductSku}'.");
            }

            // Use product name from inventory if not provided
            if (string.IsNullOrEmpty(itemDto.ProductName))
            {
                itemDto.ProductName = product.Name;
            }

            // Use inventory price if not provided or validate price
            if (itemDto.UnitPrice <= 0)
            {
                itemDto.UnitPrice = product.Price;
            }
            else if (itemDto.UnitPrice != product.Price)
            {
                // You might want to log this or handle price differences
                //_logger.LogWarning("Order item price {OrderPrice} differs from inventory price {InventoryPrice} for product {ProductId}",
                //    itemDto.UnitPrice, product.Price, itemDto.ProductId);
            }
        }

        // Generate unique order number
        var orderNumber = GenerateOrderNumber();

        // Create order
        var order = new Models.Entities.Order
        {
            OrderNumber = orderNumber,
            CustomerId = createOrderDto.CustomerId,
            ShippingAddress = createOrderDto.ShippingAddress,
            BillingAddress = createOrderDto.BillingAddress,
            Notes = createOrderDto.Notes,
            Status = "Pending",
            OrderDate = DateTime.UtcNow
        };

        // Add order items and calculate total
        decimal totalAmount = 0;
        foreach (var itemDto in createOrderDto.OrderItems)
        {
            var orderItem = new OrderItem
            {
                ProductId = itemDto.ProductId, // Store ProductId
                ProductName = itemDto.ProductName,
                ProductSku = itemDto.ProductSku,
                UnitPrice = itemDto.UnitPrice,
                Quantity = itemDto.Quantity
            };
            order.OrderItems.Add(orderItem);
            totalAmount += orderItem.TotalPrice;
        }

        order.TotalAmount = totalAmount;

        var createdOrder = await _orderRepository.CreateAsync(order);

        // Reload with details
        var orderWithDetails = await _orderRepository.GetOrderWithDetailsAsync(createdOrder.Id);
        return _mapper.Map<OrderDto>(orderWithDetails!);
    }

    public async Task<OrderDto?> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return null;

        _mapper.Map(updateOrderDto, order);
        var updatedOrder = await _orderRepository.UpdateAsync(order);

        // Reload with details
        var orderWithDetails = await _orderRepository.GetOrderWithDetailsAsync(updatedOrder.Id);
        return _mapper.Map<OrderDto>(orderWithDetails!);
    }

    public async Task<OrderDto?> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateOrderStatusDto)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return null;

        order.Status = updateOrderStatusDto.Status;
        var updatedOrder = await _orderRepository.UpdateAsync(order);

        // Reload with details
        var orderWithDetails = await _orderRepository.GetOrderWithDetailsAsync(updatedOrder.Id);
        return _mapper.Map<OrderDto>(orderWithDetails!);
    }

    public async Task<bool> DeleteOrderAsync(int id)
    {
        var order = await _orderRepository.FindByIdAsync(id);
        if (order == null) return false;

        await _orderRepository.DeleteAsync(order);
        return true;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByStatusAsync(string status)
    {
        var orders = await _orderRepository.GetOrdersByStatusAsync(status);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var orders = await _orderRepository.GetOrdersByDateRangeAsync(startDate, endDate);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    private string GenerateOrderNumber()
    {
        return $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
    }
}