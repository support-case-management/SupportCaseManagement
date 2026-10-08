using SupportCaseManagement.Domain.Enums;
using System.Net.WebSockets;
using System.Text.Json.Serialization;

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
    public IReadOnlyList<Comment> Comments => _comments.AsReadOnly();
    private readonly List<Comment> _comments = [];

    [JsonIgnore]
    public string SupportTicketStatus
    {
        get
        {
            string supportTicketStatus = string.Empty;

            switch(Status)
            {
                case TicketStatus.New:
                    supportTicketStatus = "Nytt";
                    break;

                case TicketStatus.InProgress:
                    supportTicketStatus = "Pågående";
                    break;

                case TicketStatus.Closed:
                    supportTicketStatus = "Löst";
                    break;
            }

            return supportTicketStatus;
        }
    }

    [JsonIgnore]
    public string SupportTicketPriority
    {
        get
        {
            string supportTicketStatus = string.Empty;

            switch(Priority)
            {
                case TicketPriority.Low:
                    supportTicketStatus = "Låg";
                    break;

                case TicketPriority.Normal:
                    supportTicketStatus = "Normal";
                    break;

                case TicketPriority.High:
                    supportTicketStatus = "Hög";
                    break;
            }

            return supportTicketStatus;
        }
    }

    // Återskapar ett Ärende från Json med sparat ID och datum. Används vid deserialisering av ärenden från fil.

    [JsonConstructor]
    public SupportTicket(
        Guid id,
        string title,
        string description,
        Guid customerId,
        TicketPriority priority,
        TicketStatus status,
        DateTime createdAt,
        string? assignedCaseWorker,
        IReadOnlyList<Comment> comments)

    {

        Id = id;
        CustomerId = customerId;
        CreatedAt = createdAt;

        UpdateDetails(title, description);
        ChangePriority(priority);
        AssignCaseWorker(assignedCaseWorker);
        ChangeStatus(status);

        if (comments is not null)
        {
            _comments.AddRange(comments);
        }
    }

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

        if (newStatus != TicketStatus.New && string.IsNullOrWhiteSpace(AssignedCaseWorker))
        {
            throw new InvalidOperationException(
                "Välj en handläggare innan du ändrar status");
        }

        Status = newStatus;
    }
    public void ChangePriority(TicketPriority newPriority)
    {
        if (!Enum.IsDefined(newPriority))
        {
            throw new ArgumentException("Ogiltig prioritet");
        }
        Priority = newPriority;
    }


    //lägger till en kommentar med aktuell tid. Tom text avvisas

    public void AddComment(string text)
    {
        var comment = new Comment(text, DateTime.UtcNow);
        _comments.Add(comment);
    }

    public void UpdateDetails(string title, string description)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Rubrik får inte vara tom.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Beskrivning får inte vara tom.");

        Title = title.Trim();
        Description = description.Trim();
    }

}
