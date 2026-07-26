using Microsoft.AspNetCore.Mvc;

namespace OnlineBankingApplication.Controllers
{
    public class TransactionController : Controller
    {
        public IActionResult TransferMoney()
        {
            return View();
        }

        public IActionResult Statement()
        {
            return View();
        }
    }
}
