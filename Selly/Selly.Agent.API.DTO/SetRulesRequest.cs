using System.Collections.Generic;

namespace Selly.Agent.API.DTO
{
    public class SetRulesRequest
    {
        public ICollection<Generic.Models.Rule> GenericRequest { get; set; }
    }
}
