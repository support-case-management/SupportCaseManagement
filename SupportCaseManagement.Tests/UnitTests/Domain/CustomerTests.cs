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

    [Fact]
    public void EnsureRequiredId_Should_ThrowwExceptionWhenIdIsEmpty()
    {
        // Arrange
        Guid id = Guid.Empty;
        string customerName = "Mia";
        string emailAddress = "mia@mail.com";
        bool isCompany = false;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(id, customerName, emailAddress, isCompany));

        // Assert
        Assert.Equal("Kund-Id är ett obligatoriskt fält", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void EnsureRequiredCustomerName_Should_ThrowExceptionWhenNameIsNullOrWhiteSpace(string? customerName)
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string emailAddress = "mia@mail.com";
        bool isCompany = false;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(id, customerName!, emailAddress, isCompany));

        // Assert
        Assert.Equal("Kundnamn är ett obligatoriskt fält", exception.Message);
    }

    [Fact]
    public void EnsureRequiredCustomerName_Should_ThrowExceptionWhenNameIsShorterThanMinLength()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string customerName = "M";
        string emailAddress = "mia@mail.com";
        bool isCompany = false;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(id, customerName!, emailAddress, isCompany));

        // Assert
        Assert.Equal("Kundens namn måsta vara minst 2 bokstäver långt", exception.Message);
    }

    [Theory]
    [InlineData(" Mia    ", "Mia")]
    [InlineData("\nStella & Co\n\t", "Stella & Co")]
    public void NormalizeCustomerName_Should_TrimWhiteSpaces_ReturnValidCustomerName(string customerName, string expectedCustomerName)
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string emailAddress = "mia@mail.com";
        bool isCompany = false;

        // Act
        var customer = new Customer(id, customerName, emailAddress, isCompany);

        // Assert
        Assert.Equal(expectedCustomerName, customer.CustomerName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void EnsureRequiredEmailAddress_Should_ThrowExceptionWhenEmailAddressIsNullOrWhiteSpace(string? emailAddress)
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string customerName = "Mia";
        bool isCompany = false;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(id, customerName, emailAddress!, isCompany));

        // Assert
        Assert.Equal("E-postadress är ett obligatoriskt fält", exception.Message);
    }
}
