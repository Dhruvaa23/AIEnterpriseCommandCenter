using AIEnterpriseCommandCenter.Application.DTOs.Account;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(
            IProfileRepository profileRepository,
            UserManager<ApplicationUser> userManager)
        {
            _profileRepository = profileRepository;
            _userManager = userManager;
        }

        //===========================================
        // GET
        //===========================================

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var profile = await _profileRepository.GetProfileAsync(user.Id);

            return View(profile);
        }

        //===========================================
        // POST
        //===========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileDto model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _profileRepository.UpdateProfileAsync(model);

            if (!result)
            {
                TempData["Error"] = "Unable to update profile.";

                return View(model);
            }

            TempData["Success"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}