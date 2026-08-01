using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;

namespace OnlineBankingApplication.Controllers
{

    [Authorize(Roles = "Customer")]
    public class ProfileController : Controller
    {
        private readonly IProfileRepo _repo;

        public ProfileController(IProfileRepo repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            string email = User.Identity!.Name!;

            var profile = await _repo.GetProfileAsync(email);

            if (profile == null)
                return RedirectToAction("Login", "Account");

            return View(profile);
        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            string email = User.Identity!.Name!;

            var profile = await _repo.GetProfileAsync(email);

            if (profile == null)
                return RedirectToAction("Login", "Account");

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ViewModels.ProfileVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            string email = User.Identity!.Name!;

            var profile = await _repo.GetProfileAsync(email);

            if (profile == null)
                return RedirectToAction("Login", "Account");


            bool pending = await _repo.HasPendingRequestAsync(profile.CustomerId);

            if (pending)
            {
                TempData["Error"] =
                    "A profile update request is already pending approval.";

                return RedirectToAction(nameof(Index));
            }

            ProfileUpdateRequest request = new ProfileUpdateRequest
            {
                CustomerId = profile.CustomerId,

                NewPhone = model.Phone,

                NewAddress = model.Address,

                RequestDate = DateTime.Now,

                Status = "Pending"
            };

            await _repo.SubmitRequestAsync(request);

            await _repo.SaveAsync();

            TempData["Success"] =
                "Your profile update request has been sent to the Admin.";

            return RedirectToAction(nameof(Index));
        }
    }
}