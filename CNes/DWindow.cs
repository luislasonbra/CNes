using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CNes.Core;
using CNes.Cartridge;

namespace CNes
{
    partial class DWindow : Form
    {
        NES6502 cpu;
        NESCore nes;
        Cart cr;

        private int curIndex = 0;
        private ushort tempPCReg = 0;

        public DWindow(NES6502 cpu, Cart cr, NESCore nes)
        {
            
            this.cpu = cpu;
            this.cr = cr;
            this.nes = nes;
            InitializeComponent();
        }

        private void DWindow_Load(object sender, EventArgs e)
        {
            InitPopulateList();
            InitProcInfo();
            UpdateRegsInfo();
        }

        public void InitPopulateList()
        {
            ushort resetAddr = nes.GetResetAddr();
            int updateIndex = 0;
            for (ushort i = resetAddr; i < 0xFFFF; i++)
            {
                listBytes.Items.Add("0x" + ConvertHex(i));
                listBytes.Items[updateIndex].SubItems.Add(ConvertHex(nes.Read8(i)));
                updateIndex += 1;
            }

            lblReset.Text = "0x" + Convert.ToString(resetAddr, 16);
        }

        public void InitProcInfo()
        {
            lblReset.Text = "0x" + ConvertHex(nes.GetResetAddr());
            lblCycles.Text = cpu.cycles.ToString();
        }

        public void UpdateList(int index)
        {
            //foreach (ListViewItem itm in listBytes.Items)
            //{
            //    itm.BackColor = Color.White;
            //    itm.SubItems[0].BackColor = Color.White;
            //}

            listBytes.Items[index].BackColor = Color.Lime;
            listBytes.Items[index].SubItems[0].BackColor = Color.Lime;
            listBytes.Items[index].EnsureVisible();
        }

        public void UpdateRegsInfo()
        {
            txtAReg.Text = ConvertHex(cpu.a_reg);
            txtXReg.Text = ConvertHex(cpu.x_reg);
            txtYReg.Text = ConvertHex(cpu.y_reg);
            txtSPReg.Text = ConvertHex(cpu.sp_reg);
            txtPCReg.Text = ConvertHex(cpu.pc_reg);
            
            txtCFlag.Text = ConvertHex(cpu.c_flag);
            txtZFlag.Text = ConvertHex(cpu.z_flag);
            txtIFlag.Text = ConvertHex(cpu.i_flag);
            txtBFlag.Text = ConvertHex(cpu.b_flag);
            txtVFlag.Text = ConvertHex(cpu.v_flag);
            txtNFlag.Text = ConvertHex(cpu.n_flag);

            lblCycles.Text = cpu.cycles.ToString();
        }

        public string ConvertHex(int num)
        {
            return Convert.ToString(num, 16).ToUpper();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cpu.isRunning = false;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            cpu.Run();
        }

        private void btnStep1_Click(object sender, EventArgs e)
        {
            tempPCReg = cpu.pc_reg;
            cpu.DoStep(1);
            curIndex += cpu.pc_reg - tempPCReg;

            UpdateList(curIndex);
            UpdateRegsInfo();
        }

        private void btnStep128_Click(object sender, EventArgs e)
        {
            tempPCReg = cpu.pc_reg;
            cpu.DoStep(128);
            curIndex = cpu.pc_reg - tempPCReg;

            UpdateList(curIndex);
            UpdateRegsInfo();
        }

        private void btnStep256_Click(object sender, EventArgs e)
        {
            tempPCReg = cpu.pc_reg;
            cpu.DoStep(256);
            curIndex = cpu.pc_reg - tempPCReg;

            UpdateList(curIndex);
            UpdateRegsInfo();
        }

        private void btnGotoAddr_Click(object sender, EventArgs e)
        {
            int tempIndex = 0;
            int gotoAddr = int.Parse(txtAddr.Text, NumberStyles.HexNumber);
            cpu.pc_reg = (ushort)(gotoAddr);
            if ((gotoAddr - cpu.pc_reg) > 0)
            {
                tempIndex = gotoAddr - cpu.pc_reg;
            }
            else
            {
                tempIndex = cpu.pc_reg - gotoAddr;
            }

            UpdateList(tempIndex);
        }
    }
}
