using System.Data.Entity.Migrations;

namespace qwe.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<qwe.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "qwe.Data.ApplicationDbContext";
        }

        protected override void Seed(qwe.Data.ApplicationDbContext context)
        {
            // Seed initial services data
            context.Services.AddOrUpdate(
                s => s.Name,
                new Models.Service
                {
                    Id = 1,
                    Name = "Tax Preparation",
                    Description = "Comprehensive tax preparation services for individuals and businesses. We ensure accuracy and maximize your deductions.",
                    IsActive = true,
                    DisplayOrder = 1
                },
                new Models.Service
                {
                    Id = 2,
                    Name = "Bookkeeping",
                    Description = "Professional bookkeeping services to keep your financial records organized and up-to-date.",
                    IsActive = true,
                    DisplayOrder = 2
                },
                new Models.Service
                {
                    Id = 3,
                    Name = "Payroll Processing",
                    Description = "Efficient payroll processing services ensuring timely and accurate employee payments.",
                    IsActive = true,
                    DisplayOrder = 3
                },
                new Models.Service
                {
                    Id = 4,
                    Name = "Financial Consulting",
                    Description = "Expert financial advice to help your business grow and succeed.",
                    IsActive = true,
                    DisplayOrder = 4
                },
                new Models.Service
                {
                    Id = 5,
                    Name = "Audit Support",
                    Description = "Professional support during audits to ensure compliance and smooth process.",
                    IsActive = true,
                    DisplayOrder = 5
                },
                new Models.Service
                {
                    Id = 6,
                    Name = "Business Formation",
                    Description = "Guidance on choosing the right business structure and assistance with formation.",
                    IsActive = true,
                    DisplayOrder = 6
                }
            );

            context.SaveChanges();
        }
    }
}
