using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepo _repo;

        public CustomerController(ICustomerRepo repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Dashboard()
        {
            string email = User.Identity.Name;

            var model = await _repo.GetDashboard(email);

            return View(model);
        }
    }
}