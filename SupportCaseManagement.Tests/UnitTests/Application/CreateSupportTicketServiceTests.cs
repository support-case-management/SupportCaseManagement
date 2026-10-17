using NSubstitute;
using SupportCaseManagement.Application.Features.CreateSupportTicket;
using SupportCaseManagement.Domain.Enums;
using SupportCaseManagement.Domain.Interfaces;
using SupportCaseManagement.Domain.Models;

namespace SupportCaseManagement.Tests.UnitTests.Application;

public class CreateSupportTicketServiceTests
{
    [Fact]
    public async Task CreateSupportTicketAsync_Should_GenerateAndSaveSupportTicket_ReturnSupportTicketId()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        ICustomerRepository customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.GetByIdAsync(customerId).Returns(Task.FromResult<Customer?>(new Customer(customerId, "Testkund", "test@example.com", false)));

        ISupportTicketRepository ticketRepository = Substitute.For<ISupportTicketRepository>();
        ticketRepository.GetAllAsync().Returns(Task.FromResult(new List<SupportTicket>()));

        ICreateSupportTicketService customerService = new CreateSupportTicketService(ticketRepository, customerRepository);

        // Act
        Guid id = await customerService.CreateSupportTicketAsync("Problem", "Någonting går fel", customerId, TicketPriority.Normal);

        // Assert
        Assert.NotEqual(id, Guid.Empty);

