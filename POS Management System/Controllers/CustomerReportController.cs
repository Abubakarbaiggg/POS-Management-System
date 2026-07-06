using Microsoft.AspNetCore.Mvc;

namespace POS_Management_System.Controllers
{
    public class CustomerReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
