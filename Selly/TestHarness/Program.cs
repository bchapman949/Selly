using Selly.Agent.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestHarness
{
    class Program
    {
        static void Main(string[] args)
        {
            Selly.Agent.Windows.Initialiser.Init();
            Selly.Agent.API.Program.Main(args, new Selly.Agent.Windows.ApiCallbacks());
        }
    }
}
