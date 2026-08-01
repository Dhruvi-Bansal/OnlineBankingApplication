using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineBankingApplication.Models;
using OnlineBankingApplication.Repositories;
using OnlineBankingApplication.ViewModels;

namespace OnlineBankingApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ICustomerRepo _customerRepo;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ICustomerRepo customerRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(model);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Phone
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");

                Customer customer = new Customer
                {
                    UserId = user.Id,

                    FirstName = model.FirstName,

                    LastName = model.LastName,

                    Dob = DateOnly.FromDateTime(model.DOB),

                    Gender = model.Gender,

                    Phone = model.Phone,

                    Email = model.Email,

                    AadhaarNumber = model.AadhaarNumber,

                    Pannumber = model.Pannumber,

                    Address = model.Address,

                    City = model.City,

                    State = model.State,

                    Pincode = model.Pincode,

                    AccountType = model.AccountType,

                    Branch = model.Branch,

                    Status = "Pending",

                    CreatedAt = DateTime.Now
                };

                await _customerRepo.AddCustomerAsync(customer);

                await _customerRepo.SaveAsync();

                TempData["Success"] =
                    "Registration successful. Your account is pending administrator approval.";

                return RedirectToAction(nameof(Login));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Find Identity User
            var identityUser = await _userManager.FindByEmailAsync(model.Email);

            if (identityUser == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password.");
                return View(model);
            }

            bool isAdmin = await _userManager.IsInRoleAsync(identityUser, "Admin");

            if (!isAdmin)
            {
                var customer =
                    await _customerRepo.GetCustomerByEmailAsync(model.Email);

                if (customer == null)
                {
                    ModelState.AddModelError("", "Customer record not found.");
                    return View(model);
                }

                if (customer.Status == "Pending")
                {
                    ModelState.AddModelError("",
                        "Your registration is pending admin approval.");

                    return View(model);
                }

                if (customer.Status == "Rejected")
                {
                    ModelState.AddModelError("",
                        "Your registration has been rejected.");

                    return View(model);
                }
            }

            // Customer Login
            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                false,
                lockoutOnFailure: true);

            if (result.Succeeded)
            {
                if (isAdmin)
                    return RedirectToAction("Dashboard", "Admin");

                return RedirectToAction("Dashboard", "Customer");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account locked. Try again later.");
                return View(model);
            }

            ModelState.AddModelError("", "Invalid Email or Password.");

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
        // Admin Login

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Email or Password.");
                return View(model);
            }

            bool isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

            if (!isAdmin)
            {
                ModelState.AddModelError("", "You are not an administrator.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                false,
                true);

            if (result.Succeeded)
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            ModelState.AddModelError("", "Invalid Email or Password.");

            return View(model);
        }
    }
}