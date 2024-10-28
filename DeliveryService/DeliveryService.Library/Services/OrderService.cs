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

        public static void ConfigFilterOrders(AppConfig config)
        {
            try
            {
                if (string.IsNullOrEmpty(config.CityDistrict) || config.FirstDeliveryTime == default)
                {
                    ErrorService.ErrorNotification("Ошибка в параметрах фильтрации!");
                    return;
                }

                var orders = ReadOrdersFromFile("Data/orders.txt");
                var filteredOrders = FilterOrders(orders, config.CityDistrict, config.FirstDeliveryTime);

                WriteOrdersToFile(filteredOrders, config.DeliveryOrder);
                LogService.Logger("Фильтрация завершена", config.DeliveryLog);
                Console.WriteLine("Фильтрация завершена.\n");

                Console.WriteLine("Вывести список отсортированных заказов? Отказ завершит программу... Y/n");
                string? listOrderCheck = Console.ReadLine();
                if (listOrderCheck == "Y" || listOrderCheck == "y")
                {
                    DisplayListOrder(config.DeliveryOrder);

                    Console.WriteLine("\nДля завершения, нажмите Enter...");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                ErrorService.ErrorNotification($"Ошибка: {ex.Message}");
            }
        }

        public static void DisplayListOrder(string filePath)
        {
            Console.Clear();
            Console.WriteLine("Список заказов (id,вес,район,дата):");

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorService.ErrorNotification($"Ошибка: {ex.Message}");
            }
        }
    }
}
