using ANNWF.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNWF.Utill
{
    public static class TextureUtills
    {
        // x % r, r>0
        private static double doubleMod(double x, double r)
        {
            x += System.Math.Ceiling(System.Math.Abs(x / r)) * r;
            x -= System.Math.Floor(x / r) * r;
            return x;
        }
        public static Texture fromMatrix(Matrix m)
        {
            
            int width = m.getWidth();
            int height = m.getHeight();
            float[] pixels = new float[width * height * 3];
            int length = width * height;
            for (int i = 0; i < length; i++)
            {
                double v = m.get(i/width,i%width);

                double r = doubleMod(v, 1);
                double g = doubleMod(v, 10) / 10.0;
                double b = doubleMod(v,100) / 100.0;
                pixels[3*i] = (float)r;
                pixels[3*i+1] = (float)g;
                pixels[3*i+2] = (float)b;

               
            }

            return new Texture(width, height, Texture.TextureFormat.RGB, pixels);
        }
    }
}
