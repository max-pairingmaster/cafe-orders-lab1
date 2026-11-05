using CafeOrders.Core.Domain;

namespace CafeOrders.App;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("========================================");
        Console.WriteLine("   СИСТЕМА УПРАВЛІННЯ ЗАМОВЛЕННЯМИ КАФЕ ");
        Console.WriteLine("   Лабораторна робота № 1 (Варіант 12)  ");
        Console.WriteLine("========================================");

        var customer = new Customer("Владислав Насонов", "vlad@example.com", "+380501234567");
        Console.WriteLine($"Клієнт: {customer.Name} ({customer.Email})");

        var espresso = new MenuItem("Еспресо", 45.0m, "Кава");
        var cappuccino = new MenuItem("Капучино", 65.0m, "Кава");
        var croissant = new MenuItem("Круасан з мигдалем", 75.0m, "Випічка");

        var order = new Order(customer);
        order.AddItem(espresso, 2);
        order.AddItem(cappuccino, 1);
        order.AddItem(croissant, 2);

        Console.WriteLine($"\nСтворено замовлення #{order.Id.ToString()[..8]}");
        Console.WriteLine($"Поточний статус: {order.Status}");
        Console.WriteLine("----------------------------------------");
        Console.WriteLine("Позиції замовлення:");

        foreach (var line in order.Lines)
        {
            Console.WriteLine($" • {line.Item.Name,-22} x{line.Quantity,2}  = {line.Subtotal,7:F2} грн");
        }

        decimal subtotal = order.CalculateSubtotal();
        decimal total = order.CalculateTotal(discountPercent: 5m, serviceFeePercent: 10m);

        Console.WriteLine("----------------------------------------");
        Console.WriteLine($"Базова вартість:       {subtotal,10:F2} грн");
        Console.WriteLine($"Знижка (5%):           {subtotal * 0.05m,10:F2} грн");
        Console.WriteLine($"Сервісний збір (10%):  {(subtotal * 0.95m) * 0.10m,10:F2} грн");
        Console.WriteLine($"Разом до сплати:       {total,10:F2} грн");

        order.ChangeStatus(OrderStatus.Preparing);
        Console.WriteLine($"\nНовий статус замовлення: {order.Status}");
        Console.WriteLine("========================================");
    }
}
