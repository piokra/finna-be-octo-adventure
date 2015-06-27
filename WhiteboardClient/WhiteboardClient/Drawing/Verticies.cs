using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhiteboardClient.Drawing
{
    public class Verticies
    {
        /// <summary>
        /// A helper class that holds verticies
        /// </summary>
        /// <param name="verticies">Array that contains verticies must be of at least stride*count+coordspervertex-1 big</param>
        /// <param name="coordspervertex">Number of coordinates per vertex valid: 1,2,3,4</param>
        /// <param name="count">Number of verticies</param>
        /// <param name="stride">Distance between next vertices in an array</param>
        public Verticies(float[] verticies, int coordspervertex, int count, int stride)
        {
            this.verticies = verticies;
            this.coords = coordspervertex;
            this.count = count;
            this.stride = stride;
        }
        public float[] verticies = null;
        public int coords = -1;
        public int stride = -1;
        public int count = -1;
    }
}
