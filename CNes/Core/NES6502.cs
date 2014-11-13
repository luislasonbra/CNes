using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CNes.Cartridge;

//Copyright (c) 2014 Alex Restifo
//This class emulates the NES's MOS 6502 CPU in a cycle-accurate manner.
namespace CNes.Core
{
    class NES6502
    {
        Cart cart;
        NESCore nes;

        //General purpose registers. TODO: Remove "public" modifier! Those are just for testing! (ornah)
        public byte a_reg = 0x0;
        public byte x_reg = 0x0;
        public byte y_reg = 0x0;
        //Pointer registers
        public byte sp_reg = 0xFF;
        public ushort pc_reg = 0x0;
        public byte current_op = 0x0;
        //Status bytes
        public byte c_flag = 0x0;
        public byte z_flag = 0x0;
        public byte i_flag = 0x0;
        public byte d_flag = 0x0;
        public byte b_flag = 0x0;
        public byte v_flag = 0x0;
        public byte n_flag = 0x0;

        //Status of program and misc. variables
        public bool isRunning = false;
        public bool interrupt = false;

        byte lenCycles = 0;
        byte lenBytes = 0;
        byte tempData = 0x0;
        ushort tempAddr = 0x0;
        public ulong cycles = 0;
        public ulong ticks = 0; //the PPU is going to utilize this, as NTSC PPU's have 3 PPU cycles per 1 CPU cycles

        public NES6502(Cart cart, NESCore nes)
        {
            this.cart = cart;
            this.nes = nes;
        }
        #region Push/pop (Stack operations)
        //YAY I FINISHED IT
        public void Push8(byte data)
        {
            if (sp_reg < 0x01)
                throw new StackOverflowException("Stack overflow! Implement stack dump later...");

            nes.Write8((ushort)(sp_reg), data);
            sp_reg -= 1;
            
        }
        public void Push16(ushort data)
        {
            if (sp_reg < 0x02)
                throw new StackOverflowException("Stack overflow! Implement stack dump later...");
            
            byte mem1 = (byte)(data & 0xFF); //This is the LSB, I think
            byte mem2 = (byte)((data & 0xFF00) >> 8); //This is the MSB, I think

            //These variables in each Write8 (the variable mem1&2) might have to be switched due to endianness, I'm still learning :P
            nes.Write8(sp_reg, mem1);
            nes.Write8((ushort)(sp_reg - 1), mem2);
            sp_reg -= 2;
        }
        public byte Pop8()
        {
            sp_reg += 1;
            return nes.Read8(sp_reg);
        }
        public ushort Pop16()
        {
            byte mem1 = nes.Read8((ushort)(sp_reg + 1));
            byte mem2 = nes.Read8((ushort)(sp_reg + 2));

            return SwitchEndian(mem2, mem1); //This is SUPER confusing, but it works :P (SORRY DEVELOPERS!!!!!)
        }
        public void PopStatus()
        {
            byte pop = Pop8();
            z_flag = (byte)(pop & 0x1);
            c_flag = (byte)((pop & 0x2) >> 1);
            i_flag = (byte)((pop & 0x4) >> 2);
            d_flag = (byte)((pop & 0x8) >> 3);
            b_flag = (byte)((pop & 0x10) >> 4);
            v_flag = (byte)((pop & 0x40) >> 6);
            n_flag = (byte)((pop & 0x80) >> 7);
            //NV-BDICZ
        }
        public void PushStatus()
        {
            byte result = 0x20;
            result |= z_flag;
            result |= (byte)(c_flag << 1);
            result |= (byte)(i_flag << 2);
            result |= (byte)(d_flag << 3);
            result |= (byte)(b_flag << 4);
            result |= (byte)(v_flag << 6);
            result |= (byte)(n_flag << 7);
            Push8(result);
        }
        #endregion
        #region Get address using specified addressing mode
        public byte ZeroPageAddr(ushort address)
        {
            return (byte)(address & 0xFF);
        }
        public byte ZeroPageXAddr(ushort address)
        {
            return (byte)((address + x_reg) & 0xFF);
        }
        public byte ZeroPageYAddr(ushort address)
        {
            return (byte)((address + y_reg) & 0xFF);
        }
        public ushort AbsoluteAddr(ushort address)
        {
            return address;
        }
        public ushort AbsoluteXAddr(ushort address)
        {
            return (ushort)(address + x_reg);
        }
        public ushort AbsoluteYAddr(ushort address)
        {
            return (ushort)(address + y_reg);
        }
        public ushort IndirectAddr(ushort address)
        {
            return nes.Read16(address);
        }
        public ushort IndexedIndirectAddr(ushort address) //Only returns the LSB, maybe this is an error? EDIT: NOPENOPE FIXED (keep these comments for nostalgia's sake, remove them in the final release :P
        {
            return nes.Read16((ushort)((address + x_reg) & 0xFF));
        }
        public ushort IndirectIndexedAddr(ushort address)
        {
            return (ushort)(nes.Read16(address) + y_reg);
        }
        #endregion
        #region Actual opcode handling
        //Started this on 9/6/2014
        public void ADC() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            byte tempV_flag = 0x0;
            ushort tempResult = 0x0;
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x69:
                    tempData = memArg1;
                    break; 
                case 0x65:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    break; 
                case 0x75:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    break; 
                case 0x6D:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    break; 
                case 0x7D:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break; 
                case 0x79:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0x61:
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break; 
                case 0x71:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
            }
            tempV_flag = (byte)((tempData & 0x80) >> 7);
            tempResult = (ushort)(a_reg + tempData + c_flag); ResetFlags();
            ResetFlags();

