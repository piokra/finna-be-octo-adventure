using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhiteboardClient.Drawing
{
    public class Vertex
    {
        public Vertex()
        {

        }
        public Vertex(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public float x, y, z, w;
    };
}
