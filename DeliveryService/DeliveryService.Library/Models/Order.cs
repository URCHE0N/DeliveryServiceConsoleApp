using DeliveryService.Library.Services;
using System.Globalization;

namespace DeliveryService.Library.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public double Weight { get; set; }
        public required string District { get; set; }
        public DateTime DeliveryTime { get; set; }

        public static bool TryParse(string input, out Order order)
        {
            order = null;
            var parts = input.Split(',');

            if (parts.Length != 4)
            {
                ErrorService.ErrorNotification("Неверно указано форма заказа. Элементов меньше или больше 4!");
                return false;
            }

            if (!int.TryParse(parts[0], out int orderId))
            {
                ErrorService.ErrorNotification("Неверно указан номер заказа(id)!");
                return false;
            }

            if (!double.TryParse(parts[1], CultureInfo.InvariantCulture, out double weight))
            {
                ErrorService.ErrorNotification("Неверно указан вес заказа!");
                return false;
            }

            if (parts[2].Length == 0)
            {
                ErrorService.ErrorNotification("Не указан район!");
                return false;
            }

            if (!DateTime.TryParseExact(parts[3], "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture, DateTimeStyles.None, 
                out DateTime deliveryTime))
            {
                ErrorService.ErrorNotification("Неверно указан формат даты и времени!");
                return false;
            }

            order = new Order()
            {
                OrderId = orderId,
                Weight = weight,
                District = parts[2].ToLower(),
                DeliveryTime = deliveryTime
            };

            return true;
        }
    }
}
