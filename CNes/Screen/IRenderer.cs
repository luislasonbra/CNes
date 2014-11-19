using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CNes.Screen
{
    interface IRenderer
    {
        //Class to define any NES renderer
        void InitRenderer();
        Image Render(byte[] input);
    }
}
