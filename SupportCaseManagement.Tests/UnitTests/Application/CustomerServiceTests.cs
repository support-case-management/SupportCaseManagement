using Castle.Core.Resource;
using NSubstitute;
using SupportCaseManagement.Application.Dtos.Requests;
using SupportCaseManagement.Application.Dtos.Responses;
using SupportCaseManagement.Application.Features.Customers;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Application;

public class CustomerServiceTests
{
    [Fact]
    public async Task CreateCustomerAsync_Should_GenerateAndSaveCustomer_ReturnCreateCustomerResponse()
    {
        // Arrange
        ICustomerRepository repository = Substitute.For<ICustomerRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(new List<Customer>()));

        ICustomerService customerService = new CustomerService(repository);

        CreateCustomerRequest request = new("Mia", "mia@mail.com", false);

        // Act
        CreateCustomerResponse response = await customerService.CreateCustomerAsync(request);

        // Assert
        Assert.NotNull(response);
        Assert.True(response.Success);
        Assert.NotNull(response.Customer);

        Customer createdCustomer = response.Customer!;

        Assert.NotEqual(createdCustomer.CustomerId, Guid.Empty);
        Assert.Equal(createdCustomer.CustomerName, request.CustomerName);
        Assert.Equal(createdCustomer.EmailAddress, request.EmailAddress.ToLower());
        Assert.Null(response.ErrorMessage);

