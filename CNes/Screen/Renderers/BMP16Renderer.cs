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
    //Just NTSC for now.
    //Draw bitmaps to a control (Probably a PictureBox, but if this is too slow, I'll come up with something new)
    class BMP16Renderer : IRenderer
    {
        bool canRender;
        bool isRendering = false;

        Bitmap bmp;
        Cart c;

        int height;
        const int width = 256;

        public BMP16Renderer(Cart c)
        {
            this.c = c;
            InitRenderer();
        }

        public Image Render(byte[] input, ushort curScanline) //Byte array parameter is the input array for the scanline being passed to the renderer
        {
            //Create better renderer later, but now we're just going to use some ghetto monochrome half-assed custom renderer.
            for (int l = 0; l < input.Length; l++) //I have reasons to not use a for-each
            {
                if (input[l] != 0)
                {
                    //This assumes scanlines go vertically, but maybe they go horizontally (would be a very easy fix)
                    Console.WriteLine("l: " + l + " output of logic: " + (l % 256 == 0 && l != 0 ? l / 256 : (l - (l % 256)) / 256));
                    bmp.SetPixel(curScanline, l % 256 == 0 && l != 0 ? l / 256 : (l - (l % 256)) / 256, Color.Black);
                }
            }
            /* TEST */ Console.WriteLine(257 / 256);

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
        }
    }
}
