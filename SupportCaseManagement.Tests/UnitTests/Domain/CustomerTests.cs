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

    [Theory]
    [InlineData("mia@")]
    [InlineData("stellaco.com")]
    public void EnsureRequiredEmailAddress_Should_ThrowExceptionWhenEmailAddressIsNotAValidEmail(string emailAddress)
    {
        // Arrange
        Guid id = Guid.NewGuid();
        string customerName = "Mia";
        bool isCompany = false;

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new Customer(id, customerName, emailAddress!, isCompany));

        // Assert
        Assert.Equal("E-postadressen måste vara en giltig e-postadress", exception.Message);
    }


    [Fact]
    public void Rename_Should_UpdateCustomerNameWhenNameIsValid()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        string newName = "Stella";

        // Act
        customer.Rename(newName);

        // Assert
        Assert.Equal(newName, customer.CustomerName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Rename_Should_ThrowExceptionWhenNameIsNullOrWhiteSpace(
        string? customerName)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => customer.Rename(customerName!));

        // Assert
        Assert.Equal(
            "Kundnamn är ett obligatoriskt fält",
            exception.Message);

        Assert.Equal("Mia", customer.CustomerName);
    }

    [Fact]
    public void Rename_Should_ThrowExceptionWhenNameIsTooShort()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => customer.Rename("M"));

        // Assert
        Assert.Equal(
            "Kundens namn måsta vara minst 2 bokstäver långt",
            exception.Message);

        Assert.Equal("Mia", customer.CustomerName);
    }

    [Fact]
    public void Rename_Should_TrimWhiteSpacesFromNewName()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        customer.Rename("  Stella  ");

        // Assert
        Assert.Equal("Stella", customer.CustomerName);
    }

    [Fact]
    public void ChangeEmailAddress_Should_UpdateEmailWhenAddressIsValid()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        customer.ChangeEmailAddress("stella@mail.com");

        // Assert
        Assert.Equal("stella@mail.com", customer.EmailAddress);
    }

    [Fact]
    public void ChangeEmailAddress_Should_TrimAndConvertEmailToLowerCase()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        customer.ChangeEmailAddress("  STELLA@MAIL.COM  ");

        // Assert
        Assert.Equal("stella@mail.com", customer.EmailAddress);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ChangeEmailAddress_Should_ThrowExceptionWhenEmailIsNullOrWhiteSpace(
        string? emailAddress)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => customer.ChangeEmailAddress(emailAddress!));

        // Assert
        Assert.Equal(
            "E-postadress är ett obligatoriskt fält",
            exception.Message);

        Assert.Equal("mia@mail.com", customer.EmailAddress);
    }

    [Theory]
    [InlineData("mia@")]
    [InlineData("stellaco.com")]
    public void ChangeEmailAddress_Should_ThrowExceptionWhenEmailIsInvalid(
        string emailAddress)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(
            () => customer.ChangeEmailAddress(emailAddress));

        // Assert
        Assert.Equal(
            "E-postadressen måste vara en giltig e-postadress",
            exception.Message);

        Assert.Equal("mia@mail.com", customer.EmailAddress);
    }


    [Fact]
    public void ChangeIsCompany_Should_UpdateIsCompanyToTrue()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        customer.ChangeIsCompany(true);

        // Assert
        Assert.True(customer.IsCompany);
    }

    [Fact]
    public void ChangeIsCompany_Should_UpdateIsCompanyToFalse()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Stella AB",
            "info@stella.se",
            true);

        // Act
        customer.ChangeIsCompany(false);

        // Assert
        Assert.False(customer.IsCompany);
    }

    [Theory]
    [InlineData(true, "Företag")]
    [InlineData(false, "Privatkund")]
    public void CustomerType_Should_ReturnCorrectType(
        bool isCompany,
        string expectedCustomerType)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            isCompany);

        // Act
        string customerType = customer.CustomerType;

        // Assert
        Assert.Equal(expectedCustomerType, customerType);
    }


    [Fact]
    public void Constructor_Should_CreateCustomerWithEmptySupportTickets()
    {
        // Arrange & Act
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Assert
        Assert.Empty(customer.SupportTickets);
    }

    [Fact]
    public void AddSupportTicket_Should_AddTicketIdToSupportTickets()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        SupportTicket ticket = new("Test", "Testning av kundmodellen", customer.CustomerId, SupportCaseManagement.Domain.Enums.TicketPriority.Normal, customer);

        // Act
        customer.AddSupportTicket(ticket);

        // Assert
        Assert.Contains(ticket.Id, customer.SupportTickets);
        Assert.Single(customer.SupportTickets);
    }

    [Fact]
    public void AddSupportTicket_Should_NotAddAnythingWhenTicketIsNull()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        customer.AddSupportTicket(null!);

        // Assert
        Assert.Empty(customer.SupportTickets);
    }

    [Fact]
    public void AddSupportTicket_Should_AllowMultipleSupportTickets()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        SupportTicket firstTicket = new("Test", "Testning om kunden kan ta flera ärenden", customer.CustomerId, SupportCaseManagement.Domain.Enums.TicketPriority.Normal, customer);

        SupportTicket secondTicket = new("Test nr 2", "Test om modellen kan ha flera ärenden", customer.CustomerId, SupportCaseManagement.Domain.Enums.TicketPriority.Normal, customer);

        // Act
        customer.AddSupportTicket(firstTicket);
        customer.AddSupportTicket(secondTicket);

        // Assert
        Assert.Equal(2, customer.SupportTickets.Count);
        Assert.Contains(firstTicket.Id, customer.SupportTickets);
        Assert.Contains(secondTicket.Id, customer.SupportTickets);
    }

    [Fact]
    public void JsonConstructor_Should_CreateCustomerWithProvidedSupportTickets()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid firstTicketId = Guid.NewGuid();
        Guid secondTicketId = Guid.NewGuid();

        IReadOnlyList<Guid> ticketIds =
    [
        firstTicketId,
        secondTicketId
    ];

        // Act
        Customer customer = new(
            customerId,
            "Mia",
            "mia@mail.com",
            false,
            ticketIds);

        // Assert
        Assert.Equal(customerId, customer.CustomerId);
        Assert.Equal("Mia", customer.CustomerName);
        Assert.Equal("mia@mail.com", customer.EmailAddress);
        Assert.False(customer.IsCompany);
        Assert.Equal(2, customer.SupportTickets.Count);
        Assert.Contains(firstTicketId, customer.SupportTickets);
        Assert.Contains(secondTicketId, customer.SupportTickets);
    }

    [Fact]
    public void JsonConstructor_Should_CreateEmptySupportTicketsWhenListIsNull()
    {
        // Arrange & Act
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false,
            null);

        // Assert
        Assert.Empty(customer.SupportTickets);
    }

    [Fact]
    public void JsonConstructor_Should_CopyProvidedSupportTickets()
    {
        // Arrange
        List<Guid> ticketIds = [Guid.NewGuid()];

        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false,
            ticketIds);

        // Act
        ticketIds.Add(Guid.NewGuid());

        // Assert
        Assert.Single(customer.SupportTickets);
    }

    [Fact]
    public void SupportTickets_Should_NotAllowModificationThroughReadOnlyCollection()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        // Act
        var supportTickets = customer.SupportTickets;

        // Assert
        Assert.IsAssignableFrom<IReadOnlyList<Guid>>(supportTickets);
        Assert.IsNotType<List<Guid>>(supportTickets);
    }

    [Fact]
    public void SupportTickets_Should_ThrowWhenModifiedThroughCollectionInterface()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Mia",
            "mia@mail.com",
            false);

        var supportTickets = customer.SupportTickets;

        // Act & Assert
        Assert.Throws<NotSupportedException>(() =>
            ((ICollection<Guid>)supportTickets).Add(Guid.NewGuid()));
    }
}
