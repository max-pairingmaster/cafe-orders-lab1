namespace CafeOrders.Core.Domain;

public class Order
{
    private readonly List<OrderLine> _lines = new();
    private Customer _customer;
    private OrderStatus _status;

    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public Customer Customer
    {
        get => _customer;
        set => _customer = value ?? throw new ArgumentNullException(nameof(value));
    }

    public OrderStatus Status
    {
        get => _status;
        private set => _status = value;
    }

    public IReadOnlyList<OrderLine> Lines => _lines.AsReadOnly();

    public Order(Customer customer)
    {
        _customer = customer ?? throw new ArgumentNullException(nameof(customer));
        _status = OrderStatus.New;
    }

    public void AddLine(OrderLine line)
    {
        ArgumentNullException.ThrowIfNull(line);
        _lines.Add(line);
    }

    public void AddItem(MenuItem item, int quantity)
    {
        ArgumentNullException.ThrowIfNull(item);
        _lines.Add(new OrderLine(item, quantity));
    }

    public decimal CalculateSubtotal()
    {
        return _lines.Sum(l => l.Subtotal);
    }

    public decimal CalculateTotal(decimal discountPercent = 0m, decimal serviceFeePercent = 0m)
    {
        decimal subtotal = CalculateSubtotal();
        decimal discount = subtotal * (discountPercent / 100m);
        decimal serviceFee = (subtotal - discount) * (serviceFeePercent / 100m);
        return (subtotal - discount) + serviceFee;
    }

    public bool ChangeStatus(OrderStatus newStatus)
    {
        if (_status == OrderStatus.Cancelled || _status == OrderStatus.Paid)
        {
            return false;
        }

        _status = newStatus;
        return true;
    }
}
