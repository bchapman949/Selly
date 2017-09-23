using Microsoft.Diagnostics.Tracing;
using Selly.NMS.API.DTO;
using System;
using System.Diagnostics;

namespace Selly.Agent.Windows.Helpers
{
    static class EtwCallback
    {
        private static DateTime time;
        public static async void EventOccured(TraceEvent eventData)
        {
            try
            {
                if (time != null)
                {
                    if (DateTime.Now.Subtract(time) <= TimeSpan.FromSeconds(10))
                    {
                        return;
                    }
                }

                time = DateTime.Now;
                var packetDroppedEvent = new PacketDroppedEvent();
                packetDroppedEvent.Time = DateTimeOffset.Now;

                var data = eventData.EventData();

                ulong fid = 0;
                uint direction = 0;
                ushort layerId = 0;

                int fidStart = 0;
                int appIdEnd = FindEndOfAppNameString(data);

                if (appIdEnd > 0)
                {
                    fidStart = appIdEnd + 32;
                    fid = BitConverter.ToUInt64(data, fidStart);
                    layerId = BitConverter.ToUInt16(data, fidStart + 8);
                }
                
                packetDroppedEvent.FilterName = GetFilterName(fid);
                if(packetDroppedEvent.FilterName.Contains("0x80320003"))
                {
                    appIdEnd = FindEndOfAppNameString(data);

                    if (appIdEnd > 0)
                    {
                        fidStart = appIdEnd + 48;
                        fid = BitConverter.ToUInt64(data, fidStart);
                        layerId = BitConverter.ToUInt16(data, fidStart + 8);

                        packetDroppedEvent.FilterName = GetFilterName(fid);
                    }
                }

                // Local address
                byte[] local = (byte[]) eventData.PayloadByName("LocalAddress");
                packetDroppedEvent.LocalAddress = GetAddress(local);
                packetDroppedEvent.LocalPort = GetPort(local);

                // Remote address
                byte[] remote = (byte[])eventData.PayloadByName("RemoteAddress");
                packetDroppedEvent.RemoteAddress = GetAddress(remote);
                packetDroppedEvent.RemotePort = GetPort(remote);

                ToastHelper.PopToast($"Event occured. Port: {packetDroppedEvent.LocalPort}. Address: {packetDroppedEvent.RemoteAddress}. FID: {packetDroppedEvent.FilterName}. LayerID: {layerId}. Direction: {direction}");
                await SellyService.ApiClient.SendEvent(packetDroppedEvent);

            }
            catch (Exception ex)
            {
                ExceptionHelper.WriteFile(ex, "ETW callback");
            }
        }

        // TODO: Replace with strongly typed ETW
        // That should also deal with IPv6 addressess
        private static string GetAddress(byte[] data)
        {
            return $"{data[4]}.{data[5]}.{data[6]}.{data[7]}";
        }

        // TODO: Replace with strongly typed ETW
        private static ushort GetPort(byte[] data)
        {
            Debug.Assert(data.Length > 3);

            byte[] portNumber = new byte[2];
            portNumber[0] = data[3];
            portNumber[1] = data[2];

            var port = BitConverter.ToUInt16(portNumber, 0);
            return port;
        }

        private static int FindEndOfAppNameString(byte[] data)
        {
            for(int i = 52; i + 1 < data.Length; i+=2)
            {
                if(data[i] == 0 && data[i+1] == 0)
                {
                    return i + 2;
                }
            }

            return -1;
        }

        private static string GetFilterName(ulong fid)
        {
            // TODO: Hard coded path / needs to be replaced with p/invoke
            using (Process proc = new Process())
            {
                proc.StartInfo.FileName = @"C:\Repos\selly-uob\Selly\Selly.Agent.Windows.FirewallAPI.FilterFinder\Debug\Selly.Agent.Windows.FirewallAPI.FilterFinder.exe";
                proc.StartInfo.Arguments = fid.ToString();
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.Start();

                string filterName = proc.StandardOutput.ReadToEnd();
                return filterName;
            }
        }
    }
}
