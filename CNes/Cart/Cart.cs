using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CNes.Cartridge
{
    class Cart
    {
        public byte[] header;
        public byte[][] prgRom;
        public byte[][] chrRom;

        public RomMirroring mirroring;
        public TVMode mode = TVMode.NTSC; //Writing a different value to mode during runtime is not allowed; initialization is only allowed during restart
        public Mapper map;

        public Cart(byte[] header, byte[][] prgRom, byte[][] chrRom)
        {
            this.header = header;
            this.prgRom = prgRom;
            this.chrRom = chrRom;
            InitCart();
        }

        public void InitCart()
        {
            byte mirrorBit = (byte)(header[5] & 0x9);
            switch (mirrorBit)
            {
                case 0:
                    mirroring = RomMirroring.HORIZONTAL;
                    break;
                case 1:
                    mirroring = RomMirroring.VERTICAL;
                    break;
                default:
                    mirroring = RomMirroring.FOUR_SCREEN;
                    break;
            }

            byte mapper = (byte)(((header[6] & 0xF0) << 4) | (header[5] & 0xF0));
            switch (mapper)
            {
                case 0:
                    map = Mapper.MAP_0;
                    break;
                default:
                    map = Mapper.MAP_UNKNOWN;
                    break;
            }

            
        }

        public enum RomMirroring
        {
            HORIZONTAL, VERTICAL, FOUR_SCREEN
        }

        public enum TVMode
        {
            PAL, NTSC
        }

        public enum Mapper
        {
            MAP_0, MAP_UNKNOWN
        }
    }
}
