

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
}