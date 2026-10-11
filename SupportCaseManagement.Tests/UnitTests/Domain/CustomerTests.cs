using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Domain;

public class CustomerTests
{
    [Fact]
    public void Constructor_Should_CreateCustomerWhenValuesAreValid()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string customerName = "Mia";
        string emailAddress = "mia@mail.com";
        bool isCompany = false;

        // Act
        Customer customer = new(id, customerName, emailAddress, isCompany);

        // Assert
        Assert.Equal(id, customer.CustomerId);
        Assert.Equal(customerName, customer.CustomerName);
        Assert.Equal(emailAddress, customer.EmailAddress);
        Assert.False(customer.IsCompany);
    }
}
