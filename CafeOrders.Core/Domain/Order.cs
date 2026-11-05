namespace CafeOrders.Core.Domain;

/// <summary>
/// Замовлення в кафе.
/// </summary>
public class Order
{
    /// <summary>Ставка ПДВ.</summary>
    private const decimal VatRate = 0.2m;

    /// <summary>
    /// Поріг суми постійного клієнта.
    /// </summary>
    private const decimal RegularDiscountThreshold = 1000m;

    /// <summary>
    /// Частка знижки клієнта.
    /// </summary>
    private const decimal RegularDiscountRate = 0.10m;

    /// <summary>
    /// Поріг знижки на опт.
    /// </summary>
    private const decimal LargeOrderThreshold = 5000m;

    /// <summary>
    /// Частка знижки на опт.
    /// </summary>
    private const decimal LargeOrderDiscountRate = 0.15m;

    /// <summary>
    /// Кількість для знижки.
    /// </summary>
    private const int BulkLineCount = 10;

    /// <summary>Сума знижки, грн.</summary>
    private const decimal BulkDiscountAmount = 100m;

    /// <summary>
    /// Межа кількості рядків.
    /// </summary>
    private const int MaxLineCount = 100;

    /// <summary>Мін. довжина імені.</summary>
    private const int MinCustomerNameLength = 3;

    /// <summary>Стан: новий.</summary>
    public const int StatusNew = 0;

    /// <summary>Стан: оплачений.</summary>
    public const int StatusPaid = 1;

    /// <summary>Стан: відправлений.</summary>
    public const int StatusSent = 2;

    /// <summary>Стан: скасований.</summary>
    public const int StatusCancelled = 3;

    private readonly List<string[]> _lines = new();

    /// <summary>
    /// Ініціалізує замовлення.
    /// </summary>
    /// <param name="id">Код замовлення.</param>
    /// <param name="customerName">Ім'я клієнта.</param>
    public Order(string id, string customerName)
    {
        Id = id;
        CustomerName = customerName;
        CreatedAt = DateTime.Now;
    }

    /// <summary>
    /// Ідентифікатор.
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// Ім'я клієнта.
    /// </summary>
    public string CustomerName { get; private set; }

    /// <summary>
    /// Код стану.
    /// </summary>
    public int Status { get; private set; } = StatusNew;

    /// <summary>
    /// Дата створення.
    /// </summary>
    public DateTime CreatedAt { get; private set; }

    /// <summary>
    /// Додає позицію до чеку.
    /// </summary>
    /// <param name="sku">Артикул.</param>
    /// <param name="quantity">Кількість.</param>
    /// <param name="unitPrice">Ціна за од.</param>
    public void AddLine(string sku, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = sku;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();
        _lines.Add(line);
    }

    /// <summary>
    /// Обчислює суму до сплати.
    /// </summary>
    /// <param name="isRegularCustomer">Постійний клієнт.</param>
    /// <returns>Підсумок до сплати.</returns>
    public decimal CalculateTotal(bool isRegularCustomer)
    {
        decimal total = 0;
        int lineCount = 0;
        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(_lines[i][1]);
            decimal unitPrice = decimal.Parse(_lines[i][2]);
            total += quantity * unitPrice;
            lineCount++;
        }

        // Порядок знижок: постійна, потім оптова
        if (isRegularCustomer && total > RegularDiscountThreshold)
        {
            total *= 1m - RegularDiscountRate;
        }
        else if (total > LargeOrderThreshold)
        {
            total *= 1m - LargeOrderDiscountRate;
        }

        if (lineCount > BulkLineCount)
        {
            total -= BulkDiscountAmount;
        }

        if (total < 0)
        {
            total = 0;
        }

        // ПДВ після врахування знижок
        total += total * VatRate;
        return Math.Round(total, 2);
    }

    /// <summary>
    /// Змінює стан замовлення.
    /// </summary>
    /// <param name="newStatus">Цільовий стан.</param>
    /// <returns>
    /// true, якщо успішно; false, якщо заборонено.
    /// </returns>
    public bool TryChangeStatus(int newStatus)
    {
        if (Status == StatusNew && newStatus == StatusPaid)
        {
            Status = StatusPaid;
            return true;
        }

        if (Status == StatusPaid && newStatus == StatusSent)
        {
            Status = StatusSent;
            return true;
        }

        if (Status == StatusNew && newStatus == StatusCancelled)
        {
            Status = StatusCancelled;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Перевіряє замовлення.
    /// </summary>
    /// <returns>true, якщо валідне.</returns>
    public bool IsValid()
    {
        if (Id != null && Id.Length > 0
            && CustomerName != null
            && CustomerName.Length >= MinCustomerNameLength
            && _lines.Count > 0
            && _lines.Count < MaxLineCount
            && Status >= StatusNew
            && Status <= StatusCancelled)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Формує звіт замовлення.
    /// </summary>
    /// <returns>Рядок звіту.</returns>
    public string BuildReport()
    {
        string report = "";
        for (int i = 0; i < _lines.Count; i++)
        {
            string sku = _lines[i][0];
            string qtyStr = _lines[i][1];
            string priceStr = _lines[i][2];
            decimal lineSum = int.Parse(qtyStr) * decimal.Parse(priceStr);

            report += "Товар: " + sku + "; кількість: " + qtyStr
                + "; ціна: " + priceStr + "; сума: " + lineSum + "\n";
        }

        report += "Разом: " + CalculateTotal(false) + "\n";
        return report;
    }

    /// <summary>
    /// Шукає замовлення за ID.
    /// </summary>
    /// <param name="orders">Список замовлень.</param>
    /// <param name="orderId">Код замовлення.</param>
    /// <returns>Знайдене замовлення або null.</returns>
    public static Order? FindById(List<Order> orders, string orderId)
    {
        for (int i = 0; i < orders.Count; i++)
        {
            if (orders[i].Id == orderId)
            {
                return orders[i];
            }
        }

        return null;
    }
}
