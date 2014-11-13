using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CNes.Cartridge;

namespace CNes.Core
{
    class NESCore
    {
        byte[][] systemRAM = new byte[4][];
        byte[][] prgRom;
        byte[] ppuRegs = new byte[8];
        byte[] header;

        public NESCore(Cart c)
        {
            prgRom = c.prgRom;
            header = c.header;
            systemRAM[0] = new byte[2048];
            systemRAM[1] = new byte[2048];
            systemRAM[2] = new byte[2048];
            systemRAM[3] = new byte[2048];
        }
        #region 2KB System RAM (0x0-0x800, mirrored until 0x2000)
        public byte Read8(ushort address)
        {
            if (address < 0x800)
            {
                return systemRAM[0][address];
            }
            else if (address < 0x1000)
            {
                return systemRAM[1][address - 0x800];
            }
            else if (address < 0x1800)
            {
                return systemRAM[2][address - 0x1000];
            }
            else if (address < 0x2000)
            {
                return systemRAM[3][address - 0x1800];
            }
            else if (address < 0x2008)
            {
                return ppuRegs[address - 0x2000];
            }
            else if (address >= 0x8000)
            {
                return ReadPrg8(address);
            }
            throw new Exception("Illegal memory access: you didn't read an address within writeable range");
        }

        public ushort Read16(ushort address)
        {
            byte mem1 = Read8(address);
            byte mem2 = Read8((ushort)(address + 1));
            return (ushort)((mem2 << 8) | mem1);
        }

        public void Write8(ushort address, byte data)
        {
            //
            // TODO: Implement PPU memory-mapped registers, cuz that causes page boundary errors
            //
            if (address < 0x800)
            {
                systemRAM[0][address] = data; return;
            }
            else if (address < 0x1000)
            {
                systemRAM[1][address - 0x800] = data; return;
            }
            else if (address < 0x1800)
            {
                systemRAM[2][address - 0x1000] = data; return;
            }
            else if (address < 0x2000)
            {
                systemRAM[3][address - 0x1800] = data; return;
            }
            else if (address < 0x2008) //Writing to PPU registers
            {
                ppuRegs[address - 0x2000] = data; return;
            }
            throw new Exception("Illegal memory access: you didn't write to an address within writeable range");
        }
#endregion
        private byte ReadPrg8(ushort address)
        {

            if (address < 0x9000) 
            {
                return prgRom[0][address - 0x8000]; //7FFF
            }
            else if (address < 0xA000) 
            {
                return prgRom[1][address - 0x9000]; //8FFF
            }
            else if (address < 0xB000)
            {
                return prgRom[2][address - 0xA000]; //9FFF
            }
            else if (address < 0xC000)
            {
                return prgRom[3][address - 0xB000]; //AFFF
            }
            else if (address < 0xD000)
            {
                return prgRom[4][address - 0xC000]; //BFFF
            }
            else if (address < 0xE000)
            {
                return prgRom[5][address - 0xD000]; //CFFF
            }
            else if (address < 0xF000)
            {
                return prgRom[6][address - 0xE000]; //DFFF
            }
            else if (address >= 0xF000)
            {
                return prgRom[7][address - 0xF000]; //EFFF
            }
            else
            {
                throw new Exception("Illegal page access: you accessed PRG ROM outside its addressable space");
            }
        }

        public ushort GetResetAddr()
        {
            return (ushort)(ReadPrg8(0xFFFD) << 8 + ReadPrg8(0xFFFC));
        }
    }
}
