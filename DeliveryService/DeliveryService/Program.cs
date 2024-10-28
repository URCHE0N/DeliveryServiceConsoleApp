using DeliveryService.Library.Models;
using DeliveryService.Library.Services;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Консольное приложение для службы доставки\n");

        Console.WriteLine("Вывести список не отсортированных заказов? Y/n");
        string? listOrderCheck = Console.ReadLine();
        if (listOrderCheck == "Y" || listOrderCheck == "y")
        {
            OrderService.DisplayListOrder("Data/orders.txt");

            Console.WriteLine("\nДля перехода дальше, нажмите Enter...");
            Console.ReadLine();
        }
        Console.Clear();

        Console.WriteLine("Выберите пункт (введите только цифру):");
        Console.WriteLine("1) Провести фильтрацию заказов через конфиг файл");
        Console.WriteLine("2) Ввести данные для фильтрации заказов");

        string? selectTypeFilter = Console.ReadLine();
        var config = AppConfig.LoadConfigFile("Config/config.txt");
        Console.Clear();

        switch (selectTypeFilter)
        {
            case "1":
                Console.WriteLine("Фильтрация заказов через параметры в конфиг файле.\n");
                LogService.Logger($"Фильтрация по параметрам из конфиг файла. Район: {config.CityDistrict}. " +
                    $"Дата и время: c {config.FirstDeliveryTime} по {config.FirstDeliveryTime.AddMinutes(30)}", config.DeliveryLog);
                OrderService.ConfigFilterOrders(config);
                break;

            case "2":
                Console.WriteLine("Фильтрация заказов через вручную введенные параметры.\n");
                bool checkEmptyLine = false;
                bool isValidDate = false;
                string formatDate = "yyyy-MM-dd HH:mm:ss";
                string? input = "";

                Console.WriteLine("Введите район (например: центр, юг, север, восток, запад):");
                while (!checkEmptyLine)
                {
                    input = Console.ReadLine();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Ничего не было введено");
                    }
                    else
                    {
                        checkEmptyLine = true;
                        config.CityDistrict = input.ToLower();
                        Console.WriteLine("");
                    }
                }

                Console.WriteLine($"Введите дату и время первого заказа (формат: {formatDate}):");
                while (!isValidDate)
                {
                    input = Console.ReadLine();

                    isValidDate = DateTime.TryParseExact(input, formatDate,
                        CultureInfo.InvariantCulture, DateTimeStyles.None,
                        out DateTime firstDeliveryTime);

                    if (!isValidDate)
                    {
                        Console.WriteLine("Некорректная дата");
                    }
                    else
                    {
                        config.FirstDeliveryTime = firstDeliveryTime;
                        Console.WriteLine("");
                    }
                }
                LogService.Logger($"Фильтрация по введенным параметрам. Район: {config.CityDistrict}. " +
                    $"Дата и время: c {config.FirstDeliveryTime} по {config.FirstDeliveryTime.AddMinutes(30)}", config.DeliveryLog);
                OrderService.ConfigFilterOrders(config);
                break;

            default:
                Console.WriteLine("Ничего не было введено или другие значения. По стандарту: фильтрация заказов через параметры в конфиг файле.\n");
                LogService.Logger($"Фильтрация по параметрам из конфиг файла. Район: {config.CityDistrict}. " +
                    $"Дата и время: c {config.FirstDeliveryTime} по {config.FirstDeliveryTime.AddMinutes(30)}", config.DeliveryLog);
                OrderService.ConfigFilterOrders(config);
                break;
        };
    }
}