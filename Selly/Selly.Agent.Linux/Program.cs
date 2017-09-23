using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Selly.Agent.Linux
{
    // Loosely based on https://stackoverflow.com/questions/38291567/killing-gracefully-a-net-core-daemon-running-on-linux
    class Program
    {
        const string name = "Program";
        private static SellyService _instance;
        
        static void Main(string[] args)
        {
            AssemblyLoadContext.Default.Unloading += Terminate;
            Console.CancelKeyPress += (s, e) => { Console.WriteLine($"{name}: Cancel key pressed"); };

            _instance = new SellyService();
            _instance.OnStart(args);
        }

        private static void Terminate(AssemblyLoadContext obj)
        {
            try
            {
                _instance.OnStop();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{name}: Termination exception: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Environment Exit");
                Environment.Exit(0);
            }
        }
    }
}