using Selly.Agent.Generic.Models;
using Selly.NMS.Web.Models;
using System.Collections.Generic;

namespace Selly.NMS.Web.ViewModels.Rules
{
    public class IndexVM
    {
        public IEnumerable<IRule> Rules { get; set; }
        public Models.Device Device { get; set; }
    }
}
