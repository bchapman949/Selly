using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Selly.Agent.Linux.Helpers
{
    public class UfwLogHelper
    {
        const string name = "UfwLogHelper";
        Process proc;

        public UfwLogHelper()
        {
            // TODO: Hard coded path
            proc = new Process();
            proc.StartInfo.FileName = "/usr/bin/sudo";
            proc.StartInfo.Arguments = $"-u \"user\" /bin/tailf -n0 /var/log/ufw.log";
            proc.EnableRaisingEvents = true;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.OutputDataReceived += OutputDataReceivedOccured;
            proc.ErrorDataReceived += ErrorDataReceivedOccured;
        }

        public void Start()
        {
            try
            {
                Console.WriteLine($"{name}: Starting");
                var result = proc.Start();
                Console.WriteLine($"{name}: Started ({result})");
                proc.BeginOutputReadLine();
                proc.BeginErrorReadLine();
            }
            catch(Exception e)
            {
                Console.WriteLine($"{name}: ERROR starting: {e.Message}");
            }            
        }


        public void Stop()
        {
            try
            {
                Console.WriteLine($"{name}: Signalling termination");
                proc.Kill();
                Console.WriteLine($"{name}: Waiting for termination");
                proc.WaitForExit();
                Console.WriteLine($"{name}: Terminated? {proc.HasExited}. Code: {proc.ExitCode}");
            }
            catch(Exception e)
            {
                Console.WriteLine($"{name}: ERROR terminating: {e.Message}");
            }
            finally
            {
                Console.WriteLine($"{name}: Dispose");
                proc.Dispose();
            }
        }

        // https://help.ubuntu.com/community/UFW#Interpreting_Log_Entries
        private void OutputDataReceivedOccured(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(e.Data)){ return; }

                var packet = new NMS.API.DTO.PacketDroppedEvent();

                var timeString = e.Data.Substring(0, 15);
                var format = "MMM d HH:mm:ss";                

                DateTimeOffset result;
                if (DateTimeOffset.TryParseExact(timeString, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out result))
                {
                    packet.Time = result;
                }
                else
                {
                    packet.Time = DateTimeOffset.Now;
                }
                
                packet.LocalAddress = MatchAndRetrieve(@"DST=\d+\.\d+\.\d+\.\d+", e.Data);
                packet.LocalPort = Convert.ToUInt16(MatchAndRetrieve(@"DPT=\d+", e.Data));
                packet.RemoteAddress = MatchAndRetrieve(@"SRC=\d+\.\d+\.\d+\.\d+", e.Data);
                packet.RemotePort = Convert.ToUInt16(MatchAndRetrieve(@"SPT=\d+", e.Data));
                
                Console.WriteLine($"{name}: STDOUT: {packet.ToString()}");

                using (NmsApiClient client = new NmsApiClient())
                {
                    client.SendEvent(packet).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{name}: ERROR in STDOUT: Message: {ex.Message}");
                Console.WriteLine($"{name}: ERROR in STDOUT: Message: {e.Data}");
            }
        }

        private void ErrorDataReceivedOccured(object sender, DataReceivedEventArgs e)
        {
            if(string.IsNullOrEmpty(e.Data))
            {
                return;
            }

            Console.WriteLine($"{name}: STDERR: {e.Data}");
        }

        public static byte[] StringToByteArray(string hex) 
        {
            return Enumerable.Range(0, hex.Length)
                            .Where(x => x % 2 == 0)
                            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                            .ToArray();
        }

        private string MatchAndRetrieve(string pattern, string content)
        {
            Regex regex = new Regex(pattern);
            Match match = regex.Match(content);

            if(!match.Success) { return null; }

            string[] tokens = match.Value.Split('=');
            return tokens[1];
        }
    }
}
