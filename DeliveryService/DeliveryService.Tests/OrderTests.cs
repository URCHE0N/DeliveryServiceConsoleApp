using DeliveryService.Library.Models;

namespace DeliveryService.Tests
{
    public class OrderTests
    {
        [Test]
        public void TryParse_Valid_Line_Returns_Order_True()
        {
            string line = "1,10.5,Центр,2024-10-20 12:00:00";
            int orderId = 1;
            double weight = 10.5;
            string district = "центр";

            Order.TryParse(line, out Order order);

            Assert.That(orderId, Is.EqualTo(order.OrderId));
            Assert.That(weight, Is.EqualTo(order.Weight));
            Assert.That(district, Is.EqualTo(order.District));
            Assert.That(new DateTime(2024, 10, 20, 12, 0, 0), Is.EqualTo(order.DeliveryTime));
        }

        [Test]
        public void TryParse_Invalid_Length_Line_Returns_FormatException_True()
        {
            Assert.Throws<FormatException>(() => Order.TryParse("1,5.5,Центр,2024-10-20 12:00:00,123,wqe", out Order order));
        }

        [Test]
        public void TryParse_Invalid_OrderId_Returns_FormatException_True()
        {
            Assert.Throws<FormatException>(() => Order.TryParse(",5.5,Центр,2024-10-20 12:00:00", out Order order));
        }

        [Test]
        public void TryParse_Invalid_Weight_Returns_FormatException_True()
        {
            Assert.Throws<FormatException>(() => Order.TryParse("2,текст,Центр,2024-10-20 12:00:00", out Order order));
        }

        [Test]
        public void TryParse_Invalid_District_Returns_FormatException_True()
        {
            Assert.Throws<FormatException>(() => Order.TryParse("3,2.2,,2024-10-20 12:00:00", out Order order));
        }

        [Test]
        public void TryParse_Invalid_DeliveryTime_Returns_FormatException_True()
        {
            Assert.Throws<FormatException>(() => Order.TryParse("3,2.2,Юг,2024.10.20 14:00", out Order order));
        }
    }
}