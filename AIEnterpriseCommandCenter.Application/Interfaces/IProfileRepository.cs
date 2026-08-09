using AIEnterpriseCommandCenter.Application.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.Interfaces
{
    public interface IProfileRepository
    {
        Task<ProfileDto?> GetProfileAsync(string userId);

        Task<bool> UpdateProfileAsync(ProfileDto model);
    }
}
