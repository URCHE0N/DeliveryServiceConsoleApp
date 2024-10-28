using DeliveryService.Library.Services;

namespace DeliveryService.Library.Models
{
    public class AppConfig
    {
        public string? CityDistrict { get; set; }
        public DateTime FirstDeliveryTime { get; set; }
        public string? DeliveryLog { get; set; }
        public string? DeliveryOrder { get; set; }

        public static AppConfig LoadConfigFile(string filePath)
        {
            var config = new AppConfig();
            var lines = File.ReadAllLines(filePath);

            try
            {
                foreach (var line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var parts = line.Split('=');

                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim().ToLower();

                        switch (key)
                        {
                            case "_cityDistrict":
                                config.CityDistrict = value;
                                break;

                            case "_firstDeliveryTime":
                                if (DateTime.TryParse(value, out DateTime dateTime))
                                {
                                    config.FirstDeliveryTime = dateTime;
                                }
                                else
                                {
                                    ErrorService.ErrorNotification("Неверный формат даты!");
                                }
                                break;

                            case "_deliveryLog":
                                config.DeliveryLog = value;
                                break;

                            case "_deliveryOrder":
                                config.DeliveryOrder = value;
                                break;

                            default:
                                ErrorService.ErrorNotification($"Неизвестные параметры: {key}={value}!");
                                break;
                        }
                    }
                    else
                    {
                        ErrorService.ErrorNotification($"Ошибка в формате строки: {line}!");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                ErrorService.ErrorNotification($"Конфиг файл не найдет по пути: {filePath}!");
            }
            catch (Exception ex)
            {
                ErrorService.ErrorNotification($"Ошибка при чтении файла: {ex.Message}");
            }

            return config;
        }
    }
}
