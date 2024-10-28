using DeliveryService.Library.Models;
using DeliveryService.Library.Services;

namespace DeliveryService.Tests
{
    public class OrderServiceTests
    {
        [Test]
        public void ReadOrdersFromFile_Valid_File_Returns_Orders_True()
        {
            string testFilePath = "testFile.txt";
            var testData = new List<string>
            {
                "1,2.5,Центр,2024-10-15 12:00:00",
                "2,5,Центр,2024-10-15 12:10:00",
                "3,1.2,Юг,2024-10-15 12:25:00",
            };
            int count = 3;
            int orderId = 2;
            double weight = 1.2;

            File.WriteAllLines(testFilePath, testData);

            try
            {
                var orders = OrderService.ReadOrdersFromFile(testFilePath);

                Assert.That(count, Is.EqualTo(orders.Count));
                Assert.That(orderId, Is.EqualTo(orders[1].OrderId));
                Assert.That(weight, Is.EqualTo(orders[2].Weight));
            }
            finally
            {
                if (File.Exists(testFilePath))
                {
                    File.Delete(testFilePath);
                }
            }
        }

        [Test]
        public void ReadOrdersFromFile_Returns_FileNotFoundException_True()
        {
            var testFilePath = "testFile.txt";

            Assert.Throws<FileNotFoundException>(() => OrderService.ReadOrdersFromFile(testFilePath));
        }

        [Test]
        public void FilterOrders_Valid_Criteria_Returns_Filtered_Orders_True()
        {
            int count = 2;
            string cityDistrict = "Центр";
            DateTime firstDeliveryTime = new DateTime(2024, 10, 15, 12, 0, 0);
            var orders = new List<Order>
            {
                new Order { OrderId = 1, Weight = 2.5, District = "Центр", DeliveryTime = new DateTime(2024, 10, 15, 12, 0, 0) },
                new Order { OrderId = 2, Weight = 5, District = "Центр", DeliveryTime = new DateTime(2024, 10, 15, 12, 10, 0) },
                new Order { OrderId = 3, Weight = 1.2, District = "Юг", DeliveryTime = new DateTime(2024, 10, 15, 12, 25, 0) },
            };

            var filteredOrders = OrderService.FilterOrders(orders, cityDistrict, firstDeliveryTime);

            Assert.That(count, Is.EqualTo(filteredOrders.Count));
            Assert.That(filteredOrders.Exists(o => o.OrderId == 1));
            Assert.That(filteredOrders.Exists(o => o.OrderId == 2));
            Assert.That(!filteredOrders.Exists(o => o.OrderId == 3));
        }
    }
}