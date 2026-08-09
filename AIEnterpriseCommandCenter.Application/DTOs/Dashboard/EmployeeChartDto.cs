using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Dashboard
{
    public class EmployeeChartDto
    {
        public string Month { get; set; } = string.Empty;

        public int TotalEmployees { get; set; }
    }
}
