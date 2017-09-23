// This project must be .NET Core as Process is not in .NET Standard
// until version 2. The tooling for version 2 is incomplete.

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Selly.Agent.Linux.FirewallAPI
{
    // TODO: Hard coded paths
    public class FirewallManager
    {
        public static bool IsEnabled()
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = "status";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadLine();

                if(output.Contains("inactive"))
                {
                    return false;
                }
                
                return true;
            }
        }

        public static void Enable()
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = "enable";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
            }
        }

        public static void Disable()
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = "disable";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
            }
        }

        // TODO: NFLOG
        // https://bugs.launchpad.net/ufw/+bug/1475676
        // https://wiki.ubuntu.com/UncomplicatedFirewall#Advanced_Functionality
        public static void Patch()
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = "sudo sed 's/-j LOG --log-prefix/-j NFLOG --nflog-prefix/' -i.bak /etc/ufw/user.rules";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
            }
        }

        public static void NewRule(Linux.FirewallAPI.Rule rule)
        {
            var args = rule.Create();

            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = args;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();
            }

            // TODO: Magic number (delay period)
            Task.Delay(500).Wait();
        }

        public static void DeleteRule(uint number)
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = $"delete {number}";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardInput = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                proc.StandardInput.WriteLine("y");
                proc.StandardInput.Flush();
                var output = proc.StandardOutput.ReadToEnd();
            }
        }

        public static List<Rule> GetRules()
        {
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = "status numbered";
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                var output = proc.StandardOutput.ReadToEnd();
                var rules = Rule.Parse(output);
                return rules;
            }
        }

        /// <summary>
        /// Note there is no support for name unlike Windows.
        /// </summary>
        /// <param name="rule"></param>
        public static void UpdateRule(Rule rule)
        {
            DeleteRule(rule.Number);
            
            var args = $"insert {rule.Number} {rule.Create()}";
            
            using(Process proc = new Process())
            {
                proc.StartInfo.FileName = "/usr/sbin/ufw";
                proc.StartInfo.Arguments = args;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();


                var output = proc.StandardOutput.ReadToEnd();
                var err = proc.StandardError.ReadToEnd();
                
                if(err.Contains("Invalid position"))
                {
                    Console.WriteLine("Invalid position. Attempting to create new rule.");
                    NewRule(rule);
                }
            }
        }
    }
}