using NSubstitute;
using SupportCaseManagement.Application.Features.SupportTicketList;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Application;

public class SupportTicketListServiceTests
{
    // hjälpmetoder för att göra testningen smidigare
    private static Customer CreateCustomer(string name)
    {
        return new Customer(
            Guid.NewGuid(),
            name,
            $"{name.Replace(" ", "").ToLower()}@mail.com",
            false);
    }

    private static SupportTicket CreateTicket(
        string title,
        Customer customer,
        DateTime? createdAt = null,
        TicketStatus status = TicketStatus.New)
    {
        return new SupportTicket(
            Guid.NewGuid(),
            title,
            "Testbeskrivning",
            customer.CustomerId,
            TicketPriority.Normal,
            status,
            createdAt ?? DateTime.UtcNow,
            status == TicketStatus.New ? null : "Stella",
            customer,
            []);
    }

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

    [Fact]
    public async Task GetAllTicketsSortedAsync_Should_SortTicketsByNewestFirst()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var oldest = CreateTicket(
            "Äldst", customer, DateTime.UtcNow.AddDays(-3));

        var newest = CreateTicket(
            "Nyast", customer, DateTime.UtcNow.AddDays(-1));

        var middle = CreateTicket(
            "Mitten", customer, DateTime.UtcNow.AddDays(-2));

        var tickets = new List<SupportTicket>
        {
        oldest, newest, middle
        };

