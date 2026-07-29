using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories.Interfaces;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BillController : Controller
    {
        private readonly IBillPaymentRepo _billRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public BillController(
            IBillPaymentRepo billRepo,
            UserManager<ApplicationUser> userManager)
        {
            _billRepo = billRepo;
            _userManager = userManager;
        }

        //-----------------------------------------
        // GET : Pay Bills
        //-----------------------------------------

        [HttpGet]
        public IActionResult PayBills()
        {
            return View(new PayBillVM
            {
                DueDate = DateOnly.FromDateTime(DateTime.Today)
            });
        }

        //-----------------------------------------
        // POST : Pay Bills
        //-----------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PayBills(PayBillVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var result = await _billRepo.PayBillAsync(userId, model);

            if (!result.Success)
            {
                ModelState.AddModelError("", result.Message);
                return View(model);
            }

            TempData["Success"] = result.Message;

            return RedirectToAction(nameof(Receipt),
                new { id = result.TransactionId });
        }

        //-----------------------------------------
        // Payment History
        //-----------------------------------------

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var userId = _userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
                return Challenge();

            var history = await _billRepo.GetPaymentHistoryAsync(userId);

            return View(history);
        }

        //-----------------------------------------
        // Receipt
        //-----------------------------------------

        [HttpGet]
        public async Task<IActionResult> Receipt(long id)
        {
            var receipt = await _billRepo.GetReceiptAsync(id);

            if (receipt == null)
                return NotFound();

            return View(receipt);
        }
    }
}