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
            TcpClient tcp = new TcpClient("localhost", 10001);
            while (true)
            {
                
                try
                {
                    
                    List<byte> bytes = new List<byte>();
                    var vertices = new float[18];
                    var colors = new float[18]; // Colors for our vertices  
                    vertices[0] = -0.5f; vertices[1] = -0.5f; vertices[2] = 0.0f; // Bottom left corner  
                    colors[0] = 1.0f; colors[1] = 1.0f; colors[2] = 1.0f; // Bottom left corner  
                    vertices[3] = -0.5f; vertices[4] = 0.5f; vertices[5] = 0.0f; // Top left corner  
                    colors[3] = 1.0f; colors[4] = 0.0f; colors[5] = 0.0f; // Top left corner  
                    vertices[6] = 0.5f; vertices[7] = 0.5f; vertices[8] = 0.0f; // Top Right corner  
                    colors[6] = 0.0f; colors[7] = 1.0f; colors[8] = 0.0f; // Top Right corner  
                    vertices[9] = 0.5f; vertices[10] = -0.5f; vertices[11] = 0.0f; // Bottom right corner  
                    colors[9] = 0.0f; colors[10] = 0.0f; colors[11] = 1.0f; // Bottom right corner  
                    vertices[12] = -0.5f; vertices[13] = -0.5f; vertices[14] = 0.0f; // Bottom left corner  
                    colors[12] = 1.0f; colors[13] = 1.0f; colors[14] = 1.0f; // Bottom left corner  
                    vertices[15] = 0.5f; vertices[16] = 0.5f; vertices[17] = 0.0f; // Top Right corner  
                    colors[15] = 0.0f; colors[16] = 1.0f; colors[17] = 0.0f; // Top Right corner  
                    Int32 int32 = 18 * 4 + 1;
                    bytes.AddRange(BitConverter.GetBytes(int32));
                    bytes.Add(1);
                    for (int i = 0; i < 18; i++)
                    {
                        bytes.AddRange(BitConverter.GetBytes(vertices[i]));
                    }

                    byte[] data = bytes.ToArray();
                    while (!tcp.Connected) ;
                    Console.WriteLine("Connected.");
                    tcp.GetStream().Write(data, 0, data.Length);
                    System.Console.ReadKey();
                    
                }
                catch (Exception e)
                {

                    System.Console.WriteLine(e.ToString());
                }
                
            }
        }
    }
}
