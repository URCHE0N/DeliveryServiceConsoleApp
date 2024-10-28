using DeliveryService.Library.Models;
using System.Globalization;

namespace DeliveryService.Library.Services
{
    public class OrderService
    {
        public static List<Order> ReadOrdersFromFile(string filePath)
        {
            var orders = new List<Order>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (Order.TryParse(line, out Order order))
                {
                    orders.Add(order);
                }
                else
                {
                    ErrorService.ErrorNotification($"Ошибка в строке: {line}");
                }
            }

            return orders;
        }

        public static List<Order> FilterOrders(List<Order> orders, string district, DateTime firstDeliveryTime)
        {
            return orders.Where(o => o.District == district && o.DeliveryTime >= firstDeliveryTime 
            && o.DeliveryTime <= firstDeliveryTime.AddMinutes(30)).ToList();
        }

        public static void WriteOrdersToFile(List<Order> orders, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var order in orders)
                {
                    writer.WriteLine($"{order.OrderId},{order.Weight.ToString(CultureInfo.InvariantCulture)}," +
                        $"{order.District},{order.DeliveryTime.ToString("yyyy-MM-dd HH:mm:ss")}");
                }
            }
        }
    }
}
