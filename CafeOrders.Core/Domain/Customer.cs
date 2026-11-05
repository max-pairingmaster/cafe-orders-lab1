namespace CafeOrders.Core.Domain;

public class Customer
{
    private string _name;
    private string _email;
    private string _phone;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Email
    {
        get => _email;
        set => _email = value ?? string.Empty;
    }

    public string Phone
    {
        get => _phone;
        set => _phone = value ?? string.Empty;
    }

    public int LoyaltyPoints { get; set; }

    public Customer(string name, string email = "", string phone = "")
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _email = email ?? string.Empty;
        _phone = phone ?? string.Empty;
        LoyaltyPoints = 0;
    }
}
