namespace CafeOrders.Core.Domain;

public class MenuItem
{
    private string _name;
    private decimal _price;
    private string _category;

    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    {
        get => _name;
        set => _name = value ?? throw new ArgumentNullException(nameof(value));
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Ціна не може бути від'ємною.");
            }

            _price = value;
        }
    }

    public string Category
    {
        get => _category;
        set => _category = value ?? string.Empty;
    }

    public bool IsAvailable { get; set; } = true;

    public MenuItem(string name, decimal price, string category = "Загальне")
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Ціна не може бути від'ємною.");
        }

        _price = price;
        _category = category ?? string.Empty;
    }
}
