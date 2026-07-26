using Microsoft.AspNetCore.Mvc;

namespace OnlineBankingApplication.Controllers
{
    public class BillController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
