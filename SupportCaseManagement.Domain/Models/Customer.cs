using SupportCaseManagement.Domain.ValueObjects;
using System.Text.Json.Serialization;

namespace SupportCaseManagement.Domain.Models;

public class Customer(Guid customerId, CustomerName customerName, EmailAddress emailAddress, bool isCompany)
{
    public Guid CustomerId { get; set; } = customerId;
    public CustomerName CustomerName { get; set; } = customerName;
    public EmailAddress EmailAddress { get; set; } = emailAddress;
    public bool IsCompany { get; set; } = isCompany;
    
    // lista av ärenden ska vara med också 


    [JsonIgnore]
    public string CustomerType => IsCompany ? "Företag" : "Privatkund";

    public void SetId(Guid id)
    {
        CustomerId = id;
    }
    public void Rename(CustomerName customerName)
    {
        CustomerName = customerName;
    }

    public void ChangeEmailAddress(EmailAddress emailAddress)
    {
        EmailAddress = emailAddress;
    }
}
