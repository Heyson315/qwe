using qwe.Models;
using System;
using System.Collections.Generic;

namespace qwe.Services
{
    /// <summary>
    /// Service layer for managing CPA services
    /// Separates business logic from controller
    /// </summary>
    public class ServicesService
    {
        /// <summary>
        /// Get all available services
        /// </summary>
        public List<Service> GetAllServices()
        {
            // TODO: Replace with database call when implemented
            return new List<Service>
            {
                new Service
                {
                    Name = "Tax Preparation",
                    Description = "Comprehensive tax preparation services for individuals and businesses. We ensure accuracy and maximize your deductions."
                },
                new Service
                {
                    Name = "Bookkeeping",
                    Description = "Professional bookkeeping services to keep your financial records organized and up-to-date."
                },
                new Service
                {
                    Name = "Payroll Processing",
                    Description = "Efficient payroll processing services ensuring timely and accurate employee payments."
                },
                new Service
                {
                    Name = "Financial Consulting",
                    Description = "Expert financial advice to help your business grow and succeed."
                },
                new Service
                {
                    Name = "Audit Support",
                    Description = "Professional support during audits to ensure compliance and smooth process."
                },
                new Service
                {
                    Name = "Business Formation",
                    Description = "Guidance on choosing the right business structure and assistance with formation."
                }
            };
        }

        /// <summary>
        /// Get service by name
        /// </summary>
        public Service GetServiceByName(string name)
        {
            var services = GetAllServices();
            return services.Find(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Search services by keyword
        /// </summary>
        public List<Service> SearchServices(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAllServices();

            var services = GetAllServices();
            return services.FindAll(s => 
                s.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                s.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
