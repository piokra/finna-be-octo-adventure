using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANNWF.Utill
{
    public class Texture
    {
        public enum TextureFormat
	    {
	         RGB = 3,
            RGBA = 4,

	    }
        private TextureFormat mFormat = TextureFormat.RGB;
        private int mWidth = -1;
        private int mHeight = -1;
        private float[] mPixels = null;
        public Texture(int width, int height, TextureFormat format, float[] pixels)
        {
            mWidth = width;
            mHeight = height;
            mFormat = format;
            try
            {
                updatePixels(pixels);
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public void updatePixels(float[] pixels)
        {
            int vpp = (int)mFormat;
            if (pixels.Length < vpp * mWidth * mHeight)
                throw new Exception("Invalid pixel size");
            mPixels = new float[mWidth * mHeight * vpp];
            pixels.CopyTo(mPixels, 0);
        }

        public int getHeight()
        {
            return mHeight;
        }
        public int getWidth()
        {
            return mWidth;
        }
        public float[] getPixels()
        {
            return mPixels;
        }
        public TextureFormat getTextureFormat()
        {
            return mFormat;
        }
    }
}
