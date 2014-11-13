using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CNes.Core;
using CNes.Cartridge;

namespace CNes.GFX
{
    /* PPU NOTES
     * The PPU has its own 16KB address space, in the form of:
     * 2KB to store a map. (what is this? are these nametables/pattern tables?)
     * 8KB sprite data and background data (collectively the CHR-ROM section of the cart)
     * The name-tables serve a purpose of pointing to tiles stored in the pattern table
     * (They hold the tile data [location] of a tile in the pattern table)
     * 
     * Don't really know what the shift registers are, but I guess I'll have to do some more research
     * 
     * SPRITE RENDERING DATA IS STORED IN               PPU OAM!!
     * BACKGROUND RENDERING DATA IS STORED IN(kinda)    NAMETABLES!!
     * TILES ARE 8x8 AND HAVE 2 BITS PER PIXEL! (These bits are fed into shift registers)
     */
    class PPUCore
    {
        byte[][] chrRom;
        short current_scanline = 0;
        ushort cur_pat_offset = 0x0;
        ushort sa_reg = 0x0; //Pattern shift register A
        ushort sb_reg = 0x0; //Pattern shfit register B
        byte lp_reg = 0x0; //Lower palette 8-bit shift register
        byte hp_reg = 0x0; //High palette 8-bit shift register
        public PPUCore(Cart c)
        {
            this.chrRom = c.chrRom;
        }
        
        //I guess this will be our main ppu-control function, so we'll stick with it
        public void RenderScanline()
        {
            #region TEMPORARY SOLUTION, FIX LATER
            sa_reg = Read8(cur_pat_offset); //Load both shift registers with pattern data
            sb_reg = Read8((ushort)(cur_pat_offset + 0x8));

            #endregion
        }
        #region PPU Memory accesses
        public byte Read8(ushort ppuAddr)
        {
            if (ppuAddr < 0x1000)
            {
                return chrRom[0][ppuAddr];
            }
            else if (ppuAddr < 0x2000)
            {
                return chrRom[1][ppuAddr - 0x1000];
            }
            return 0;
        }
        #endregion
    }
}
