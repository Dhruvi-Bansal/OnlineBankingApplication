using Microsoft.AspNetCore.Mvc;

namespace OnlineBankingApplication.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
