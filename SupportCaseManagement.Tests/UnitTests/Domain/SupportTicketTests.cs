using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Domain;

public class SupportTicketTests
{
    [Fact]
    public void ChangeStatus_WithoutCaseWorker_ThrowsInvalidOperationException()
    {
        // Arrange
        var customer = new Customer(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        var ticket = new SupportTicket(
            "Skrivaren fungerar inte",
            "Det går inte att skriva ut",
            customer.CustomerId,
            TicketPriority.Normal,
            customer);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.InProgress));
        Assert.Equal(TicketStatus.New, ticket.Status);


    }

    [Fact]
    public void ChangeStatus_WithCaseWorker_ChangesStatusToInProgress()
    {
        // Arrange
        var customer = new Customer(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        var ticket = new SupportTicket(
            "Skrivaren fungerar inte",
            "Det går inte att skriva ut",
            customer.CustomerId,
            TicketPriority.Normal,
            customer);

        ticket.AssignCaseWorker("Mia");

        // Act
        ticket.ChangeStatus(TicketStatus.InProgress);

        // Assert
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
    }

    [Fact]
    public void ChangeStatus_ToClosedWithoutCaseWorker_ThrowsInvalidOperationException()
    {
        // Arrange
        var customer = new Customer(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        var ticket = new SupportTicket(
            "Skrivaren fungerar inte",
            "Det går inte att skriva ut",
            customer.CustomerId,
            TicketPriority.Normal,
            customer);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
            ticket.ChangeStatus(TicketStatus.Closed));

        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    [Fact]
    public void Constructor_Should_CreateSupportTicketWhenValuesAreValid()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        // Act
        SupportTicket ticket = new(
            "Problem",
            "Någonting funkar inte",
            customer.CustomerId,
            TicketPriority.Normal,
            customer);

        // Assert
        Assert.NotEqual(ticket.Id, Guid.Empty);
        Assert.Equal("Problem", ticket.Title);
        Assert.Equal("Någonting funkar inte", ticket.Description);
        Assert.Equal(customer.CustomerId, ticket.CustomerId);
        Assert.Equal(TicketPriority.Normal, ticket.Priority);
        Assert.Equal(TicketStatus.New, ticket.Status);
        Assert.Equal(customer, ticket.Customer);
        Assert.Null(ticket.AssignedCaseWorker);
        Assert.Equal([], ticket.Comments);
        Assert.Equal("Nytt", ticket.SupportTicketStatus);
        Assert.Equal("Normal", ticket.SupportTicketPriority);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void EnsureRequiredTitle_Should_ThrowExceptionWhenTitleIsNullOrWhiteSpace(string? title)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                title!,
                "Någonting funkar inte",
                customer.CustomerId,
                TicketPriority.Normal,
                customer));

        // Assert
        Assert.Equal("Rubrik får inte vara tom. (Parameter 'title')", exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void EnsureRequiredDescription_Should_ThrowExceptionWhenDescriptionIsNullOrWhiteSpace(string? description)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                "Problem",
                description!,
                customer.CustomerId,
                TicketPriority.Normal,
                customer));

        // Assert
        Assert.Equal("Beskrivning får inte vara tom. (Parameter 'description')", exception.Message);
    }

    [Fact]
    public void EnsureRequiredCustomerId_Should_ThrowExceptionWhenCustomerIdIsGuidEmpty()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(),
            "Testkund",
            "test@example.com",
            false);

        // Act
        var exception = Assert.Throws<ArgumentException>(() =>
            new SupportTicket(
                "Problem",
                "Någonting fungerar inte",
                Guid.Empty!,
                TicketPriority.Normal,
                customer));

        // Assert
        Assert.Equal("En giltig kund måste anges. (Parameter 'customerId')", exception.Message);
    }

    // UpdateDetails
    [Fact]
    public void UpdateDetails_Should_UpdateAndTrimTitleAndDescription()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Gammal rubrik", "Gammal beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        ticket.UpdateDetails("  Ny rubrik  ", "  Ny beskrivning  ");

        // Assert
        Assert.Equal("Ny rubrik", ticket.Title);
        Assert.Equal("Ny beskrivning", ticket.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateDetails_Should_ThrowWhenTitleIsInvalid(string? title)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Gammal rubrik", "Gammal beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        Assert.Throws<ArgumentException>(() =>
            ticket.UpdateDetails(title!, "Ny beskrivning"));

        // Assert
        Assert.Equal("Gammal rubrik", ticket.Title);
        Assert.Equal("Gammal beskrivning", ticket.Description);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void UpdateDetails_Should_ThrowWhenDescriptionIsInvalid(string? description)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Gammal rubrik", "Gammal beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        Assert.Throws<ArgumentException>(() =>
            ticket.UpdateDetails("Ny titel", description!));

        // Assert
        Assert.Equal("Gammal rubrik", ticket.Title);
        Assert.Equal("Gammal beskrivning", ticket.Description);
    }

    // AssignCaseWorker
    [Fact]
    public void AssignCaseWorker_Should_TrimCaseWorkerName()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        ticket.AssignCaseWorker("  Mia  ");

        // Assert
        Assert.Equal("Mia", ticket.AssignedCaseWorker);
    }

    [Fact]
    public void AssignCaseWorker_Should_AllowRemovingCaseWorkerWhenStatusIsNew()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        ticket.AssignCaseWorker("Mia");

        // Act
        ticket.AssignCaseWorker(null);

        // Assert
        Assert.Null(ticket.AssignedCaseWorker);
    }

    [Fact]
    public void AssignCaseWorker_Should_ThrowWhenRemovingCaseWorkerFromInProgressTicket()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        ticket.AssignCaseWorker("Mia");
        ticket.ChangeStatus(TicketStatus.InProgress);

        // Act 
        Assert.Throws<ArgumentException>(() =>
            ticket.AssignCaseWorker(null));

        // Assert
        Assert.Equal("Mia", ticket.AssignedCaseWorker);
    }

    // ChangePriority
    [Theory]
    [InlineData(TicketPriority.Low)]
    [InlineData(TicketPriority.Normal)]
    [InlineData(TicketPriority.High)]
    public void ChangePriority_Should_SetPriority(TicketPriority priority)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        ticket.ChangePriority(priority);

        // Assert
        Assert.Equal(priority, ticket.Priority);
    }

    [Fact]
    public void ChangePriority_Should_ThrowWhenPriorityIsInvalid()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        Assert.Throws<ArgumentException>(() =>
            ticket.ChangePriority((TicketPriority)999));

        // Assert
        Assert.Equal(TicketPriority.Normal, ticket.Priority);
    }

    // ChangeStatus
    [Fact]
    public void ChangeStatus_Should_ThrowWhenStatusIsInvalid()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        Assert.Throws<ArgumentException>(() =>
            ticket.ChangeStatus((TicketStatus)999));

        // Assert
        Assert.Equal(TicketStatus.New, ticket.Status);
    }

    // AddComment
    [Fact]
    public void AddComment_Should_AddCommentToTicket()
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        // Act
        ticket.AddComment("Vi undersöker problemet.");

        // Assert
        Assert.Single(ticket.Comments);
        Assert.NotNull(ticket.Comments[0]);
        Assert.Equal("Vi undersöker problemet.", ticket.Comments[0].Text);
    }

    // SupportTicketStatus
    [Theory]
    [InlineData(TicketStatus.New, "Nytt")]
    [InlineData(TicketStatus.InProgress, "Pågående")]
    [InlineData(TicketStatus.Closed, "Löst")]
    public void SupportTicketStatus_Should_ReturnCorrectText(
    TicketStatus status,
    string expectedText)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        if (status != TicketStatus.New)
        {
            ticket.AssignCaseWorker("Mia");
        }

        ticket.ChangeStatus(status);

        // Act
        string result = ticket.SupportTicketStatus;

        // Assert
        Assert.Equal(expectedText, result);
    }

    // SupportTicketPriority
    [Theory]
    [InlineData(TicketPriority.Low, "Låg")]
    [InlineData(TicketPriority.Normal, "Normal")]
    [InlineData(TicketPriority.High, "Hög")]
    public void SupportTicketPriority_Should_ReturnCorrectText(
    TicketPriority priority,
    string expectedText)
    {
        // Arrange
        Customer customer = new(
            Guid.NewGuid(), "Testkund", "test@example.com", false);

        SupportTicket ticket = new(
            "Problem", "Beskrivning",
            customer.CustomerId, TicketPriority.Normal, customer);

        ticket.ChangePriority(priority);

        // Act
        string result = ticket.SupportTicketPriority;

        // Assert
        Assert.Equal(expectedText, result);
    }
}