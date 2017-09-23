namespace Selly.Agent.API.DTO
{
    public class SetRuleRequest
    {
        public Generic.Models.Rule GenericRequest { get; set; }
        public Linux.Models.Rule UfwRequest { get; set; }
        public Windows.Models.Rule WindowsRequst { get; set; }
    }
}
