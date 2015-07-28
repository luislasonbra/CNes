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

        public unsafe Image Render(byte[] input, ushort curScanline) //Byte array parameter is the input array for the scanline being passed to the renderer
        {

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