        var repository = Substitute.For<ISupportTicketRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(tickets));

        var service = new SupportTicketListService(repository);

        // Act
        var result = (await service.GetAllTicketsSortedAsync()).ToList();

        // Assert
        Assert.Collection(
            result,
            ticket => Assert.Equal("Nyast", ticket.Title),
            ticket => Assert.Equal("Mitten", ticket.Title),
            ticket => Assert.Equal("Äldst", ticket.Title));
    }

    [Fact]
    public async Task GetAllTicketsSortedAsync_Should_ReturnEmptyListWhenNoTicketsExist()
    {
        // Arrange
        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync()
            .Returns(Task.FromResult(new List<SupportTicket>()));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetAllTicketsSortedAsync();

        // Assert
        Assert.Empty(result);
        await repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetTicketsWithAsync_Should_ReturnOnlyTicketsWithRequestedStatus()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var newTicket = CreateTicket(
            "Nytt ärende", customer, status: TicketStatus.New);

        var inProgressTicket = CreateTicket(
            "Pågående ärende", customer,
            status: TicketStatus.InProgress);

        var closedTicket = CreateTicket(
            "Avslutat ärende", customer,
            status: TicketStatus.Closed);

        var tickets = new List<SupportTicket>
        {
        newTicket, inProgressTicket, closedTicket
        };

        var repository = Substitute.For<ISupportTicketRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(tickets));

        var service = new SupportTicketListService(repository);

        // Act
        var result = (await service.GetTicketsWithAsync(
            TicketStatus.InProgress)).ToList();

        // Assert
        var ticket = Assert.Single(result);
        Assert.Equal("Pågående ärende", ticket.Title);
        Assert.Equal(TicketStatus.InProgress, ticket.Status);
        Assert.True(service.IsFilterOn);
    }

    [Fact]
    public async Task GetTicketsWithAsync_Should_ReturnEmptyListWhenNoTicketsMatchStatus()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var ticket = CreateTicket(
            "Nytt ärende", customer, status: TicketStatus.New);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetTicketsWithAsync(TicketStatus.Closed);

        // Assert
        Assert.Empty(result);
        Assert.True(service.IsFilterOn);
    }

    [Fact]
    public async Task GetTicketsWithAsync_Should_SortTicketsByNewestFirst()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var oldest = CreateTicket(
            "Äldst", customer, DateTime.UtcNow.AddDays(-3));

        var newest = CreateTicket(
            "Nyast", customer, DateTime.UtcNow.AddDays(-1));

        var middle = CreateTicket(
            "Mitten", customer, DateTime.UtcNow.AddDays(-2));

        var tickets = new List<SupportTicket>
        {
        oldest, newest, middle
        };

        var repository = Substitute.For<ISupportTicketRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(tickets));

        var service = new SupportTicketListService(repository);

        // Act
        var result = (await service.GetTicketsWithAsync(TicketStatus.New)).ToList();

        // Assert
        Assert.Collection(
            result,
            ticket => Assert.Equal("Nyast", ticket.Title),
            ticket => Assert.Equal("Mitten", ticket.Title),
            ticket => Assert.Equal("Äldst", ticket.Title));
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_FindTicketsByTitle()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var matchingTicket = CreateTicket(
            "Problem med inloggning", customer);

        var otherTicket = CreateTicket(
            "Problem med betalning", customer);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket>
            {
            matchingTicket, otherTicket
            }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = (await service.GetTicketsContainingAsync("inloggning"))
            .ToList();

        // Assert
        var ticket = Assert.Single(result);
        Assert.Equal("Problem med inloggning", ticket.Title);
    }

    [Theory]
    [InlineData("INLOGGNING")]
    [InlineData("inloggning")]
    [InlineData("InLoGgNiNg")]
    public async Task GetTicketsContainingAsync_Should_SearchCaseInsensitively(
        string searchText)
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var ticket = CreateTicket(
            "Problem med inloggning", customer);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetTicketsContainingAsync(searchText);

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_FindTicketsByCustomerName()
    {
        // Arrange
        Customer customer = CreateCustomer("Stella");

        var ticket = CreateTicket("Tekniskt problem", customer);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetTicketsContainingAsync("stell");

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_TrimSearchText()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var ticket = CreateTicket("Problem med inloggning", customer);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetTicketsContainingAsync("  inloggning  ");

        // Assert
        Assert.Single(result);
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_ReturnEmptyListWhenNoMatchExists()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var ticket = CreateTicket("Problem med inloggning", customer);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        // Act
        var result = await service.GetTicketsContainingAsync("faktura");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_SearchOnlyFilteredTicketsWhenFilterIsOn()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var newTicket = CreateTicket(
            "Problem med inloggning", customer,
            status: TicketStatus.New);

        var closedTicket = CreateTicket(
            "Problem med inloggning", customer,
            status: TicketStatus.Closed);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket>
            {
            newTicket, closedTicket
            }));

        var service = new SupportTicketListService(repository);

        // Act
        await service.GetTicketsWithAsync(TicketStatus.New);

        var result = (await service.GetTicketsContainingAsync("inloggning"))
            .ToList();

        // Assert
        var ticket = Assert.Single(result);
        Assert.Equal(newTicket.Id, ticket.Id);

        await repository.Received(1).GetAllAsync();
    }

    [Fact]
    public async Task GetAllTicketsSortedAsync_Should_TurnOffActiveFilter()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var ticket = CreateTicket(
            "Nytt ärende", customer, status: TicketStatus.New);

        var repository = Substitute.For<ISupportTicketRepository>();

        repository.GetAllAsync().Returns(
            Task.FromResult(new List<SupportTicket> { ticket }));

        var service = new SupportTicketListService(repository);

        await service.GetTicketsWithAsync(TicketStatus.New);
        Assert.True(service.IsFilterOn);

        // Act
        await service.GetAllTicketsSortedAsync();

        // Assert
        Assert.False(service.IsFilterOn);
    }

    [Fact]
    public async Task GetTicketsContainingAsync_Should_SortTicketsByNewestFirst()
    {
        // Arrange
        Customer customer = CreateCustomer("Mia");

        var oldest = CreateTicket(
            "Äldst", customer, DateTime.UtcNow.AddDays(-3));

        var newest = CreateTicket(
            "Nyast", customer, DateTime.UtcNow.AddDays(-1));

        var middle = CreateTicket(
            "Mitten", customer, DateTime.UtcNow.AddDays(-2));

        var tickets = new List<SupportTicket>
        {
        oldest, newest, middle
        };

        var repository = Substitute.For<ISupportTicketRepository>();
        repository.GetAllAsync().Returns(Task.FromResult(tickets));

        var service = new SupportTicketListService(repository);

        // Act
        var result = (await service.GetTicketsContainingAsync("t")).ToList();

        // Assert
        Assert.Collection(
            result,
            ticket => Assert.Equal("Nyast", ticket.Title),
            ticket => Assert.Equal("Mitten", ticket.Title),
            ticket => Assert.Equal("Äldst", ticket.Title));
    }
}
