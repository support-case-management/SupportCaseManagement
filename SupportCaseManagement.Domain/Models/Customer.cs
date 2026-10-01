using SupportCaseManagement.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportCaseManagement.Domain.Models;

public class Customer(Guid customerId, CustomerName customerName, EmailAddress emailAddress, bool isCompany)
{
    public Guid CustomerId { get; } = customerId;
    public CustomerName CustomerName { get; set; } = customerName;
    public EmailAddress EmailAddress { get; private set; } = emailAddress;
    public bool IsCompany { get; set; } = isCompany;
    public List<SupportTicket> TicketList { get; set; } = [];


    [JsonIgnore]
    public string CustomerType => IsCompany ? "Företag" : "Privatkund";

    public void Rename(CustomerName customerName)
    {
        CustomerName = customerName;
    }

    public void ChangeEmailAddress(EmailAddress emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