            if (tempResult > 0xFF)
            {
                tempResult -= 0x100;
                c_flag = 1;
            }

            if ((tempResult & 0x80) >> 7 != tempV_flag)
                v_flag = 1;

            if ((tempResult & 0x80) == 0x80)
                n_flag = 1;

            if (tempResult == 0)
                z_flag = 1;

            a_reg = (byte)tempResult;
        }
        public void AND() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 1));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x29:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break;
                case 0x25:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0x35:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 4;
                    break;
                case 0x2D:
                    tempAddr = AbsoluteAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x3D:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x39:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x21:
                    tempAddr = IndexedIndirectAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x31:
                    tempAddr = IndirectIndexedAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break;
            }

            a_reg &= tempData;
            ResetFlags();

            if (a_reg == 0)
                z_flag = 1;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void ASL() 
        {
            tempAddr = 0x0;
            tempData = 0x0;

            ushort mem1Addr = (ushort)(pc_reg + 1);
            switch (current_op)
            {
                case 0x0A:
                    tempData = a_reg;
                    break;
                case 0x06:
                    tempAddr = ZeroPageAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0x16:
                    tempAddr = ZeroPageXAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0x0E:
                    tempAddr = AbsoluteAddr(mem1Addr); //Seems to work :P
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0x1E:
                    tempAddr = AbsoluteXAddr(mem1Addr); //Looks like it works
                    tempData = nes.Read8(tempAddr);
                    break;
            }

            //set carry bit to orginal (before shift) bit 7
            ResetFlags();
            c_flag = (byte)((tempData & 0x80) >> 7);
            tempData <<= 1;

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            switch (current_op)
            {
                case 0x0A:
                    a_reg = tempData;
                    cycles += 2; pc_reg += 1;
                    break;
                case 0x06:
                    nes.Write8(tempAddr, tempData);
                    cycles += 5; pc_reg += 2;
                    break;
                case 0x16:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6; pc_reg += 2;
                    break;
                case 0x0E:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6; pc_reg += 3;
                    break;
                case 0x1E:
                    nes.Write8(tempAddr, tempData);
                    cycles += 7; pc_reg += 3;
                    break;
            }
        }
        //Optimization is definately possible with the carries (check the flag's status before defining vars)
        public void BCC()
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (c_flag == 0)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BCS() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (c_flag == 1)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BEQ() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (z_flag == 1)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BIT() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            byte tempResult = 0x0;
            ushort fullData = SwitchEndian(memArg1, memArg2);
            switch (current_op)
            {
                case 0x24:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0x2C:
                    tempData = nes.Read8(fullData);
                    break;
            }
            //In BIT, the a_reg acts as a mask for an AND to be performed on a memory location
            tempResult = (byte)(a_reg & tempData);
            ResetFlags();

            if (tempResult == 0)
                z_flag = 1;

            n_flag = (byte)((tempData & 0x80) >> 7);
            v_flag = (byte)((tempData & 0x40) >> 6);

        }
        public void BMI() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (n_flag == 1)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BNE() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (z_flag == 0)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BPL() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (n_flag == 0)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }
                
            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BRK() 
        {
            Push16(pc_reg);
            PushStatus();
            pc_reg = nes.Read16(0xFFFE);
            b_flag = 1;

            pc_reg += 1; //I might not need this, since it already sets the PC, but I'll confirm this experimentally
            cycles += 7;
        }
        public void BVC() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (v_flag == 0)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }
            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void BVS() 
        {
            byte offset = nes.Read8((ushort)(pc_reg + 1));
            bool useNeg = false;
            if (offset > 0x7F)
            {
                offset = (byte)(0xFF - offset);
                useNeg = true;
            }

            if (v_flag == 1)
            {
                if (useNeg)
                {
                    pc_reg -= offset;
                    cycles += 1;
                }
                else
                {
                    pc_reg += offset;
                    cycles += 1;
                }

            }

            ResetFlags(); //Maybe this is unnescessary...
            pc_reg += 1; cycles += 2;
        }
        public void CLC() 
        {
            c_flag = 0;
            cycles += 2; pc_reg += 1;
        }
        public void CLD() 
        {
            d_flag = 0;
            cycles += 2; pc_reg += 1;
        }
        public void CLI() 
        {
            i_flag = 0;
            cycles += 2; pc_reg += 1;
        }
        public void CLV()
        {
            v_flag = 0;
            cycles += 2; pc_reg += 1;
        }
        /*
         **********************************************************************************************************************************  
         * I fubar'd the CMP, CPX, and CPY opcodes D: !! FIX PL0X (I think I fixed it maybe...)
         ********************************************************************************************************************************** 
         */
        public void CMP() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0xC9:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break;
                case 0xC5:
                    tempAddr = ZeroPageAddr(memArg1); 
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0xD5:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 4;
                    break;
                case 0xCD:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0xDD:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0xD9:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0xC1: 
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0xD1:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break;
            }
            byte cmpResult = 0;
            ResetFlags();

            if (a_reg - tempData > 0)
            {
                cmpResult = (byte)(a_reg - tempData);
            }

            if (cmpResult >= tempData)
                c_flag = 1;

            if (cmpResult == tempData)
                z_flag = 1;

            if ((cmpResult & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void CPX() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0xE0:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break;
                case 0xE4:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0xEC:
                    tempAddr = fullData; //This is terrible code, fix it (and many others that make the same mistake)
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
            }
            byte cmpResult = 0;
            ResetFlags();

            if (x_reg - tempData > 0)
            {
                cmpResult = (byte)(x_reg - tempData);
            }

            if (cmpResult >= tempData)
                c_flag = 1;

            if (cmpResult == tempData)
                z_flag = 1;

            if ((cmpResult & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void CPY() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0xC0:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break;
                case 0xC4:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0xCC:
                    tempAddr = fullData; //This is terrible code, fix it (and many others that make the same mistake)
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
            }
            byte cmpResult = 0;
            ResetFlags();

            if (y_reg - tempData > 0)
            {
                cmpResult = (byte)(y_reg - tempData);
            }

            if (cmpResult >= tempData)
                c_flag = 1;

            if (cmpResult == tempData)
                z_flag = 1;

            if ((cmpResult & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void DEC()
        {
            ushort mem1Addr = (ushort)(pc_reg + 1);
            tempAddr = 0x0;
            tempData = 0x0;
            switch (current_op)
            {
                case 0xC6:
                    tempAddr = ZeroPageAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xD6:
                    tempAddr = ZeroPageXAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xCE:
                    tempAddr = AbsoluteAddr(mem1Addr); //Seems to work :P
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xEE:
                    tempAddr = AbsoluteXAddr(mem1Addr); //Also seems to work :P
                    tempData = nes.Read8(tempAddr);
                    break;
                default:
                    throw new Exception("DEC instruction is broken!!11!!!!111!!!");
            }

            tempData -= 1;
            ResetFlags();

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            if (tempData == 0x0)
                z_flag = 1;

            switch (current_op)
            {
                case 0xC6:
                    nes.Write8(tempAddr, tempData);
                    cycles += 5;
                    pc_reg += 2;
                    break;
                case 0xD6:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6;
                    pc_reg += 2;
                    break;
                case 0xCE:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6;
                    pc_reg += 3;
                    break;
                case 0xDE:
                    nes.Write8(tempAddr, tempData);
                    cycles += 7;
                    pc_reg += 3;
                    break;
            }
        }
        public void DEX() 
        {
            x_reg -= 1;
            ResetFlags();

            if (x_reg == 0)
                z_flag = 1;

            if ((x_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void DEY() 
        {
            y_reg -= 1;
            ResetFlags();

            if (y_reg == 0)
                z_flag = 1;

            if ((y_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void EOR() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x49:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break; 
                case 0x45:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break; 
                case 0x55:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 4;
                    break; 
                case 0x4D:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x5D:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break; 
                case 0x59:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break; 
                case 0x41:
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x51:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break; 
            }

            a_reg ^= tempData;
            ResetFlags();

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            if (a_reg == 0x0)
                z_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void INC() 
        {
            ushort mem1Addr = (ushort)(pc_reg + 1);
            tempAddr = 0x0;
            tempData = 0x0;
            switch (current_op)
            {
                case 0xE6:
                    tempAddr = ZeroPageAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xF6:
                    tempAddr = ZeroPageXAddr(mem1Addr); //Works
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xEE:
                    tempAddr = AbsoluteAddr(mem1Addr); //Seems to work :P
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xFE:
                    tempAddr = AbsoluteXAddr(mem1Addr); //Also seems to work :P
                    tempData = nes.Read8(tempAddr);
                    break;
                default:
                    throw new Exception("INC instruction is broken!!11!!!!111!!!");
            }

            tempData += 1;
            ResetFlags();

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            if (tempData == 0x0)
                z_flag = 1;

            switch (current_op)
            {
                case 0xE6:
                    nes.Write8(tempAddr, tempData);
                    cycles += 5;
                    pc_reg += 2;
                    break;
                case 0xF6:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6;
                    pc_reg += 2;
                    break;
                case 0xEE:
                    nes.Write8(tempAddr, tempData);
                    cycles += 6;
                    pc_reg += 3;
                    break;
                case 0xFE:
                    nes.Write8(tempAddr, tempData);
                    cycles += 7;
                    pc_reg += 3;
                    break;
            }
        }
        public void INX()
        {
            x_reg += 1;
            ResetFlags();

            if (x_reg == 0)
                z_flag = 1;

            if ((x_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void INY() 
        {
            y_reg += 1;
            ResetFlags();

            if (y_reg == 0)
                z_flag = 1;

            if ((y_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void JMP()
        {
            byte arg1Data = nes.Read8((ushort)(pc_reg + 1));
            byte arg2Data = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(arg1Data, arg2Data);
            switch (current_op)
            {
                case 0x4C:
                    pc_reg = AbsoluteAddr(fullData);
                    lenCycles = 3;
                    break;
                case 0x6C:
                    pc_reg = IndirectAddr(fullData);
                    lenCycles = 5;
                    break;
            }

            cycles += lenCycles;
        }
        public void JSR() 
        {
            ushort fullData = SwitchEndian(nes.Read8((ushort)(pc_reg + 1)), nes.Read8((ushort)(pc_reg + 2))); //Unconventional, but I want to save on space (line #'s)
            Push16((ushort)(pc_reg + 2)); //I think, since the JSR is 3 bytes, that the return address needs to be 2 bytes ahead to execute the next instruction after the RTS........

            pc_reg = fullData;
            cycles += 6;
        }
        public void LDA()
        {
            tempData = 0x0;
            tempAddr = 0x0;

            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);
            switch (current_op)
            {
                case 0xA9:
                    tempData = memArg1;
                    lenCycles = 2;
                    lenBytes = 2;
                    break;
                case 0xA5:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 3; 
                    lenBytes = 2;
                    break;
                case 0xB5:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 4; 
                    lenBytes = 2;
                    break;
                case 0xAD: 
                    tempData = nes.Read8(fullData);
                    lenCycles = 4; 
                    lenBytes = 3;
                    //MessageBox.Show("Temp Addr: 0x" + Convert.ToString(tempAddr, 16) + Environment.NewLine + "Data: 0x" + Convert.ToString(tempData, 16) + Environment.NewLine + "FullData: 0x" + Convert.ToString(fullData, 16));
                    break;
                case 0xBD:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 4; 
                    lenBytes = 3;
                    break;
                case 0xB9:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 4; 
                    lenBytes = 3;
                    break;
                case 0xA1:
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 6; 
                    lenBytes = 2;
                    break;
                case 0xB1:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenCycles = 5;
                    lenBytes = 2;
                    break;
            }

            a_reg = tempData;
            ResetFlags();

            if (a_reg == 0x0)
                z_flag = 1;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void LDX() 
        {
            ushort arg1Addr = (ushort)(pc_reg + 1);
            
            switch (current_op)
            {
                case 0xA2:
                    tempData = nes.Read8((ushort)(pc_reg + 1));
                    lenBytes = 2;
                    lenCycles = 2;
                    break;
                case 0xA6:
                    tempData = nes.Read8(ZeroPageAddr(arg1Addr));
                    lenBytes = 2;
                    lenCycles = 3;
                    break;
                case 0xB6:
                    tempData = nes.Read8(ZeroPageYAddr(arg1Addr));
                    lenBytes = 2;
                    lenCycles = 4;
                    break;
                case 0xAE:
                    tempData = nes.Read8(AbsoluteAddr(arg1Addr));
                    lenBytes = 3;
                    lenCycles = 4;
                    break;
                case 0xBE:
                    tempData = nes.Read8(AbsoluteYAddr(arg1Addr));
                    lenBytes = 3;
                    lenCycles = 4;
                    break;
            }

            x_reg = tempData;
            ResetFlags();

            if (x_reg == 0)
                z_flag = 1;

            if ((y_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void LDY() 
        {
            byte dataAtAddr = 0x0;
            ushort arg1Addr = (ushort)(pc_reg + 1);

            switch (current_op)
            {
                case 0xA0:
                    dataAtAddr = nes.Read8((ushort)(pc_reg + 1));
                    lenBytes = 2;
                    lenCycles = 2;
                    break;
                case 0xA4:
                    dataAtAddr = nes.Read8(ZeroPageAddr(arg1Addr));
                    lenBytes = 2;
                    lenCycles = 3;
                    break;
                case 0xB4:
                    dataAtAddr = nes.Read8(ZeroPageXAddr(arg1Addr));
                    lenBytes = 2;
                    lenCycles = 4;
                    break;
                case 0xAC:
                    dataAtAddr = nes.Read8(AbsoluteAddr(arg1Addr));
                    lenBytes = 3;
                    lenCycles = 4;
                    break;
                case 0xBC:
                    dataAtAddr = nes.Read8(AbsoluteXAddr(arg1Addr));
                    lenBytes = 3;
                    lenCycles = 4;
                    break;
            }

            x_reg = dataAtAddr;

            if (x_reg == 0)
                z_flag = 1;

            if ((y_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void LSR() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x4A:
                    tempData = a_reg;
                    lenBytes = 1; lenCycles = 2;
                    break;
                case 0x46:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break;
                case 0x56:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break; 
                case 0x4E:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 6;
                    break;
                case 0x5E:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 7;
                    break; 
            }

            c_flag = (byte)(tempData & 0x1);
            tempData >>= 1;

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            if (tempData == 0x0)
                z_flag = 1;

            switch (current_op)
            {
                case 0x4A:
                    a_reg = tempData;
                    break;
                case 0x46:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x56:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x4E:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x5E:
                    nes.Write8(tempAddr, tempData);
                    break; 
            }

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void NOP() 
        {
            //Do nothing....
            pc_reg += 1;
            cycles += 2;
        }
        public void ORA() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x09:
                    tempData = memArg1;
                    lenBytes = 2; lenCycles = 2;
                    break;
                case 0x05:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0x15:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 4;
                    break;
                case 0x0D:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x1D:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x19:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x01:
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x11:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break;
            }
            a_reg |= tempData;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            if (a_reg == 0x0)
                z_flag = 1;

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void PHA() 
        {
            Push8(a_reg);
            pc_reg += 1;
            cycles += 3;
        }
        public void PHP() 
        {
            PushStatus();

            pc_reg += 1;
            cycles += 3;
        }
        public void PLA()
        {
            a_reg = Pop8();

            if (a_reg == 0)
                z_flag = 1;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 4;
        }
        public void PLP() 
        {
            PopStatus();

            pc_reg += 1;
            cycles += 4;
        }
        public void ROL() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            byte newCFlag = 0x0;
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x2A:
                    tempData = a_reg;
                    lenBytes = 1; lenCycles = 2;
                    break;
                case 0x26:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break; 
                case 0x36:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x2E:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 6;
                    break;
                case 0x3E:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 7;
                    break;
            }
            newCFlag = (byte)((tempData & 0x80) >> 7);
            tempData = (byte)((tempData << 1) | c_flag);
            ResetFlags();

            if (tempData == 0)
                z_flag = 0;

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            c_flag = newCFlag;

            switch (current_op)
            {
                case 0x2A:
                    a_reg = tempData;
                    break;
                case 0x26:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x36:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x2E:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x3E:
                    nes.Write8(tempAddr, tempData);
                    break;
            }

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void ROR() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            byte newCFlag = 0x0;
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x6A:
                    tempData = a_reg;
                    lenBytes = 1; lenCycles = 2;
                    break;
                case 0x66:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 5;
                    break;
                case 0x76:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x6E:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 6;
                    break;
                case 0x7E:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    lenBytes = 3; lenCycles = 7;
                    break;
            }

            newCFlag = (byte)(tempData & 0x1);
            tempData = (byte)((tempData >> 1) | (c_flag << 7));
            ResetFlags();

            c_flag = newCFlag;
            if (tempData == 0)
                z_flag = 0;

            if ((tempData & 0x80) == 0x80)
                n_flag = 1;

            c_flag = newCFlag;

            switch (current_op)
            {
                case 0x6A:
                    a_reg = tempData;
                    break;
                case 0x66:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x76:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x6E:
                    nes.Write8(tempAddr, tempData);
                    break;
                case 0x7E:
                    nes.Write8(tempAddr, tempData);
                    break;
            }

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void RTI() 
        {
            PopStatus();
            pc_reg = Pop16();

            pc_reg += 1;
            cycles += 6;
        }
        public void RTS() 
        {
            pc_reg = Pop16();

            //Maybe increment the program counter?? I don't think so
            cycles += 6;
        }
        public void SBC() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0xE9:
                    tempData = memArg1;
                    break;
                case 0xE5:
                    tempAddr = ZeroPageAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xF5:
                    tempAddr = ZeroPageXAddr(memArg1);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xED:
                    tempAddr = fullData;
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xFD:
                    tempAddr = AbsoluteXAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xF9:
                    tempAddr = AbsoluteYAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xE1:
                    tempAddr = IndexedIndirectAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
                case 0xF1:
                    tempAddr = IndirectIndexedAddr(fullData);
                    tempData = nes.Read8(tempAddr);
                    break;
            }
            //Ugh....... Finish this later....
        }
        public void SEC() 
        {
            c_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void SED() 
        {
            d_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void SEI() 
        {
            i_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void STA() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);
            
            switch (current_op)
            {
                case 0x85:
                    tempAddr = ZeroPageAddr(memArg1);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0x95:
                    tempAddr = ZeroPageXAddr(memArg1);
                    lenBytes = 2; lenCycles = 4;
                    break; 
                case 0x8D:
                    tempAddr = fullData;
                    lenBytes = 3; lenCycles = 4;
                    break;
                case 0x9D:
                    tempAddr = AbsoluteXAddr(fullData);
                    lenBytes = 3; lenCycles = 5;
                    break; 
                case 0x99:
                    tempAddr = AbsoluteYAddr(fullData);
                    lenBytes = 3; lenCycles = 5;
                    break;
                case 0x81:
                    tempAddr = IndexedIndirectAddr(memArg1);
                    lenBytes = 2; lenCycles = 6;
                    break;
                case 0x91:
                    tempAddr = IndirectIndexedAddr(memArg1);
                    lenBytes = 2; lenCycles = 6;
                    break;
            }
            
            nes.Write8(tempAddr, a_reg);

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void STX()
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x86:
                    tempAddr = ZeroPageAddr(memArg1);
                    lenBytes = 2; lenCycles = 3;
                    break;
                case 0x96:
                    tempAddr = ZeroPageYAddr(memArg1);
                    lenBytes = 2; lenCycles = 4;
                    break;
                case 0x8E:
                    tempAddr = fullData;
                    lenBytes = 3; lenCycles = 4;
                    break;
            }

            nes.Write8(tempAddr, x_reg);

            pc_reg += lenBytes;
            cycles += lenCycles;
        }
        public void STY() 
        {
            byte memArg1 = nes.Read8((ushort)(pc_reg + 1));
            byte memArg2 = nes.Read8((ushort)(pc_reg + 2));
            ushort fullData = SwitchEndian(memArg1, memArg2);

            switch (current_op)
            {
                case 0x84:
                    break;
                case 0x94:
                    break;
                case 0x8C:
                    break;
            }
        }
        public void TAX() 
        {
            x_reg = a_reg;
            ResetFlags();

            if (x_reg == 0)
                z_flag = 1;

            if ((x_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void TAY() 
        {
            y_reg = a_reg;
            ResetFlags();

            if (y_reg == 0)
                z_flag = 1;

            if ((y_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void TSX() 
        {
            x_reg = sp_reg;
            ResetFlags();

            if (x_reg == 0)
                z_flag = 1;

            if ((x_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void TXA() 
        {
            a_reg = x_reg;
            ResetFlags();

            if (a_reg == 0)
                z_flag = 1;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        public void TXS() 
        {
            sp_reg = x_reg;

            pc_reg += 1;
            cycles += 2;
        }
        public void TYA() 
        {
            a_reg = y_reg;
            ResetFlags();

            if (a_reg == 0)
                z_flag = 1;

            if ((a_reg & 0x80) == 0x80)
                n_flag = 1;

            pc_reg += 1;
            cycles += 2;
        }
        #endregion
        public void Run()
        {
            isRunning = true;
            while (isRunning && !interrupt)
            {
                DoCycle();
            }
        }

        public void DoStep(int steps)
        {
            for (int i = 0; i < steps; i++)
            {
                DoCycle();
            }
        }

        public void DoCycle()
        {
            current_op = nes.Read8(pc_reg);
            switch (current_op)
            {
                case 0x18: CLC(); break;
                case 0xD8: CLD(); break;
                case 0x58: CLI(); break;
                case 0xB8: CLV(); break;

                case 0xE6: INC(); break;
                case 0xF6: INC(); break;
                case 0xEE: INC(); break;
                case 0xFE: INC(); break;
                case 0xE8: INX(); break;
                case 0xC8: INY(); break;

                case 0x4C: JMP(); break;
                case 0x6C: JMP(); break;

                case 0xEA: NOP(); break;

                case 0xA9: LDA(); break;
                case 0xA5: LDA(); break;
                case 0xB5: LDA(); break;
                case 0xAD: LDA(); break;
                case 0xBD: LDA(); break;
                case 0xB9: LDA(); break;
                case 0xA1: LDA(); break;
                case 0xB1: LDA(); break;

                case 0xA2: LDX(); break;
                case 0xA6: LDX(); break;
                case 0xB6: LDX(); break;
                case 0xAE: LDX(); break;
                case 0xBE: LDX(); break;

                case 0xA0: LDY(); break;
                case 0xA4: LDY(); break;
                case 0xB4: LDY(); break;
                case 0xAC: LDY(); break;
                case 0xBC: LDY(); break;

                case 0x85: STA(); break;
                case 0x95: STA(); break;
                case 0x8D: STA(); break;
                case 0x9D: STA(); break;
                case 0x99: STA(); break;
                case 0x81: STA(); break;
                case 0x91: STA(); break;

                case 0x86: STX(); break;
                case 0x96: STX(); break;
                case 0x8E: STX(); break;

                case 0x84: STY(); break;
                case 0x94: STY(); break;
                case 0x8C: STY(); break;

                case 0xC6: DEC(); break;
                case 0xD6: DEC(); break;
                case 0xCE: DEC(); break;
                case 0xDE: DEC(); break;
                case 0xCA: DEX(); break;
                case 0x88: DEY(); break;

                case 0x0A: ASL(); break;
                case 0x06: ASL(); break;
                case 0x16: ASL(); break;
                case 0x0E: ASL(); break;
                case 0x1E: ASL(); break;
                case 0x4A: LSR(); break;
                case 0x46: LSR(); break;
                case 0x56: LSR(); break;
                case 0x4E: LSR(); break;
                case 0x5E: LSR(); break;
                case 0x2A: ROL(); break;
                case 0x26: ROL(); break;
                case 0x36: ROL(); break;
                case 0x2E: ROL(); break;
                case 0x3E: ROL(); break;
                case 0x6A: ROR(); break;
                case 0x66: ROR(); break;
                case 0x76: ROR(); break;
                case 0x6E: ROR(); break;
                case 0x7E: ROR(); break;

                case 0x69: ADC(); break;
                case 0x65: ADC(); break;
                case 0x75: ADC(); break;
                case 0x6D: ADC(); break;
                case 0x7D: ADC(); break;
                case 0x79: ADC(); break;
                case 0x61: ADC(); break;
                case 0x71: ADC(); break;
                case 0xE9: SBC(); break;
                case 0xE5: SBC(); break;
                case 0xF5: SBC(); break;
                case 0xED: SBC(); break;
                case 0xFD: SBC(); break;
                case 0xF9: SBC(); break;
                case 0xE1: SBC(); break;
                case 0xF1: SBC(); break;

                case 0x29: AND(); break;
                case 0x25: AND(); break;
                case 0x35: AND(); break;
                case 0x2D: AND(); break;
                case 0x3D: AND(); break;
                case 0x39: AND(); break;
                case 0x21: AND(); break;
                case 0x31: AND(); break;
                case 0x24: BIT(); break;
                case 0x2C: BIT(); break;
                case 0x09: ORA(); break;
                case 0x05: ORA(); break;
                case 0x15: ORA(); break;
                case 0x0D: ORA(); break;
                case 0x1D: ORA(); break;
                case 0x19: ORA(); break;
                case 0x01: ORA(); break;
                case 0x11: ORA(); break;
                case 0x49: EOR(); break;
                case 0x45: EOR(); break;
                case 0x55: EOR(); break;
                case 0x4D: EOR(); break;
                case 0x5D: EOR(); break;
                case 0x59: EOR(); break;
                case 0x41: EOR(); break;
                case 0x51: EOR(); break;

                case 0x48: PHA(); break;
                case 0x08: PHP(); break;
                case 0x68: PLA(); break;
                case 0x28: PLP(); break;

                case 0x38: SEC(); break;
                case 0xF8: SED(); break;
                case 0x78: SEI(); break;

                case 0x90: BCC(); break;
                case 0xB0: BCS(); break;
                case 0xF0: BEQ(); break;
                case 0x30: BMI(); break;
                case 0xD0: BNE(); break;
                case 0x10: BPL(); break;
                case 0x50: BVC(); break;
                case 0x70: BVS(); break;

                case 0xC9: CMP(); break;
                case 0xC5: CMP(); break;
                case 0xD5: CMP(); break;
                case 0xCD: CMP(); break;
                case 0xDD: CMP(); break;
                case 0xD9: CMP(); break;
                case 0xC1: CMP(); break;
                case 0xD1: CMP(); break;
                case 0xE0: CPX(); break;
                case 0xE4: CPX(); break;
                case 0xEC: CPX(); break;
                case 0xC0: CPY(); break;
                case 0xC4: CPY(); break;
                case 0xCC: CPY(); break;

                case 0x20: JSR(); break;
                case 0x00: BRK(); break;
                case 0x60: RTS(); break;
                case 0x40: RTI(); break;

                case 0xAA: TAX(); break;
                case 0xA8: TAY(); break;
                case 0xBA: TSX(); break;
                case 0x8A: TXA(); break;
                case 0x9A: TXS(); break;
                case 0x98: TYA(); break;

                default: 
                    Console.WriteLine("Invalid Opcode!!    Op: 0x" + Convert.ToString(current_op, 16) + Environment.NewLine + "Opcode at: 0x" + Convert.ToString(pc_reg, 16)); break;
            }
        }

        public ushort SwitchEndian(byte mem1, byte mem2)
        {
            return (ushort)((mem2 << 8) | mem1);
        }

        //Always do this after a math operation
        public void ResetFlags()
        {
            c_flag = 0;
            z_flag = 0;
            v_flag = 0;
            n_flag = 0;
        }
    }
}