using AIEnterpriseCommandCenter.Application.DTOs.Account;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //====================================================
        // Get Profile
        //====================================================

        public async Task<ProfileDto?> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new ProfileDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeCode = user.EmployeeCode,
                Email = user.Email ?? "",
                Department = user.Department,
                Designation = user.Designation,
                Role = roles.FirstOrDefault() ?? "",
                IsActive = user.IsActive,
                CreatedOn = user.CreatedOn,
                ProfilePicture = user.ProfilePicture
            };
        }

        //====================================================
        // Update Profile
        //====================================================

        public async Task<bool> UpdateProfileAsync(ProfileDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Department = model.Department;
            user.Designation = model.Designation;

            // EmployeeCode, Email, Role NOT Editable

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}