using AIEnterpriseCommandCenter.Application.DTOs.User;
using AIEnterpriseCommandCenter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AIEnterpriseCommandCenter.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        //========================================
        // INDEX
        //========================================

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsync();

            return View(users);
        }

        //========================================
        // CREATE
        //========================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Roles = new SelectList(await _userRepository.GetRolesAsync());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(await _userRepository.GetRolesAsync());

                return View(model);
            }

            var result = await _userRepository.CreateAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Unable to create user.");

                ViewBag.Roles = new SelectList(await _userRepository.GetRolesAsync());

                return View(model);
            }

            TempData["Success"] = "User created successfully.";

            return RedirectToAction(nameof(Index));
        }

        //========================================
        // EDIT
        //========================================

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            ViewBag.Roles = new SelectList(
                await _userRepository.GetRolesAsync(),
                user.Role);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateUserDto model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new SelectList(
                    await _userRepository.GetRolesAsync(),
                    model.Role);

                return View(model);
            }

            var result = await _userRepository.UpdateAsync(model);

            if (!result)
            {
                ModelState.AddModelError("", "Unable to update user.");

                ViewBag.Roles = new SelectList(
                    await _userRepository.GetRolesAsync(),
                    model.Role);

                return View(model);
            }

            TempData["Success"] = "User updated successfully.";

            return RedirectToAction(nameof(Index));
        }

        //========================================
        // ACTIVATE / DEACTIVATE
        //========================================

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(string id, bool isActive)
        {
            await _userRepository.ChangeStatusAsync(id, isActive);

            return RedirectToAction(nameof(Index));
        }

        //========================================
        // RESET PASSWORD
        //========================================

        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var result = await _userRepository.ResetPasswordAsync(
                id,
                "Welcome@123");

            if (result)
                TempData["Success"] = "Password reset successfully.";

            else
                TempData["Error"] = "Password reset failed.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus(string id)
        {
            await _userRepository.ToggleStatusAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _userRepository.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}