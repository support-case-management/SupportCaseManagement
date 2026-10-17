using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Models;
using SupportCaseManagement.Domain.ValueObjects;

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
}