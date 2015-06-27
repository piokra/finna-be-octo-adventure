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
        public MainWindow()
        {
            InitializeComponent();
            mPoints.Add(new Vertex(0, 0, 0, 1));
            mPoints.Add(new Vertex(0, 1, 0, 1));
            mPoints.Add(new Vertex(1, 1, 0, 1));

            
        }

        /// <summary>
        /// Handles the OpenGLDraw event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        class Vertex
        {
            public Vertex()
            {

            }
            public Vertex(float x, float y, float z, float w)
            {
                this.x=x;
                this.y=y;
                this.z=z;
                this.w=w;
            }
            public float x,y,z,w;
        };
        List<Vertex> mPoints = new List<Vertex>();
        float[] mTestVerticies = { 0,0,0,
                                   1,0,0,
                                   1,1,0,
                                   1,1,0,
                                   0,1,0,
                                   1,0,0};
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            //  Bind the shader, set the matrices.
            mShaderProgram.Bind(gl);


            //  Bind the out vertex array.
            mVertexBufferArray.Bind(gl);

            //  Draw the square.
            gl.DrawArrays(OpenGL.GL_TRIANGLES, 0, 6);

            //  Unbind our vertex array and shader.
            mVertexBufferArray.Unbind(gl);
            mShaderProgram.Unbind(gl);
           


        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>

        ShaderProgram mShaderProgram;
        VertexBufferArray mVertexBufferArray;

        uint mPositionHandle = 0;
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            
            
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

            //  Create the vertex array object.
            mVertexBufferArray = new VertexBufferArray();
            mVertexBufferArray.Create(gl);
            mVertexBufferArray.Bind(gl);

            //  Create a vertex buffer for the vertex data.
            var vertexDataBuffer = new VertexBuffer();
            vertexDataBuffer.Create(gl);
            vertexDataBuffer.Bind(gl);
            vertexDataBuffer.SetData(gl, 0, vertices, false, 3);

            //  Now do the same for the colour data.

            //  Unbind the vertex array, we've finished specifying data for it.
            mVertexBufferArray.Unbind(gl);
            



            
            
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
                ListBox1.Items.Add(TextBox1.Text);
                TextBox1.Text = "";
            }
        }
        bool mMouseDown = false;
        private void openGLControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mMouseDown = false;
        }

        private void openGLControl_MouseLeave(object sender, MouseEventArgs e)
        {
            mMouseDown = false;
        }

        private void openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mMouseDown)
            {


                float x = (float)e.GetPosition(openGLControl).X;
                float y = (float)e.GetPosition(openGLControl).Y;
                x = x * 2 / (float)openGLControl.ActualWidth;
                y = -y * 2 / (float)openGLControl.ActualHeight;
                x -= 1;
                y += 1;
                mPoints.Add(new Vertex(x, y, 0, 1));
                ListBox1.Items.Add(x.ToString() + " " + y.ToString());
            }
       }

        private void openGLControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            mMouseDown = true;
        }
    }
}
