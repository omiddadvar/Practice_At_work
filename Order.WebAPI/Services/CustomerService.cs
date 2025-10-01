using AutoMapper;
using Order.WebAPI.Abstractions.Repositories;
using Order.WebAPI.Abstractions.Services;
using Order.WebAPI.Models.DTOs;
using Order.WebAPI.Models.Entities;

namespace Order.WebAPI.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository, IOrderRepository orderRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync()
    {
        var customers = await _customerRepository.GetCustomersWithOrdersAsync();
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

        // Set total orders for each customer
        foreach (var customerDto in customerDtos)
        {
            var customer = customers.First(c => c.Id == customerDto.Id);
            customerDto.TotalOrders = customer.Orders.Count;
        }

        return customerDtos;
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _customerRepository.GetCustomerWithOrdersAsync(id);
        if (customer == null) return null;

        var customerDto = _mapper.Map<CustomerDto>(customer);
        customerDto.TotalOrders = customer.Orders.Count;
        return customerDto;
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto createCustomerDto)
    {
        if (await _customerRepository.CustomerExistsAsync(createCustomerDto.Email))
        {
            throw new ArgumentException($"Customer with email '{createCustomerDto.Email}' already exists.");
        }

        var customer = _mapper.Map<Customer>(createCustomerDto);
        var createdCustomer = await _customerRepository.CreateAsync(customer);
        return _mapper.Map<CustomerDto>(createdCustomer);
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto updateCustomerDto)
    {
        var customer = await _customerRepository.FindByIdAsync(id);
        if (customer == null) return null;

        _mapper.Map(updateCustomerDto, customer);
        var updatedCustomer = await _customerRepository.UpdateAsync(customer);
        return _mapper.Map<CustomerDto>(updatedCustomer);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.FindByIdAsync(id);
        if (customer == null) return false;

        await _customerRepository.DeleteAsync(customer);
        return true;
    }

    public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId)
    {
        var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
        return _mapper.Map<IEnumerable<OrderDto>>(orders);
    }
}