using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.VertexBuffers;
namespace WhiteboardClient.Drawing
{
    public class Drawable : IDisposable
    {
        public Drawable(Verticies vertices, ShaderProgram program, OpenGL gl )
        {
            setGL(gl);
            mVertices = vertices;
            setProgram(program);
            //constructVertexArray(vertices, gl);
        }
        Verticies mVertices = null;
        OpenGL mGL = null;
        public void Dispose()
        {
            mVertexBufferArray.Delete(mGL);
        }
        VertexBufferArray mVertexBufferArray = null;
        int mVertexCount = 0; 
        void setGL(OpenGL gl)
        {
            mGL = gl;
        }
        bool mCompiled = false;
        void constructVertexArray(Verticies vertices, OpenGL gl)
        {
            mVertexCount = vertices.count;
            mVertexBufferArray = new VertexBufferArray();
            mVertexBufferArray.Create(gl);
            mVertexBufferArray.Bind(gl);
            var vertexDataBuffer = new VertexBuffer();
            vertexDataBuffer.Create(gl);
            vertexDataBuffer.Bind(gl);
            vertexDataBuffer.SetData(gl, 0, vertices.verticies, false, vertices.stride);
            vertexDataBuffer.Unbind(gl);
            mVertexBufferArray.Unbind(gl);
            
        }
        ShaderProgram mShaderProgram = null;
        void setProgram(ShaderProgram program)
        {
            mShaderProgram = program;
        }

        public void draw(OpenGL gl)
        {
            if(!mCompiled)
            {
                mCompiled = true;
                constructVertexArray(mVertices, mGL);
            }
            mShaderProgram.Bind(gl);


            //  Bind the out vertex array.
            mVertexBufferArray.Bind(gl);

            //  Draw the square.
            gl.DrawArrays(OpenGL.GL_POINTS, 0, mVertexCount-1);

            //  Unbind our vertex array and shader.
            mVertexBufferArray.Unbind(gl);
            mShaderProgram.Unbind(gl);
        }
    }
}
