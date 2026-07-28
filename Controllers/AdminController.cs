using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;
using OnlineBankingApplication.ViewModels;

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

        public async Task<IActionResult> Dashboard()
        {
            var customers = await _customerRepo.GetPendingCustomersAsync();

            return View(customers);
        }
        private int GetProductId(string accountType)
        {
            return accountType switch
            {
                "Savings" => 1,
                "Current" => 2,
                "Salary" => 3,
               
                _ => 1
            };
        }

        //public async Task<IActionResult> Approve(int id)
        //{
        //    var customer = await _customerRepo.GetCustomerByIdAsync(id);

        //    if (customer == null)
        //        return NotFound();

        //    customer.Status = "Approved";

        //    string accountNumber = await _accountRepo.GenerateAccountNumber();

        //    // Generate IFSC first
        //    string ifsc = GenerateIFSC(customer.Branch ?? "");

        //    // Check whether IFSC is generated
        //    if (string.IsNullOrWhiteSpace(ifsc))
        //    {
        //        throw new Exception("IFSC Code is NULL");
        //    }

        //    BankAccount account = new BankAccount
        //    {
        //        CustomerId = customer.CustomerId,

        //        ProductId = GetProductId(customer.AccountType ?? ""),

        //        AccountNumber = accountNumber,

        //        AccountType = customer.AccountType ?? "",

        //        BranchName = customer.Branch ?? "",

        //        Ifsccode = GenerateIFSC(customer.Branch ?? ""),

        //        Balance = 0,

        //        Status = "Active",

        //        OpenedDate = DateTime.Now
        //    };
        //    await _accountRepo.CreateAccountAsync(account);

        //    await _customerRepo.SaveAsync();
        //    await _accountRepo.SaveAsync();

        //    TempData["Success"] = "Customer Approved Successfully.";

        //    return RedirectToAction(nameof(Dashboard));
        //}
        [HttpGet]
        public async Task<IActionResult> Approve(int id)
        {
            var customer = await _customerRepo.GetCustomerByIdAsync(id);

            if (customer == null)
                return NotFound();

            var vm = new ApproveCustomerVM
            {
                CustomerId = customer.CustomerId,
                CustomerName = customer.FirstName + " " + customer.LastName
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(ApproveCustomerVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var customer = await _customerRepo.GetCustomerByIdAsync(model.CustomerId);

            if (customer == null)
                return NotFound();

            customer.Status = "Approved";

            string accountNumber = await _accountRepo.GenerateAccountNumber();

            BankAccount account = new BankAccount
            {
                CustomerId = customer.CustomerId,

                ProductId = GetProductId(customer.AccountType ?? ""),

                AccountNumber = accountNumber,

                AccountType = customer.AccountType ?? "",

                BranchName = customer.Branch ?? "",

                Ifsccode = GenerateIFSC(customer.Branch ?? ""),

                Balance = model.InitialDeposit,

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
            switch (branch)
            {
                case "Delhi":
                    return "ONBK0001001";

                case "Noida":
                    return "ONBK0001002";

                case "Ghaziabad":
                    return "ONBK0001003";

                case "Greater Noida":
                    return "ONBK0001004";

                default:
                    return "ONBK0000000";
            }
        }

    }
}