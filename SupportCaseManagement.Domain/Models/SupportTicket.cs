using SupportCaseManagement.Domain.Enums;

namespace SupportCaseManagement.Domain.Models;

public class SupportTicket
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public Guid CustomerId { get; private set; }
    public TicketPriority Priority { get; private set; }
    public TicketStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string? AssignedCaseWorker { get; private set; }
    public Customer Customer { get; init; }

    // Privat konstruktor för deserialisering/ORM
    private SupportTicket() { }

    public SupportTicket(string title, string description, Guid customerId, TicketPriority priority)
    {
        // 3.2 Validering i domänen
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Rubrik får inte vara tom.", nameof(title));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivning får inte vara tom.", nameof(description));

        if (customerId == Guid.Empty)
            throw new ArgumentException("En giltig kund måste anges.", nameof(customerId));

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description.Trim();
        CustomerId = customerId;
        Priority = priority;
        Status = TicketStatus.New; // Status sätts automatiskt till Nytt (New)
        CreatedAt = DateTime.UtcNow;
    }

    public void AssignCaseWorker(string? name)
    {
        if (Status != TicketStatus.New && string.IsNullOrWhiteSpace(name))
        {

            throw new ArgumentException
            (
                "Ett pågående eller löst ärende måste ha en handlägare"
            );
        }

        AssignedCaseWorker = string.IsNullOrWhiteSpace(name)
            ? null
            : name.Trim();

    }

    public void ChangeStatus(TicketStatus newStatus)
    {
        if (!Enum.IsDefined(newStatus))
        {
            throw new ArgumentException("Ogiltig status");
        }

        if(newStatus != TicketStatus.New && string.IsNullOrWhiteSpace(AssignedCaseWorker))
        {
            throw new InvalidOperationException(
                "Välj en handläggare innan du ändrar status");
        }

        Status = newStatus;
    }

}
