using System;
using System.Diagnostics;
using System.Linq;
namespace Selly.Agent.Linux.Helpers
{
    public class NetfilterLogHelper
    {
        const string name = "NetfilterLogHelper";
        Process proc;

        public NetfilterLogHelper()
        {
            // TODO: Hard coded path
            proc = new Process();
            proc.StartInfo.Environment.Add("LD_LIBRARY_PATH", "/usr/local/lib");
            proc.StartInfo.FileName = "/home/user/Repos/selly-uob/Selly/Selly.Agent.Linux.NetfilterLog/main";
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

        private void OutputDataReceivedOccured(object sender, DataReceivedEventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(e.Data)){ return; }

                Console.WriteLine($"{name}: STDOUT: {e.Data}");

                var parts = e.Data.Split('|');

                if (parts.Length < 2) { return; }

                using (NmsApiClient client = new NmsApiClient())
                {
                    client.SendEvent(new NMS.API.DTO.PacketDroppedEvent()
                    {
                        Time = DateTimeOffset.Now,
                        LocalAddress = parts[1],
                        LocalPort = Convert.ToUInt16(parts[3]),
                        RemoteAddress = parts[0],
                        RemotePort = Convert.ToUInt16(parts[2])

                    }).Wait();
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
    }
}
