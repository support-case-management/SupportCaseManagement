namespace SupportCaseManagement.Domain.ValueObjects;

public record CustomerName
{
    public string Value { get; }

    public CustomerName(string value, int minLength = 2)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Customer name is required.", nameof(value));
        }

        string normalizedValue = value.Trim();

        if (normalizedValue.Length < minLength)
        {
            throw new ArgumentException($"Customername must be at least {minLength}.", nameof(value));
        }

        Value = normalizedValue;
    }

    public override string ToString()
    {
        return Value;
    }
}