        await repository.Received(1).SaveAllAsync(
            Arg.Is<IEnumerable<Customer>>(customers =>
            customers.Count() == 1 &&
            customers.Any(customer => customer.CustomerId == createdCustomer.CustomerId)));
    }

    [Fact]
    public async Task CreateCustomerAsync_Should_PreserveExistingCustomersWhenSavingNewCustomer()
    {
        // Arrange
        Customer existingCustomer = new(
            Guid.NewGuid(),
            "Stella",
            "stella@mail.com",
            false);

        ICustomerRepository repository = Substitute.For<ICustomerRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(new List<Customer> { existingCustomer }));

        ICustomerService customerService = new CustomerService(repository);

        repository.GetAllAsync()
            .Returns(Task.FromResult(
                new List<Customer> { existingCustomer }));

        // Act
        CreateCustomerResponse response =
            await customerService.CreateCustomerAsync(
                new CreateCustomerRequest("Mia", "mia@mail.com", false));

        // Assert
        Assert.True(response.Success);

        await repository.Received(1).SaveAllAsync(
            Arg.Is<IEnumerable<Customer>>(customers =>
                customers.Count() == 2 &&
                customers.Any(c => c.CustomerId == existingCustomer.CustomerId) &&
                customers.Any(c => c.CustomerId == response.Customer!.CustomerId)));
    }

    [Fact]
    public async Task CreateCustomerAsync_Should_ReturnFailureWhenRepositoryThrows()
    {
        // Arrange
        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetAllAsync()
            .Returns(Task.FromException<List<Customer>>(
                new Exception("Kunde inte hämta kunder")));

        ICustomerService customerService =
            new CustomerService(repository);

        CreateCustomerRequest request =
            new("Mia", "mia@mail.com", false);

        // Act
        CreateCustomerResponse response =
            await customerService.CreateCustomerAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal(
            "Kunde inte hämta kunder",
            response.ErrorMessage);

        await repository.DidNotReceive()
            .SaveAllAsync(Arg.Any<IEnumerable<Customer>>());
    }

    //GetAll
    [Fact]
    public async Task GetAllCustomersAsync_Should_ReturnCustomersSortedByName()
    {
        // Arrange
        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        List<Customer> customers =
        [
            new(Guid.NewGuid(), "Mia", "mia@mail.com", false),
            new(Guid.NewGuid(), "Anna", "anna@mail.com", false),
            new(Guid.NewGuid(), "Erik", "erik@mail.com", true)
        ];

        repository.GetAllAsync()
            .Returns(Task.FromResult(customers));

        ICustomerService customerService =
            new CustomerService(repository);

        // Act
        GetAllCustomersResponse response =
            await customerService.GetAllCustomersAsync();

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Customers);
        Assert.Equal(3, response.Customers.Count);

        Assert.Equal("Anna", response.Customers[0].CustomerName);
        Assert.Equal("Erik", response.Customers[1].CustomerName);
        Assert.Equal("Mia", response.Customers[2].CustomerName);

        Assert.Null(response.ErrorMessage);

        await repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllCustomersAsync_Should_ReturnFailureWhenRepositoryThrows()
    {
        // Arrange
        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetAllAsync()
            .Returns(Task.FromException<List<Customer>>(
                new Exception("Ett fel uppstod")));

        ICustomerService customerService =
            new CustomerService(repository);

        // Act
        GetAllCustomersResponse response =
            await customerService.GetAllCustomersAsync();

        // Assert
        Assert.False(response.Success);
        Assert.Empty(response.Customers);
        Assert.Equal("Ett fel uppstod", response.ErrorMessage);
    }

    // GetCustomerById
    [Fact]
    public async Task GetCustomerByIdAsync_Should_ReturnCustomerWhenFound()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer expectedCustomer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false);

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(expectedCustomer));

        ICustomerService customerService =
            new CustomerService(repository);

        // Act
        GetCustomerByIdResponse response =
            await customerService.GetCustomerByIdAsync(customerId);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Customer);
        Assert.Equal(customerId, response.Customer.CustomerId);
        Assert.Equal("Mia", response.Customer.CustomerName);
        Assert.Null(response.ErrorMessage);

        await repository.Received(1).GetByIdAsync(customerId);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_Should_ReturnFailureWhenCustomerNotFound()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(null));

        ICustomerService customerService =
            new CustomerService(repository);

        // Act
        GetCustomerByIdResponse response =
            await customerService.GetCustomerByIdAsync(customerId);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal(
            "Något gick fel, försök igen",
            response.ErrorMessage);
    }

    // UpdateCustomer
    [Fact]
    public async Task UpdateCustomerAsync_Should_UpdateCustomerWhenValid()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer existingCustomer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false);

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(existingCustomer));

        repository.UpdateAsync(Arg.Any<Customer>())
            .Returns(Task.FromResult(existingCustomer));

        ICustomerService customerService =
            new CustomerService(repository);

        UpdateCustomerRequest request = new(
            customerId,
            "Mia Andersson",
            "mia.andersson@mail.com",
            true);

        // Act
        UpdateCustomerResponse response =
            await customerService.UpdateCustomerAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.NotNull(response.Customer);

        Assert.Equal("Mia Andersson", response.Customer.CustomerName);
        Assert.Equal("mia.andersson@mail.com", response.Customer.EmailAddress);
        Assert.True(response.Customer.IsCompany);
        Assert.Equal(customerId, response.Customer.CustomerId);
        Assert.Null(response.ErrorMessage);

        await repository.Received(1).UpdateAsync(
            Arg.Is<Customer>(customer =>
                customer.CustomerId == customerId &&
                customer.CustomerName == "Mia Andersson" &&
                customer.EmailAddress == "mia.andersson@mail.com" &&
                customer.IsCompany));
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_ReturnFailureWhenCustomerNotFound()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(null));

        ICustomerService customerService =
            new CustomerService(repository);

        UpdateCustomerRequest request = new(
            customerId,
            "Mia Andersson",
            "mia.andersson@mail.com",
            true);

        // Act
        UpdateCustomerResponse response =
            await customerService.UpdateCustomerAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal("Kunden kunde inte hittas", response.ErrorMessage);

        await repository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_ReturnFailureWhenNameIsEmpty()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer existingCustomer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false);

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(existingCustomer));

        ICustomerService customerService =
            new CustomerService(repository);

        UpdateCustomerRequest request = new(
            customerId,
            "",
            "mia@mail.com",
            false);

        // Act
        UpdateCustomerResponse response =
            await customerService.UpdateCustomerAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal(
            "Kundens namn kan inte tas bort",
            response.ErrorMessage);

        await repository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_ReturnFailureWhenEmailIsInvalid()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer existingCustomer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false);

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(existingCustomer));

        ICustomerService customerService =
            new CustomerService(repository);

        UpdateCustomerRequest request = new(
            customerId,
            "Mia",
            "inte-en-giltig-e-postadress",
            false);

        // Act
        UpdateCustomerResponse response =
            await customerService.UpdateCustomerAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal(
            "E-postadressen måste vara en giltig e-postadress",
            response.ErrorMessage);
        Assert.Equal("mia@mail.com", existingCustomer.EmailAddress);

        await repository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());
    }

    [Fact]
    public async Task UpdateCustomerAsync_Should_ReturnFailureWhenRepositoryThrows()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer existingCustomer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false);

        ICustomerRepository repository =
            Substitute.For<ICustomerRepository>();

        repository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(existingCustomer));

        repository.UpdateAsync(Arg.Any<Customer>())
            .Returns(Task.FromException<Customer>(
                new Exception("Kunde inte uppdatera kunden")));

        ICustomerService customerService =
            new CustomerService(repository);

        UpdateCustomerRequest request = new(
            customerId,
            "Mia Andersson",
            "mia.andersson@mail.com",
            true);

        // Act
        UpdateCustomerResponse response =
            await customerService.UpdateCustomerAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Null(response.Customer);
        Assert.Equal(
            "Kunde inte uppdatera kunden",
            response.ErrorMessage);

        await repository.Received(1).UpdateAsync(
            Arg.Is<Customer>(customer =>
                customer.CustomerId == customerId &&
                customer.CustomerName == "Mia Andersson" &&
                customer.EmailAddress == "mia.andersson@mail.com" &&
                customer.IsCompany));
    }
}
