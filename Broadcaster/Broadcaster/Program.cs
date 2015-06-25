using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Broadcaster.Broadcaster;
namespace Broadcaster
{
    class Program
    {
        static void Main(string[] args)
        {
            int cores = Environment.ProcessorCount;
            Server server = new Server(10001);
            server.run();
            System.Console.ReadKey();
            server.stop();
            System.Console.WriteLine("Aborted.");
            System.Console.ReadKey();
        }
    }
}
