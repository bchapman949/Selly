namespace Selly.Agent.Linux.FirewallAPI
{
    public class Entry
    {
        public long Packets {get;set;}
        public long Bytes {get;set;}
        public Target Target {get;set;}
        public Protocol Protocol {get;set;}
        public string Opt {get;set;}
        public string Input {get;set;}
        public string Out {get;set;}
        public string Source {get;set;}
        public string Destination {get;set;}
    }
}