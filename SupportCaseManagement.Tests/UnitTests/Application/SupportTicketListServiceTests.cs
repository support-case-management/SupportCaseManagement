using NSubstitute;
using SupportCaseManagement.Application.Features.SupportTicketList;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Application;

public class SupportTicketListServiceTests
{
    [Fact]
    public async Task GetAllTicketsSortedAsync_Should_ReturnListOfSupportTickets()
    {
        // Arrange 
        Customer customer = new(Guid.NewGuid(), "Mia", "mia@mail.com", false);

        List<SupportTicket> expectedList = [
            // gör en med json konstruktorn för att kunna sätta en annan tid än DateTime.UtcNow på den ena
            new SupportTicket(
                Guid.NewGuid(),
                "Test",
                "Test för sorteringen av listan",
                customer.CustomerId,
                TicketPriority.Normal,
                TicketStatus.New,
                DateTime.UtcNow.AddDays(2),
                null,
                customer,
                []),
            new SupportTicket(
                "Test",
                "Test för sorteringen av listan",
                customer.CustomerId,
                TicketPriority.Normal,
                customer)];

        ISupportTicketRepository repository = Substitute.For<ISupportTicketRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(expectedList));

        ISupportTicketListService supportTicketListService = new SupportTicketListService(repository);

        // Act
        var actualList = await supportTicketListService.GetAllTicketsSortedAsync();

        // Assert
        Assert.Equal(expectedList.OrderByDescending(ticket => ticket.CreatedAt),
            actualList);
        await repository.Received(1).GetAllAsync();
    }
}
