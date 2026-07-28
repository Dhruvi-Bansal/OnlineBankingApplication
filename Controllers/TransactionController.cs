using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Repositories;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class TransactionController : Controller
    {
        private readonly ITransactionRepo _transactionRepo;

        public TransactionController(ITransactionRepo transactionRepo)
        {
            _transactionRepo = transactionRepo;
        }

        //---------------------------------------------------------
        // GET : Transfer
        //---------------------------------------------------------

        [HttpGet]
        public IActionResult Transfer()
        {
            return View(new TransferMoneyVM());
        }



        //---------------------------------------------------------
        // POST : Transfer
        //---------------------------------------------------------

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer(
            TransferMoneyVM vm)
        {

            if (!ModelState.IsValid)
                return View(vm);



            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);



            bool success =
                await _transactionRepo.TransferMoney(
                    userId!,
                    vm.ReceiverAccountNumber,
                    vm.Amount,
                    vm.Description);



            if (success)
            {
                TempData["Success"] =
                    "Fund transferred successfully.";

                return RedirectToAction(nameof(History));
            }



            ModelState.AddModelError("",
                "Transfer failed. Receiver account does not exist or insufficient balance.");


            return View(vm);
        }

        //---------------------------------------------------------
        // Transaction History
        //---------------------------------------------------------

        public async Task<IActionResult> History()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactions =
                await _transactionRepo.GetTransactions(userId!);

            return View(transactions);
        }

        //---------------------------------------------------------
        // Receipt
        //---------------------------------------------------------

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