using DeliveryService.Library.Models;

namespace DeliveryService.Library.Services
{
    public class ErrorService
    {
        public static string ErrorNotification(string message)
        {
            LogService.Logger(message, "Logs/log.txt");
            Console.WriteLine($"{message}\n");
            throw new FormatException(message);
        }
    }
}
