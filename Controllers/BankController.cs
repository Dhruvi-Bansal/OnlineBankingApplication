using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.DAL;

namespace OnlineBankingApplication.Controllers
{
    public class BankController : Controller
    {
        private readonly OnlineBankingDbContext _context;

        public BankController(OnlineBankingDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
