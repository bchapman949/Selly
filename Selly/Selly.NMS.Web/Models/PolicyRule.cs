namespace Selly.NMS.Web.Models
{
    public class PolicyRule
    {
        public string Id { get; set; }

        public string PolicyId { get; set; }
        public Policy Policy { get; set; }

        // TODO: I don't particularly like this, but I don't want to extend Generic.Rule.
        // I don't know if that will work properly and I'm not taking chances this late into the project.
        // Also note some of the types have been changed for db compatability
        public string Name { get; set; }
        public int Action { get; set; }
        public int Protocol { get; set; }
        public string LocalAddress { get; set; }
        public int LocalPort { get; set; }
        public string RemoteAddress { get; set; }
        public int RemotePort { get; set; }
        public int Direction { get; set; }
    }
}