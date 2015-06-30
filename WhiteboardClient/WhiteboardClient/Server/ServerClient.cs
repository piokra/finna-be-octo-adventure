using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using SharpGL;
using SharpGL.Shaders;
using System.Threading;
using System.Threading.Tasks;
using WhiteboardClient.Drawing;
namespace WhiteboardClient.Server
{
    class ServerClient
    {
        TcpClient mConnection = null;
        public ServerClient(string hostname, int port)
        {
            mConnection = new TcpClient(hostname, port);
        }

        Thread mThread = null;
        MainWindow mWindow = null;
        OpenGL mGL = null;
        ShaderProgram mShaderProgram = null;
        public void run(MainWindow window, ShaderProgram sp, OpenGL gl)
        {
            try
            {
                mThread = new Thread(new ThreadStart(listen));
                
                mWindow = window;
                mGL = gl;
                mShaderProgram = sp;
                
                
                
                mThread.Start();
            }
            catch (Exception)
            {
                
                
            }
        }

        public void stop()
        {
            try
            {
                mThread.Abort();
                mConnection.Close();
            }
            catch (Exception e)
            {
                
                
            }
        }
        Mutex mMutex = new Mutex();
        List<byte[]> mToSend = new List<byte[]>();
        List<byte[]> mReceived = new List<byte[]>();
        public void addMessage(byte[] bytes)
        {
            mMutex.WaitOne();
            mToSend.Add((byte[])bytes.Clone());
            mMutex.ReleaseMutex();
        
        }
        
        
        void listen()
        {
            try
            {
                while(true)
                {

                    sendMessages();
                    receiveMessages();
                    processMessages();
                }
            }
            catch (Exception e)
            {
                throw e;
              
            }
        }
        void sendMessages()
        {
            mMutex.WaitOne();
            int length = mToSend.Count;
            for (int i = 0; i < length; i++)
            {
                int msendlength = mToSend[i].Length;
                mConnection.GetStream().Write(mToSend[i], 0, msendlength);
            }
            mToSend.Clear();
            mMutex.ReleaseMutex();
        }
        Task mGotFirst = null;
        int mResultSize = 0;
        void receiveMessages()
        {
            try
            {


                byte b;
                List<byte> t = new List<byte>();
                byte[] count = new byte[4];

                Action a = delegate
                {


                    for (int i = 0; i < 4; i++)
                    {
                        try
                        {
                            count[i] = (byte)mConnection.GetStream().ReadByte();
                        }
                        catch (Exception)
                        {
                            
                            
                        }
                        

                    }
                    mResultSize = BitConverter.ToInt32(count, 0);
                };
                if (mGotFirst == null)
                {
                    mGotFirst = new Task(a);
                    mGotFirst.Start();
                }
                if (mGotFirst.Status == TaskStatus.RanToCompletion)
                {



                    byte[] data = new byte[mResultSize];

                    mConnection.GetStream().Read(data, 0, mResultSize);
                    t.AddRange(count);
                    t.AddRange(data);
                    mReceived.Add(t.ToArray());
                    mGotFirst = null;
                }
            }
            catch (Exception)
            {


            }
        }
        void processMessages()
        {
            int length = mReceived.Count;
            for (int i = 0; i < length; i++)
            {
                try
                {
                    if (mReceived[i][4] == 1)
                        drawableMessage(mReceived[i].ToArray());
                    if (mReceived[i][4] == 2)
                        clearMessage();
                    if (mReceived[i][4] == 3)
                        chatMessage(mReceived[i].ToArray());
                
                }
                catch (Exception)
                {
                    
                    
                }

                
            }
            mReceived.Clear();
        }
        void discardMessage()
        {

        }
        void clearMessage()
        {
            mWindow.clearDrawable();
        }
        void chatMessage(byte[] data)
        {
            mWindow.chatMessage(Encoding.ASCII.GetString(data).Substring(5));
        }
        void drawableMessage(byte[] data)
        {
            int length = (data.Length-17)/4;
            float[] floats = new float[length];
            float r, g, b;
            r = BitConverter.ToSingle(data, 5);
            g = BitConverter.ToSingle(data, 9);
            b = BitConverter.ToSingle(data, 13);
            for (int i = 0; i < length; i++)
            {
                
                floats[i] = BitConverter.ToSingle(data,17+4*i);
                
            }

            Drawable d = new Drawable(new Verticies((float[])floats.Clone(), 3, length, 3), mShaderProgram, mGL,r,g,b);
            mWindow.addDrawable(d);
        }
    }
}
