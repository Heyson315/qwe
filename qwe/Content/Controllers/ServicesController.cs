using qwe.Models;
using System.Collections.Generic;
using System.Web.Http;

namespace qwe.Controllers
{
    public class ServicesController : ApiController
    {
        // GET: api/Services
        public IEnumerable<Service> Get()
        {
            return new List<Service>
            {
                new Service { Name = "Tax Preparation", Description = "Individual and business tax preparation." },
                new Service { Name = "Bookkeeping", Description = "Comprehensive bookkeeping services." },
                new Service { Name = "Payroll", Description = "Full-service payroll processing." },
                new Service { Name = "Financial Consulting", Description = "Expert financial consulting and advisory." }
            };
        }
    }
}
