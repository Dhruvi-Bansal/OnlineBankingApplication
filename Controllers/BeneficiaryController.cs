using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.DAL;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryRepo _repo;
        private readonly OnlineBankingDbContext _context;

        public BeneficiaryController(
            IBeneficiaryRepo repo,
            OnlineBankingDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            string email = User.Identity!.Name!;

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            var list = await _repo.GetBeneficiaries(customer.CustomerId);

            return View(list);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Beneficiary beneficiary)
        {
            if (!ModelState.IsValid)
                return View(beneficiary);

            string email = User.Identity!.Name!;

            var customer = _context.Customers
                .FirstOrDefault(x => x.Email == email);

            if (customer == null)
                return RedirectToAction("Login", "Account");

            string? validation = await _repo.ValidateBeneficiary(
                customer.CustomerId,
                beneficiary.AccountNumber,
                beneficiary.Ifsccode);

            if (validation != null)
            {
                ModelState.AddModelError("", validation);

                return View(beneficiary);
            }

            beneficiary.CustomerId = customer.CustomerId;

            beneficiary.AddedDate = DateTime.Now;

            await _repo.Add(beneficiary);

            await _repo.Save();

            TempData["Success"] = "Beneficiary added successfully.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ben = await _repo.GetById(id);

            return View(ben);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Beneficiary beneficiary)
        {
            await _repo.Update(beneficiary);
            await _repo.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ben = await _repo.GetById(id);

            return View(ben);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.Delete(id);
            await _repo.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}