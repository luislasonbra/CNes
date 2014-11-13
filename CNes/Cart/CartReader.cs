using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace CNes.Cartridge
{
    class CartReader
    {
        private string nl = Environment.NewLine;

        public Cart ReadCart(string fileName)
        {
            int i = 0;
            FileStream fs = File.OpenRead(fileName);

            byte[] header = new byte[16];
            byte[] wholeFile = new byte[fs.Length];
            fs.Read(header, 0, 16);

            if (ASCIIEncoding.ASCII.GetString(header, 0, 4).Equals("NES")) { Console.WriteLine("ROM is valid!" + nl, 1); } else { throw new Exception("Invalid ROM!"); }

            byte prgRoms = header[4];
            int prgRoms4k = prgRoms << 2; //Just faster multiplication :P
            byte chrRoms = header[5];
            int chrRoms1k = chrRoms << 3;
            byte[][] prgRom = new byte[prgRoms4k][];
            byte[][] chrRom = new byte[chrRoms1k][];

            for (i = 0; i < prgRoms4k; i++)
            {
                prgRom[i] = new byte[4096];
                fs.Read(prgRom[i], 0, 4096); //This kind of read can Happen because fs.Read() advances the position by 3rd argument bytes every time
            }

            for (i = 0; i < chrRoms1k; i++)
            {
                chrRom[i] = new byte[1024];
                fs.Read(chrRom[i], 0, 1024);
            }

            Cart cr = new Cart(header, prgRom, chrRom);
            return cr;
        }
    }
}
