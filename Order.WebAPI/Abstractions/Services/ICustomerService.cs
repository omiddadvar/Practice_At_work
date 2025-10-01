using Order.WebAPI.Models.DTOs;

namespace Order.WebAPI.Abstractions.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetCustomersAsync();
    Task<CustomerDto?> GetCustomerByIdAsync(int id);
    Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto);
    Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto);
    Task<bool> DeleteCustomerAsync(int id);
    Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId);
}