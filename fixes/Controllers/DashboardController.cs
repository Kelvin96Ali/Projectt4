using fixes.Data;
using Microsoft.AspNetCore.Mvc;

namespace fixes.Controllers
{
    public class DashboardController : BaseController
    {

        // Constructor to initialize the DbContext
        public DashboardController(ApplicationDbContext context)
         : base(context) {}

        public IActionResult index()
        {
            return View();
        }

    
    
    }

}
