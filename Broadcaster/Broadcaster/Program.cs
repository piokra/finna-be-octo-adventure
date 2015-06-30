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
            int port = 10001;
            try
            {
                int tport = Int32.Parse(args[0]);
                port = tport;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
            }
            
            Server server = new Server(port);
            server.run();
            System.Console.ReadKey();
            server.stop();
            System.Console.WriteLine("Aborted.");
            System.Console.ReadKey();
        }
    }
}
