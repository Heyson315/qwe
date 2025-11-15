using Microsoft.VisualStudio.TestTools.UnitTesting;
using qwe.Services;
using qwe.Models;
using System.Linq;

namespace qwe.Tests.Services
{
    [TestClass]
    public class ServicesServiceTests
    {
        private ServicesService _servicesService;

        [TestInitialize]
        public void Setup()
        {
            _servicesService = new ServicesService();
        }

        [TestMethod]
        public void GetAllServices_ShouldReturnServices()
        {
            // Act
            var services = _servicesService.GetAllServices();

            // Assert
            Assert.IsNotNull(services);
            Assert.IsTrue(services.Count > 0);
        }

        [TestMethod]
        public void GetAllServices_ShouldReturnTaxPreparation()
        {
            // Act
            var services = _servicesService.GetAllServices();

            // Assert
            Assert.IsTrue(services.Any(s => s.Name == "Tax Preparation"));
        }

        [TestMethod]
        public void GetServiceByName_WithValidName_ShouldReturnService()
        {
            // Arrange
            var serviceName = "Bookkeeping";

            // Act
            var service = _servicesService.GetServiceByName(serviceName);

            // Assert
            Assert.IsNotNull(service);
            Assert.AreEqual(serviceName, service.Name);
        }

        [TestMethod]
        public void GetServiceByName_WithInvalidName_ShouldReturnNull()
        {
            // Arrange
            var serviceName = "NonExistentService";

            // Act
            var service = _servicesService.GetServiceByName(serviceName);

            // Assert
            Assert.IsNull(service);
        }

        [TestMethod]
        public void SearchServices_WithKeyword_ShouldReturnMatchingServices()
        {
            // Arrange
            var keyword = "tax";

            // Act
            var services = _servicesService.SearchServices(keyword);

            // Assert
            Assert.IsNotNull(services);
            Assert.IsTrue(services.Count > 0);
            Assert.IsTrue(services.Any(s => 
                s.Name.Contains(keyword, System.StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(keyword, System.StringComparison.OrdinalIgnoreCase)));
        }

        [TestMethod]
        public void SearchServices_WithEmptyKeyword_ShouldReturnAllServices()
        {
            // Act
            var services = _servicesService.SearchServices("");

            // Assert
            Assert.IsNotNull(services);
            Assert.AreEqual(_servicesService.GetAllServices().Count, services.Count);
        }
    }
}
