using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class ChequeBookController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICustomerRepo _customerRepo;
        private readonly IBankAccountRepo _bankAccountRepo;
        private readonly OnlineBankingDbContext _context;

        public ChequeBookController(
            UserManager<ApplicationUser> userManager,
            ICustomerRepo customerRepo,
            IBankAccountRepo bankAccountRepo,
            OnlineBankingDbContext context)
        {
            _userManager = userManager;
            _customerRepo = customerRepo;
            _bankAccountRepo = bankAccountRepo;
            _context = context;
        }
        [HttpGet]
        public IActionResult RequestSuccess()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Status()
        {
            var userId = _userManager.GetUserId(User);

            var customer = await _customerRepo.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return NotFound();

            var requests = await _context.ChequeBookRequests
                .Include(x => x.Account)
                .Where(x => x.Account.CustomerId == customer.CustomerId)
                .OrderByDescending(x => x.RequestDate)
                .ToListAsync();

            return View(requests);
        }
        // Show Shipping Address
        [HttpGet]
        public async Task<IActionResult> Request()
        {
            var userId = _userManager.GetUserId(User);

            var customer = await _customerRepo.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // Submit Request
        [HttpPost]
        public async Task<IActionResult> ConfirmRequest()
        {
            var userId = _userManager.GetUserId(User);

            var customer = await _customerRepo.GetCustomerByUserIdAsync(userId);

            if (customer == null)
                return NotFound();

            var account = await _bankAccountRepo.GetAccountByCustomerId(customer.CustomerId);

            if (account == null)
            {
                TempData["Error"] = "Bank account not found.";
                return RedirectToAction("Dashboard", "Customer");
            }

            ChequeBookRequest request = new ChequeBookRequest
            {
                AccountId = account.AccountId,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };


            _context.ChequeBookRequests.Add(request);

            await _context.SaveChangesAsync();

            TempData["Success"] = "Your cheque book request has been received.";

            return RedirectToAction(nameof(RequestSuccess));
        }
    }
}