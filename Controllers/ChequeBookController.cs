using Microsoft.AspNetCore.Mvc;

namespace OnlineBankingApplication.Controllers
{
    public class ChequeBookController : Controller
    {
        public IActionResult Request()
        {
            return View();
        }
    }
}
