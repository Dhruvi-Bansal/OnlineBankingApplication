using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Repositories;
using OnlineBankingApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using OnlineBankingApplication.Models;
namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepo _transactionRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransactionController(
            ITransactionRepo transactionRepo,
            UserManager<ApplicationUser> userManager)
        {
            _transactionRepo = transactionRepo;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Transfer()
        {
            return View(new TransferMoneyVM());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Transfer(TransferMoneyVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Challenge();

            bool passwordCorrect =
                await _userManager.CheckPasswordAsync(user, vm.Password);

            if (!passwordCorrect)
            {
                ModelState.AddModelError("Password", "Incorrect password.");
                return View(vm);
            }

            var userId = user.Id;

            bool success = await _transactionRepo.TransferMoney(
                userId,
                vm.ReceiverAccountNumber,
                vm.Amount,
                vm.Description);

            if (success)
            {
                TempData["Success"] = "Fund transferred successfully.";
                return RedirectToAction(nameof(History));
            }

            ModelState.AddModelError("",
                "Transfer failed. Receiver account does not exist or insufficient balance.");

            return View(vm);
        }
        public async Task<IActionResult> History() 
        { 
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var transactions = await _transactionRepo.GetTransactions(userId!); 
            ViewBag.CurrentAccountId = await _transactionRepo.GetCurrentAccountId(userId!); 
            return View(transactions); 
        }

        public async Task<IActionResult> Receipt(long id)
        {
            var transaction =
                await _transactionRepo.GetTransaction(id);

            if (transaction == null)
                return NotFound();

            return View(transaction);
        }
    }
}