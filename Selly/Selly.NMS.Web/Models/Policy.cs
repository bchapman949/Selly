using System.Collections.Generic;

namespace Selly.NMS.Web.Models
{
    public class Policy
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public List<PolicyRule> Rules { get; set; }
    }
}
