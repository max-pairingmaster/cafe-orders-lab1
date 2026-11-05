namespace CafeOrders.Core.Domain;

public class OrderLine
{
    private MenuItem _item;
    private int _quantity;

    public Guid Id { get; init; } = Guid.NewGuid();

    public MenuItem Item
    {
        get => _item;
        set => _item = value ?? throw new ArgumentNullException(nameof(value));
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Кількість має бути більшою за 0.");
            }

            _quantity = value;
        }
    }

    public decimal Subtotal => Item.Price * Quantity;

    public OrderLine(MenuItem item, int quantity)
    {
        _item = item ?? throw new ArgumentNullException(nameof(item));
        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Кількість має бути більшою за 0.");
        }

        _quantity = quantity;
    }
}
