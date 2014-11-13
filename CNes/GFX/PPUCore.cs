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
        int cur_scanline = 0;
        long ticks = 0; //Might integrate this into something else later?
        ushort cur_back_offset = 0x0;
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
            //Load both shift registers with pattern data
            sa_reg = Read8(cur_back_offset); 
            sb_reg = Read8((ushort)(cur_back_offset + 0x8));
            cur_back_offset += 0x8;

            byte[] finRender = new byte[8]; //VALUE 8 IS TEMPORARY (DRAWING A TILE FIRST);
            for (int renderI = 0; renderI < 8; renderI++)
            {
                finRender[renderI] = (byte)(((sa_reg & 1) << 1) | (sb_reg & 1));
                sa_reg >>= 1; sb_reg >>= 1;
                if (renderI % 8 == 0) //Reload the shift registers every 8 cycles, kind of slow (maybe implement some kind of loop unrolling?)
                {
                    sa_reg = Read8(cur_back_offset);
                    sb_reg = Read8((ushort)(cur_back_offset + 0x8));
                }
            }
            #endregion
        }
        public void DoPPUCycle()
        {

        }
        #region PPU Memory accesses (all takes 2 ticks)
        public byte Read8(ushort ppuAddr)
        {
            byte ret = 0x0;
            if (ppuAddr < 0x1000)
            {
                ret = chrRom[0][ppuAddr];
            }
            else if (ppuAddr < 0x2000)
            {
                ret = chrRom[1][ppuAddr - 0x1000];
            }
            else if (ppuAddr < 0x3000)
            {
                ret = chrRom[2][ppuAddr - 0x2000];
            }
            ticks += 2;
            return ret;
        }
        #endregion
    }
}
