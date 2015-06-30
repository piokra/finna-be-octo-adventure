using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpGL.SceneGraph;
using SharpGL;
using SharpGL.VertexBuffers;
using SharpGL.Shaders;
using System.IO;
using WhiteboardClient.Drawing;
using WhiteboardClient.Server;
using System.Threading;
namespace WhiteboardClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        /// 
        public MainWindow()
        {
            
            InitializeComponent();
            throw new Exception("You can't use this constructor");
            

            
        }

        public MainWindow(string hostname, int port)
        {
            InitializeComponent();
            mClient = new ServerClient(hostname, port);
            Random r = new Random();
            mR = (float)r.NextDouble();
            mG = (float)r.NextDouble();
            mB = (float)r.NextDouble();
        }

        public void Dispose()
        {
            mClient.stop();
        }
        ServerClient mClient = null;
        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>

        List<float> mPoints = new List<float>();
        float[] mTestVerticies = { 0,0,0,
                                   1,0,0,
                                   1,1,0,
                                   1,1,0,
                                   0,1,0,
                                   1,0,0};
        List<Drawable> mToDraw = new List<Drawable>();
        Mutex mDrawMutex = new Mutex();
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            mDrawMutex.WaitOne();
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);
            mDrawable = new Drawable(new Verticies(mPoints.ToArray(), 3, mPoints.Count / 3, 3), mShaderProgram, gl,mR,mG,mB);
            mDrawable.draw(gl);
            int length = mToDraw.Count();
            for (int i = 0; i < length; i++)
            {
                mToDraw[i].draw(gl);
            }

            mDrawMutex.ReleaseMutex();

            updateChat();
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>

        ShaderProgram mShaderProgram;
        VertexBufferArray mVertexBufferArray;

        uint mPositionHandle = 0;
        Drawable mDrawable = null;
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            gl.PointSize(10);
            
            string vertex = File.ReadAllText("Shaders//Vertex//default.vert");
            string frag = File.ReadAllText("Shaders//Fragment//default.frag");
            mShaderProgram = new ShaderProgram();
            mShaderProgram.Create(gl, vertex, frag, null);
            mShaderProgram.BindAttributeLocation(gl, mPositionHandle, "in_Position");
            mShaderProgram.AssertValid(gl);

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

            //mDrawable = new Drawable(new Verticies(vertices, 3, 6, 3),mShaderProgram,gl);

            mClient.run(this, mShaderProgram, gl);

            
            
            //  Set the clear color.
            gl.ClearColor(0, 0.3f, 1, 1);
            
        }

        /// <summary>
        /// Handles the Resized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            //  TODO: Set the projection matrix here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            

            //  Set the projection matrix.
            gl.Viewport(0, 0, (int)openGLControl.ActualWidth, (int)openGLControl.ActualHeight);
            //  Create a perspective transformation.
            
            //  Use the 'look at' helper function to position and aim the camera.
            
            //  Set the modelview matrix.
        }

        /// <summary>
        /// The current rotation.
        /// </summary>


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key==Key.Enter)
            {
                

                List<byte> bytes = new List<byte>();
                byte[] text = Encoding.ASCII.GetBytes(TextBox1.Text);
                bytes.AddRange(BitConverter.GetBytes(text.Length+1));
                bytes.Add(3);
                bytes.AddRange(text);
                mClient.addMessage(bytes.ToArray());
                TextBox1.Text = "";
                
                
            }
        }
        bool mMouseDown = false;
        private void openGLControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            
            finalizeDrawing();
        }

        private void openGLControl_MouseLeave(object sender, MouseEventArgs e)
        {
            
            finalizeDrawing();
        
        }

        
        public void addDrawable(Drawable drawable)
        {
            mDrawMutex.WaitOne();
            mToDraw.Add(drawable);
            mDrawMutex.ReleaseMutex();
        }
        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            
            if (mMouseDown)
            {
                mStep--;
                if (mStep == 0)
                {

                    mStep = 4;
                    float x = (float)e.GetPosition(openGLControl).X;
                    float y = (float)e.GetPosition(openGLControl).Y;
                    x = x * 2 / (float)openGLControl.ActualWidth;
                    y = -y * 2 / (float)openGLControl.ActualHeight;
                    x -= 1;
                    y += 1;
                    mPoints.Add(x);
                    mPoints.Add(y);
                    mPoints.Add(0);
                    
                } 
           }
       }
        int mStep = 30;
        float mR, mG, mB;
        void finalizeDrawing()
        {
            if (mMouseDown == false) return;
            mMouseDown = false;
            List<byte> bytes = new List<byte>();
            int length = mPoints.Count();

            bytes.AddRange(BitConverter.GetBytes(length * 4 + 13));
            bytes.Add(1);
            bytes.AddRange(BitConverter.GetBytes(mR));
            bytes.AddRange(BitConverter.GetBytes(mG));
            bytes.AddRange(BitConverter.GetBytes(mB));
            for (int i = 0; i < length; i++)
            {
                bytes.AddRange(BitConverter.GetBytes(mPoints[i]));
            }
            mClient.addMessage(bytes.ToArray());
            mPoints.Clear();
        }
        private void openGLControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mMouseDown = true;
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            clearDrawable();
            byte[] msg = { 1, 0, 0, 0, 2 };
            mClient.addMessage(msg);
            
        }
        Mutex mChatMutex = new Mutex();
        List<string> mChatTexts = new List<string>();
        public void chatMessage(string text)
        {
            mChatMutex.WaitOne();
            mChatTexts.Add(text);
            mChatMutex.ReleaseMutex();
            
        }
        public void updateChat()
        {
            mChatMutex.WaitOne();
            int length = mChatTexts.Count;
            for (int i = 0; i < length; i++)
            {
                ListBox1.Items.Insert(0, mChatTexts[i]);
            }
            mChatTexts.Clear();
            mChatMutex.ReleaseMutex();
        }
        public void clearDrawable()
        {
            mToDraw.Clear();
           
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                mClient.stop();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            mR = (float)r.NextDouble();
            mG = (float)r.NextDouble();
            mB = (float)r.NextDouble();
        }
    }
}
