using Microsoft.AspNetCore.Mvc;
using Selly.Agent.API.DTO;

namespace Selly.Agent.API.Controllers
{
    [Route("api/[controller]")]
    public class RulesController : Controller
    {
        // GET api/rules
        // Get all rules
        [HttpGet]
        public GetRulesResponse Get()
        {
            return Program.Callbacks.GetRules();
        }

        // POST api/rules
        // Create new rule
        [HttpPost]
        public SetRuleResponse Post([FromBody] SetRuleRequest rule)
        {
            return Program.Callbacks.NewRule(rule);
        }

        // PUT api/rules
        // Update all rules
        [HttpPut]
        public SetRulesResponse Put([FromBody]SetRulesRequest value)
        {
            return Program.Callbacks.UpdateRules(value);
        }

        // DELETE api/rules
        // Reset firewall to default
        [HttpDelete]
        public void Delete()
        {
        }

        // GET api/rules/1
        // Get rule
        public void Get([FromRoute] string id)
        {

        }

        // PUT api/rules/1
        // Update rule
        [HttpPut("{id}")]
        public void Put([FromRoute] string id, [FromBody] SetRuleRequest rule)
        {
            Program.Callbacks.UpdateRule(id, rule);
        }

        // DELETE api/rules/1
        // Delete rule
        [HttpDelete("{id}")]
        public void Delete([FromRoute] string id)
        {
            Program.Callbacks.DeleteRule(id);
        }
    }
}
