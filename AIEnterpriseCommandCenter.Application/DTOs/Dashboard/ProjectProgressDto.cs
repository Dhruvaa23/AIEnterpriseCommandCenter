using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Application.DTOs.Dashboard
{
    public class ProjectProgressDto
    {
        public string ProjectName { get; set; } = string.Empty;
        public int Progress { get; set; }
    }
}
