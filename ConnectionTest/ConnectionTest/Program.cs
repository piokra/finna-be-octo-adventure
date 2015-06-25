using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
namespace ConnectionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ipAddress = Dns.GetHostAddresses("localhost")[1];
            
            System.Console.WriteLine(ipAddress.ToString());
            try
            {
                TcpClient tcp = new TcpClient("localhost", 10001);
                byte[] data = Encoding.ASCII.GetBytes("Hello"+(char)0);
                while (!tcp.Connected) ;
                Console.WriteLine("Connected.");
                tcp.GetStream().Write(data, 0, data.Length);
                byte b;
                while ((b = (byte)tcp.GetStream().ReadByte()) > 0)
                    Console.WriteLine(b);
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
            System.Console.ReadKey();
            
        }
    }
}
