using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Selly.NMS.Web.Models
{
    public class DbPolicyRepository : IPolicyRepository
    {
        MainDbContext context;

        public DbPolicyRepository(MainDbContext db)
        {
            context = db;
        }

        public IEnumerable<Policy> Policies
        {
            get { return context.Policies.ToList(); }
        }

        public void AddPolicy(Policy policy)
        {
            context.Policies.Add(policy);
            context.SaveChanges();
        }

        public Policy GetPolicy(string id)
        {
            return context.Policies.Include(x => x.Rules).FirstOrDefault(x => x.Id == id);
        }

        public void DeletePolicy(string id)
        {
            context.PolicyRules.RemoveRange(context.PolicyRules.Where(x => x.PolicyId == id));

            var policy = context.Policies.FirstOrDefault(x => x.Id == id);
            if(policy == null) { return; }

            context.Policies.Remove(policy);
            context.SaveChanges();
        }

        public void AddPolicyRule(PolicyRule rule)
        {
            context.PolicyRules.Add(rule);
            context.SaveChanges();
        }

        public PolicyRule GetPolicyRule(string id, string ruleId)
        {
            return context.PolicyRules.FirstOrDefault(x => x.PolicyId == id && x.Id == ruleId);
        }

        public void UpdatePolicyRule(PolicyRule rule)
        {
            context.PolicyRules.Update(rule);
            context.SaveChanges();
        }

        public void DeletePolicyRule(string id)
        {
            context.PolicyRules.Remove(context.PolicyRules.FirstOrDefault(x=> x.Id == id));
            context.SaveChanges();
        }
    }
}
