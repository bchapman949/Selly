using Selly.Agent.API.DTO;

namespace Selly.Agent.API
{
    public interface IApiCallbacks
    {
        EchoResponse Echo();
        GetConfigurationResponse GetConfiguration();
        ConfigureResponse Configure(ConfigureRequest request);

        GetRulesResponse GetRules();
        SetRuleResponse NewRule(SetRuleRequest rule);
        void UpdateRule(string id, SetRuleRequest rule);
        void DeleteRule(string id);

        SetRulesResponse UpdateRules(SetRulesRequest request);
    }
}
