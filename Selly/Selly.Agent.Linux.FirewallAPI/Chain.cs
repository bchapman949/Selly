using System;
using System.Collections.Generic;
using System.Linq;

namespace Selly.Agent.Linux.FirewallAPI
{
    public abstract class Chain
    {
        public string Name { get; set; }
        public List<Entry> Entries { get; set; }

        public override string ToString()
        {
            return $"{Name}. Count: {Entries.Count}.";
        }

        public static Chain Parse(string input)
        {
            Chain result;

            var lines = input.Split('\n');

            // Chain type
            string chainType = lines[0];

            if (chainType.Contains("POLICY"))
            {
                var defaultChain = new DefaultChain();
                defaultChain.Policy = "";
                result = defaultChain;
            }
            else
            {
                var userChain = new UserChain();
                userChain.ReferenceCount = 0;
                result = userChain;
            }

            // Table headers
            string headers = lines[1];

            result.Name = "DefaultName";
            result.Entries = new List<Entry>();

            // FOREACH line
            for (int i = 2; i < lines.Length; i++)
            {
                // IF empty line
                if (String.IsNullOrWhiteSpace(lines[i])) { continue; }

                Entry entry = new Entry();

                // Tokenise
                var tokens = lines[i].Split(' ');
                tokens = tokens.Where(token => !string.IsNullOrWhiteSpace(token)).ToArray();

                //try{ entry.Pakcets = Convert.ToUInt32(tokens[0]); } catch (Exception e) { Console.WriteLine(tokens[0]); Console.ReadKey(); }
                entry.Packets = Convert.ToUInt32(tokens[0]);
                entry.Bytes = Convert.ToInt64(tokens[1].Replace('K', ' ').Replace('M', ' ').Trim());

                switch (tokens[2])
                {
                    case "DROP":
                        entry.Target = Target.DROP;
                        break;

                    case "ACCEPT":
                        entry.Target = Target.ACCEPT;
                        break;

                    default:
                        entry.Target = Target.DROP;
                        break;
                }

                switch (tokens[3])
                {
                    case "TCP":
                        entry.Protocol = Protocol.TCP;
                        break;

                    case "UDP":
                        entry.Protocol = Protocol.UDP;
                        break;

                    default:
                        entry.Protocol = Protocol.TCP;
                        break;
                }

                entry.Opt = tokens[4];
                entry.Input = tokens[5];
                entry.Out = tokens[6];
                entry.Source = tokens[7];
                entry.Destination = tokens[8];

                // END OF LINE
                result.Entries.Add(entry);
            }

            return result;
        }

        
    }
}