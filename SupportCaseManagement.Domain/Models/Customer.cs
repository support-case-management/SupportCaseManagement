using System.Net.Mail;
using System.Text.Json.Serialization;

namespace SupportCaseManagement.Domain.Models;

public class Customer
{
    private static readonly int _minNameLength = 2;
    private readonly List<SupportTicket> _supportTickets;

    public Guid CustomerId { get; init; }
    public string CustomerName { get; private set; }
    public string EmailAddress { get; private set; }
    public bool IsCompany { get; private set; }
    public IReadOnlyList<SupportTicket> SupportTickets => _supportTickets.AsReadOnly();

    [JsonIgnore]
    public string CustomerType => IsCompany ? "Företag" : "Privatkund";


    [JsonConstructor]
    public Customer(Guid customerId, string customerName, string emailAddress, bool isCompany, IReadOnlyList<SupportTicket> supportTickets)
    {
        CustomerId = customerId;
        CustomerName = customerName;
        EmailAddress = emailAddress;
        IsCompany = isCompany;
        _supportTickets = supportTickets.ToList() ?? [];
    }

    public Customer(Guid customerId, string customerName, string emailAddress, bool isCompany)
    {
        CustomerId = ValidateCustomerId(customerId);
        CustomerName = ValidateCustomerName(customerName);
        EmailAddress = ValidateEmailAddress(emailAddress);
        IsCompany = isCompany;
        _supportTickets = [];
    }


    private static Guid ValidateCustomerId(Guid customerId)
    {
        if (customerId == Guid.Empty)
            throw new ArgumentException("Kund-Id är ett obligatoriskt fält");

        return customerId;
    }

    private static string ValidateCustomerName(string customerName)
    {
        if (string.IsNullOrWhiteSpace(customerName))
            throw new ArgumentException("Kundnamn är ett obligatoriskt fält");

        string customerNameTrimmed = customerName.Trim();

        if (customerNameTrimmed.Length < _minNameLength)
            throw new ArgumentException($"Kundens namn måsta vara minst {_minNameLength} bokstäver långt");

        return customerNameTrimmed;
    }

    private static string ValidateEmailAddress(string customerEmail)
    {
        if (string.IsNullOrWhiteSpace(customerEmail))
            throw new ArgumentException("E-postadress är ett obligatoriskt fält");

        string normalizedEmail = customerEmail.Trim().ToLower();

        if (!MailAddress.TryCreate(normalizedEmail, out var emailAddress) || !string.Equals(emailAddress.Address, normalizedEmail, StringComparison.Ordinal))
            throw new ArgumentException("E-postadressen måste vara en giltig e-postadress");

        return normalizedEmail;
    }

    public void Rename(string customerName)
    {
        CustomerName = ValidateCustomerName(customerName);
    }

    public void ChangeEmailAddress(string emailAddress)
    {
        EmailAddress = ValidateEmailAddress(emailAddress);
    }

    public void ChangeIsCompany(bool isCompany)
    {
        IsCompany = isCompany;
    }

    public void AddSupportTicket(SupportTicket supportTicket)
    {
        if (supportTicket is not null)
        {
            _supportTickets.Add(supportTicket);
        }
    }
}
