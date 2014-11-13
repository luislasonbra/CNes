using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using CNes.Core;
using CNes.Cartridge;

namespace CNes.Screen.Renderers
{
    //Just NTSC for now
    //Draw bitmaps to a control (Probably a PictureBox, but if this is too slow, I'll come up with something new)
    class BMP16Renderer : NESRenderer
    {
        bool canRender;
        bool isRendering = false;

        Bitmap bmp;
        Graphics g;
        Cart c;

        int height;
        const int width = 256;

        public BMP16Renderer(Cart c)
        {
            this.c = c;
            InitRenderer();
        }

        public Image Render(byte[] input) //Byte array parameter is the input array for the scanline being passed to the renderer
        {
            BitmapData bData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr ptr = bData.Scan0;
            Marshal.Copy(input, 0, ptr, height * width * 3);
            bmp.UnlockBits(bData);
            //g.DrawRectangle(pen, rect); //I think we're manipulating a Graphics instance to draw on a bitmap, so we'll return that bitmap

            return bmp; //Returns the image to be used in the picturebox???????
        }

        public void InitRenderer()
        {
            switch (c.mode)
            {
                case Cart.TVMode.NTSC:
                    bmp = new Bitmap(width, 224);
                    height = 224;
                    break;
                case Cart.TVMode.PAL:
                    bmp = new Bitmap(width, 240);
                    height = 240;
                    break;
            }
            g = Graphics.FromImage(bmp);
        }
    }
}
