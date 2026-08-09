using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterpriseCommandCenter.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }

        public string Title { get; set; } = "";

        public string Message { get; set; } = "";

        public string Type { get; set; } = "";

        public bool IsRead { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
