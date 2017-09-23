using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Models
{
    public interface IPolicyRepository
    {
        IEnumerable<Policy> Policies { get; }

        void AddPolicy(Policy policy);
        Policy GetPolicy(string id);
        void DeletePolicy(string id);

        void AddPolicyRule(PolicyRule rule);
        PolicyRule GetPolicyRule(string id, string ruleId);
        void UpdatePolicyRule(PolicyRule rule);
        void DeletePolicyRule(string id);
    }
}
