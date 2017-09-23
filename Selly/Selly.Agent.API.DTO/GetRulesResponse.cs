using System.Collections.Generic;

namespace Selly.Agent.API.DTO
{
    public class GetRulesResponse
    {
        public ICollection<Windows.Models.Rule> WindowsResult { get; set; }
        public ICollection<Linux.Models.Rule> UfwResult {get;set;}
    }
}