        await ticketRepository.Received(1).SaveAllAsync(
            Arg.Is<IEnumerable<SupportTicket>>(tickets =>
            tickets.Count() == 1 &&
            tickets.Any(ticket => ticket.Id == id)));
    }


    [Fact]
    public async Task CreateSupportTicketAsync_Should_ThrowWhenCustomerDoesNotExist()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        ICustomerRepository customerRepository =
            Substitute.For<ICustomerRepository>();

        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(null));

        ISupportTicketRepository ticketRepository =
            Substitute.For<ISupportTicketRepository>();

        ICreateSupportTicketService service =
            new CreateSupportTicketService(ticketRepository, customerRepository);

        // Act
        InvalidOperationException exception =
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.CreateSupportTicketAsync(
                    "Problem",
                    "Någonting går fel",
                    customerId,
                    TicketPriority.Normal));

        // Assert
        Assert.Equal(
            "Denna kund finns inte i systemet.",
            exception.Message);

        await customerRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());

        await ticketRepository.DidNotReceive().GetAllAsync();
    }


    [Fact]
    public async Task CreateSupportTicketAsync_Should_AddTicketIdToCustomer()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer customer = new(
            customerId,
            "Testkund",
            "test@example.com",
            false);

        ICustomerRepository customerRepository =
            Substitute.For<ICustomerRepository>();

        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(customer));

        customerRepository.UpdateAsync(Arg.Any<Customer>())
            .Returns(Task.FromResult(customer));

        ISupportTicketRepository ticketRepository =
            Substitute.For<ISupportTicketRepository>();

        ticketRepository.GetAllAsync()
            .Returns(Task.FromResult(new List<SupportTicket>()));

        ICreateSupportTicketService service =
            new CreateSupportTicketService(ticketRepository, customerRepository);

        // Act
        Guid ticketId = await service.CreateSupportTicketAsync(
            "Problem",
            "Någonting går fel",
            customerId,
            TicketPriority.Normal);

        // Assert
        Assert.Contains(ticketId, customer.SupportTickets);

        await customerRepository.Received(1).UpdateAsync(
            Arg.Is<Customer>(updatedCustomer =>
                updatedCustomer.CustomerId == customerId &&
                updatedCustomer.SupportTickets.Contains(ticketId)));
    }


    [Fact]
    public async Task CreateSupportTicketAsync_Should_PreserveExistingTickets()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer customer = new(
            customerId,
            "Testkund",
            "test@example.com",
            false);

        SupportTicket existingTicket = new(
            "Befintligt problem",
            "Ett tidigare ärende",
            customerId,
            TicketPriority.Normal,
            customer);

        ICustomerRepository customerRepository =
            Substitute.For<ICustomerRepository>();

        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(customer));

        customerRepository.UpdateAsync(Arg.Any<Customer>())
            .Returns(Task.FromResult(customer));

        ISupportTicketRepository ticketRepository =
            Substitute.For<ISupportTicketRepository>();

        ticketRepository.GetAllAsync()
            .Returns(Task.FromResult(
                new List<SupportTicket> { existingTicket }));

        ICreateSupportTicketService service =
            new CreateSupportTicketService(ticketRepository, customerRepository);

        // Act
        Guid newTicketId = await service.CreateSupportTicketAsync(
            "Nytt problem",
            "Ett nytt ärende",
            customerId,
            TicketPriority.High);

        // Assert
        await ticketRepository.Received(1).SaveAllAsync(
            Arg.Is<IEnumerable<SupportTicket>>(tickets =>
                tickets.Count() == 2 &&
                tickets.Any(ticket => ticket.Id == existingTicket.Id) &&
                tickets.Any(ticket => ticket.Id == newTicketId)));
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task CreateSupportTicketAsync_Should_ThrowWhenTitleIsInvalid(
        string? title)
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer customer = new(
            customerId,
            "Testkund",
            "test@example.com",
            false);

        ICustomerRepository customerRepository =
            Substitute.For<ICustomerRepository>();

        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(customer));

        ISupportTicketRepository ticketRepository =
            Substitute.For<ISupportTicketRepository>();

        ICreateSupportTicketService service =
            new CreateSupportTicketService(
                ticketRepository,
                customerRepository);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateSupportTicketAsync(
                title!,
                "En giltig beskrivning",
                customerId,
                TicketPriority.Normal));

        // Kontrollera att kunden inte uppdateras
        await customerRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());

        // Kontrollera att inga ärenden hämtas eller sparas
        await ticketRepository.DidNotReceive().GetAllAsync();

        await ticketRepository.DidNotReceive()
            .SaveAllAsync(Arg.Any<IEnumerable<SupportTicket>>());
    }


    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task CreateSupportTicketAsync_Should_ThrowWhenDescriptionIsInvalid(
        string? description)
    {
        // Arrange
        Guid customerId = Guid.NewGuid();

        Customer customer = new(
            customerId,
            "Testkund",
            "test@example.com",
            false);

        ICustomerRepository customerRepository =
            Substitute.For<ICustomerRepository>();

        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(customer));

        ISupportTicketRepository ticketRepository =
            Substitute.For<ISupportTicketRepository>();

        ICreateSupportTicketService service =
            new CreateSupportTicketService(
                ticketRepository,
                customerRepository);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateSupportTicketAsync(
                "En giltig rubrik",
                description!,
                customerId,
                TicketPriority.Normal));

        // Kontrollera att kunden inte uppdateras
        await customerRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Customer>());

        // Kontrollera att inga ärenden hämtas eller sparas
        await ticketRepository.DidNotReceive().GetAllAsync();

        await ticketRepository.DidNotReceive()
            .SaveAllAsync(Arg.Any<IEnumerable<SupportTicket>>());
    }


    [Fact]
    public async Task CreateSupportTicketAsync_Should_ThrowWhenPriorityIsInvalid()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Customer customer = new(customerId, "Testkund", "test@example.com", false);

        ICustomerRepository customerRepository = Substitute.For<ICustomerRepository>();
        customerRepository.GetByIdAsync(customerId)
            .Returns(Task.FromResult<Customer?>(customer));

        ISupportTicketRepository ticketRepository = Substitute.For<ISupportTicketRepository>();

        ICreateSupportTicketService service =
            new CreateSupportTicketService(ticketRepository, customerRepository);

        // Act & Assert
        ArgumentException exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            service.CreateSupportTicketAsync(
                "Problem",
                "En beskrivning",
                customerId,
                (TicketPriority)999));

        Assert.Equal("Ogiltig prioritet", exception.Message);

        // Kontrollera att inget sparas när prioriteten är ogiltig
        await customerRepository.DidNotReceive().UpdateAsync(Arg.Any<Customer>());
        await ticketRepository.DidNotReceive().GetAllAsync();
        await ticketRepository.DidNotReceive().SaveAllAsync(
            Arg.Any<IEnumerable<SupportTicket>>());
    }

}
