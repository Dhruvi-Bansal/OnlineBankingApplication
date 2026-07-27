using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICustomerRepo _customerRepo;
        private readonly IBankAccountRepo _accountRepo;

        public AdminController(
            ICustomerRepo customerRepo,
            IBankAccountRepo accountRepo)
        {
            _customerRepo = customerRepo;
            _accountRepo = accountRepo;
        }

        public IActionResult Dashboard()
        {
            return View(new List<Customer>());
        }

        public async Task<IActionResult> Approve(int id)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            customer.Status = "Approved";

            string accountNumber = await _accountRepo.GenerateAccountNumber();

            BankAccount account = new BankAccount
            {
                CustomerId = customer.CustomerId,
                AccountNumber = accountNumber,
                AccountType = customer.AccountType ?? "",
                BranchName = customer.Branch ?? "",
               Ifsccode = GenerateIFSC(customer.Branch ?? ""),
                Balance = 0,
                Status = "Active",
                OpenedDate = DateTime.Now
            };

            await _accountRepo.CreateAccountAsync(account);

            await _customerRepo.SaveAsync();
            await _accountRepo.SaveAsync();

            TempData["Success"] = "Customer Approved Successfully.";

            return RedirectToAction(nameof(Dashboard));
        }
        public async Task<IActionResult> Reject(int id)
        {
            await _customerRepo.RejectCustomerAsync(id);

            await _customerRepo.SaveAsync();

            TempData["Success"] = "Customer Rejected.";

            return RedirectToAction(nameof(Dashboard));
        }

        private string GenerateIFSC(string branch)
        {
            return branch switch
            {
                "Delhi" => "ONBK0001001",
                "Noida" => "ONBK0001002",
                "Ghaziabad" => "ONBK0001003",
                "Greater Noida" => "ONBK0001004",
                _ => "ONBK0000000"


            };
        }

    }
}