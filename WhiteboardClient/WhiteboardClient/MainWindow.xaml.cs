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
            public Vertex(double x, double y, double z, double w)
            {
                this.x=x;
                this.y=y;
                this.z=z;
                this.w=w;
            }
            public double x,y,z,w;
        };
        List<Vertex> mPoints = new List<Vertex>();
        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Clear the color and depth buffer.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            //  Load the identity matrix.
            gl.LoadIdentity();

            //  Rotate around the Y axis.
            gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);

            //  Draw a coloured pyramid.
            gl.Begin(OpenGL.GL_LINE_LOOP);
            int length = mPoints.Count;
            for (int i = 0; i < length; i++)
            {
                gl.Vertex(mPoints[i].x, mPoints[i].y, mPoints[i].z);
            }
            gl.End();

            //  Nudge the rotation.
            rotation += 3.0f;
        }

        /// <summary>
        /// Handles the OpenGLInitialized event of the openGLControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="SharpGL.SceneGraph.OpenGLEventArgs"/> instance containing the event data.</param>
        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            //  TODO: Initialise OpenGL here.

            //  Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;

            //  Set the clear color.
            gl.ClearColor(1, 1, 1, 1);
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
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        /// <summary>
        /// The current rotation.
        /// </summary>
        private float rotation = 0.0f;

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


                double x = e.GetPosition(openGLControl).X;
                double y = e.GetPosition(openGLControl).Y;
                x = x * 2 / openGLControl.ActualWidth;
                y = y * 2 / openGLControl.ActualHeight;
                x -= 1;
                y -= 1;
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
