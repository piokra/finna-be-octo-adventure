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
                if(data.Length>0)
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
                    int clienti = mClients.Count - 1;
                    ThreadStart a = delegate 
                    {
                        listenForNewData(clienti);
                    };
                    mListeners.Add(new Thread(a));
                    mMutexes.Add(new Mutex());
                    mReceivedData.Add(new List<byte>());
                    mListeners[clienti].Start();
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
                        pos = i;
                        if(mClients[pos]!=null)
                        if(!mClients[pos].Connected)
                        {
                            TcpClient t = mClients[pos];
                            mClients[pos] = null;
                           
                            t.Close();
                            mListeners[pos].Abort();
                            mMutexes[pos].Close();
                            mMutexes[pos] = null;
                            mReceivedData[pos] = null;
                            
                            mListeners[pos] = null;
                        }
                        else
                        {
                            
                        }
                        i++;
                    }
                    catch (Exception e)
                {

                    Console.WriteLine(e.ToString());
                }
            }

        }
        List<Thread> mListeners = new List<Thread>();
        List<Mutex> mMutexes = new List<Mutex>();
        List<List<byte>> mReceivedData = new List<List<byte>>();
        List<byte> checkForNewData()
        {
            List<byte> data = new List<byte>();
            for (int i = 0; i < mReceivedData.Count; i++ )
            {
                if(mMutexes[i]!=null)
                if(mReceivedData[i].Count > 0)
                {
                    
                    mMutexes[i].WaitOne();

                    data.AddRange(mReceivedData[i]);
                    mReceivedData[i].Clear();
                    mMutexes[i].ReleaseMutex();
                    return data;

                    
                }
            }
            return data;
            
            
        }
        void listenForNewData(int clientIndex)
        {
            try
            {
                while(true)
                {
                    byte[] count = new byte[4];
                    
                    
                    
                    mClients[clientIndex].GetStream().Read(count, 0, 4);
                    
                    int size = BitConverter.ToInt32(count, 0);
                    byte[] data = new byte[size];

                    mClients[clientIndex].GetStream().Read(data, 0, size);
                    mMutexes[clientIndex].WaitOne();
                    mReceivedData[clientIndex].AddRange(count);
                    mReceivedData[clientIndex].AddRange(data);
                    mMutexes[clientIndex].ReleaseMutex();
                }
            }
            catch (Exception e)
            {

                System.Console.WriteLine(e.ToString());
            }
        }
        void broadcast(byte[] data)
        {
            
            int length = mClients.Count();
            List<Task> tasks = new List<Task>();
            Console.WriteLine("Broadcasting: " + data.Length);
            for (int i = 0; i < data.Length; i++ )
            {
                Console.Write(data[i]+" ");
            }
            Console.WriteLine();
            for (int i = 0; i < length; i++)
            {
                if (mClients[i] != null)
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
                if(tasks[i]!=null)
                    tasks[i].Wait(100);
            }
        }
    }
}
