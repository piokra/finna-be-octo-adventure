using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Broadcaster.Broadcaster
{
    class Server
    {
        public Server(int port)
        {
            mPort = port;
            mServer = new TcpListener(mPort);
        }
        int mPort = -1;
        TcpListener mServer = null;
        Thread mThread = null;
        public void run()
        {
            mThread = new Thread(new ThreadStart(listen));
            mThread.Start();
        }

        public void stop()
        {
            try
            {
                mThread.Abort();
                mServer.Stop();
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
            int length = mClients.Count();
            for (int i = 0; i < length; i++)
            {
                try
                {
                    mClients[i].GetStream().Close();
                    mClients[i].Close();
                }
                catch (Exception)
                {
                    
                    
                }
            }
            
            mClients = new List<TcpClient>();
            mData = new List<byte[]>();
            mThread = null;
            
        }
        List<TcpClient> mClients = new List<TcpClient>();
        List<Byte[]> mData = new List<Byte[]>();
        void listen()
        {
            mServer.Start();
            while(true)
            {
                addClients();
                byte[] data = checkForNewData().ToArray();
                broadcast(data);
                removeDisconnectedClients();
            }
            
        }   
        void addClients()
        {
            try
            {
                while(mServer.Pending())
                {
                    System.Console.WriteLine("Connection pending");
                    TcpClient tcp = mServer.AcceptTcpClient();
                    mClients.Add(tcp);
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                
            }
        }
        void removeDisconnectedClients()
        {
            
                int length = mClients.Count;
                int i = 0;
                int pos = 0;
                while (i<length)
                {
                    try
                    {
                        if(!mClients[i].Connected)
                        {
                            TcpClient t = mClients[i];
                            mClients.RemoveAt(pos);
                            t.Close();
                        }
                        else
                        {
                            pos++;
                        }
                        i++;
                    }
                    catch (Exception e)
                {

                    Console.WriteLine(e.ToString());
                }
            }

        }
        List<byte> checkForNewData()
        {
            List<byte> data = new List<byte>();
            
            int length = mClients.Count;
            byte b = 0;
            try
            {



                for (int i = 0; i < length; i++)
                {
                    System.Console.WriteLine(i);
                    
                    while ((b = (byte)mClients[i].GetStream().ReadByte()) > 0)
                    {
                        Console.WriteLine(b);
                        data.Add(b);
                    }
                    data.Add(0);
                    Console.WriteLine("Done.");                        
                }
                length = data.Count();
                for (int i = 0; i < length; i++)
                {
                    System.Console.Write(data[i]);
                }
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
            return data;
        }

        void broadcast(byte[] data)
        {
            
            int length = mClients.Count();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < length; i++)
            {
                
                try
                {
                    Task task = mClients[i].GetStream().WriteAsync(data, 0, data.Length);
                    tasks.Add(task);
                }
                catch (Exception)
                {
                    
                    
                }
            }

            for(int i=0; i<tasks.Count; i++)
            {
                tasks[i].Wait(100);
            }
        }
    }
}
