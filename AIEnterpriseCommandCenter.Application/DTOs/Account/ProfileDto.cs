using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace AIEnterpriseCommandCenter.Application.DTOs.Account
{
    public class ProfileDto
    {
        public string Id { get; set; } = "";

        [Required]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        public string EmployeeCode { get; set; } = "";

        public string Email { get; set; } = "";

        public string Department { get; set; } = "";

        public string Designation { get; set; } = "";

        public string Role { get; set; } = "";

        public bool IsActive { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ProfilePicture { get; set; }
    }
}
