using AIEnterpriseCommandCenter.Application.DTOs.User;
using AIEnterpriseCommandCenter.Application.Interfaces;
using AIEnterpriseCommandCenter.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AIEnterpriseCommandCenter.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRepository(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //====================================================
        // Get All Users
        //====================================================

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _userManager.Users
                .OrderBy(x => x.FirstName)
                .ToListAsync();

            var list = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                list.Add(new UserDto
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
                    CreatedOn = user.CreatedOn
                });
            }

            return list;
        }

        //====================================================
        // Get User By Id
        //====================================================

        public async Task<UpdateUserDto?> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UpdateUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                EmployeeCode = user.EmployeeCode,
                Email = user.Email ?? "",
                Department = user.Department,
                Designation = user.Designation,
                Role = roles.FirstOrDefault() ?? "",
                IsActive = user.IsActive
            };
        }

        //====================================================
        // Create User
        //====================================================

        public async Task<bool> CreateAsync(CreateUserDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return false;

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                EmployeeCode = model.EmployeeCode,
                Department = model.Department,
                Designation = model.Designation,
                EmailConfirmed = true,
                IsActive = true,
                CreatedOn = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return false;

            if (!string.IsNullOrEmpty(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            return true;
        }

        //====================================================
        // Update User
        //====================================================

        public async Task<bool> UpdateAsync(UpdateUserDto model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
                return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.EmployeeCode = model.EmployeeCode;
            user.Email = model.Email;
            user.UserName = model.Email;
            user.Department = model.Department;
            user.Designation = model.Designation;
            user.IsActive = model.IsActive;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                return false;

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Any())
                await _userManager.RemoveFromRolesAsync(user, roles);

            if (!string.IsNullOrEmpty(model.Role))
                await _userManager.AddToRoleAsync(user, model.Role);

            return true;
        }

        //====================================================
        // Change Status
        //====================================================

        public async Task<bool> ChangeStatusAsync(string id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            user.IsActive = isActive;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        //====================================================
        // Reset Password
        //====================================================

        public async Task<bool> ResetPasswordAsync(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(
                user,
                token,
                newPassword);

            return result.Succeeded;
        }

        //====================================================
        // Change Role
        //====================================================

        public async Task<bool> ChangeRoleAsync(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, role);

            return result.Succeeded;
        }

        //====================================================
        // Get Roles
        //====================================================

        public async Task<List<string>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(x => x.Name!)
                .ToListAsync();
        }

        public async Task<CreateUserDto> GetCreateModelAsync()
        {
            return await Task.FromResult(new CreateUserDto());
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            var result = await _userManager.DeleteAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> ToggleStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                return false;

            user.IsActive = !user.IsActive;

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }
    }
}