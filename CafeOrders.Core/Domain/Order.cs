namespace CafeOrders.Core.Domain;

public class Order
{
    private readonly List<string[]> _lines = new();

    public Order(string id, string customerName)
    {
        Id = id;
        CustomerName = customerName;
        CreatedAt = DateTime.Now;
    }

    public string Id { get; private set; }

    public string CustomerName { get; private set; }

    public int Status { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public void AddLine(string sku, int quantity, decimal unitPrice)
    {
        string[] line = new string[3];
        line[0] = sku;
        line[1] = quantity.ToString();
        line[2] = unitPrice.ToString();
        _lines.Add(line);
    }

    public decimal CalculateTotal(bool isRegularCustomer)
    {
        decimal total = 0;
        int lineCount = 0;
        for (int i = 0; i < _lines.Count; i++)
        {
            int quantity = int.Parse(_lines[i][1]);
            decimal unitPrice = decimal.Parse(_lines[i][2]);
            total = total + quantity * unitPrice;
            lineCount = lineCount + 1;
        }

        // if (total > 500) { total = total - 50; } // стара знижка
        if (isRegularCustomer == true && total > 1000)
        {
            total = total * 0.9m;
        }
        else if (total > 5000)
        {
            total = total * 0.85m;
        }
        else
        {
            total = total;
        }

        if (lineCount > 10)
        {
            total = total - 100;
        }

        if (total < 0)
        {
            total = 0;
        }

        total = total + total * 0.2m;
        return Math.Round(total, 2);
    }

    public bool TryChangeStatus(int newStatus)
    {
        if (Status == 0 && newStatus == 1)
        {
            Status = 1;
            return true;
        }

        if (Status == 1 && newStatus == 2)
        {
            Status = 2;
            return true;
        }

        if (Status == 0 && newStatus == 3)
        {
            Status = 3;
            return true;
        }

        return false;
    }

    public bool IsValid()
    {
        if (Id != null && Id != ""
            && CustomerName != null
            && CustomerName.Length > 2
            && _lines.Count > 0
            && _lines.Count < 100
            && Status >= 0
            && Status <= 3)
        {
            return true;
        }

        return false;
    }

    public string BuildReport()
    {
        string report = "";
        for (int i = 0; i < _lines.Count; i++)
        {
            string sku = _lines[i][0];
            string qtyStr = _lines[i][1];
            string priceStr = _lines[i][2];
            decimal lineSum = int.Parse(qtyStr) * decimal.Parse(priceStr);

            report = report + "Товар: " + sku + "; кількість: " + qtyStr
                + "; ціна: " + priceStr + "; сума: " + lineSum + "\n";
        }

        report = report + "Разом: " + CalculateTotal(false) + "\n";
        return report;
    }

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
