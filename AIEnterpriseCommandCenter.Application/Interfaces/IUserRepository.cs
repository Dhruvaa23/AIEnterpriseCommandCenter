using AIEnterpriseCommandCenter.Application.DTOs.User;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IUserRepository
    {
        // Get all users
        Task<List<UserDto>> GetAllAsync();

        // Get user by Id
        Task<UpdateUserDto?> GetByIdAsync(string id);

        Task<CreateUserDto> GetCreateModelAsync();
        // Create new user
        Task<bool> CreateAsync(CreateUserDto model);

        // Update existing user
        Task<bool> UpdateAsync(UpdateUserDto model);

        // Activate / Deactivate user
        Task<bool> ChangeStatusAsync(string id, bool isActive);

        // Reset Password
        Task<bool> ResetPasswordAsync(string id, string newPassword);

        // Change Role
        Task<bool> ChangeRoleAsync(string id, string role);

        // Get available roles
        Task<List<string>> GetRolesAsync();

        Task<bool> DeleteAsync(string id);

        Task<bool> ToggleStatusAsync(string id);
    }
}