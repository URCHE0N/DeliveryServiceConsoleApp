namespace DeliveryService.Library.Services
{
    public class LogService
    {
        public static void Logger(string message, string filePath)
        {
            using (var writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}: {message}");
            }
        }
    }
}
