using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using CNes.Core;
using CNes.Cartridge;
using CNes.Screen;
using CNes.GFX;
using CNes.Screen.Renderers;

namespace CNes
{
    public partial class EmuWindow : Form
    {
        public string fileName = "";
        private static readonly string nl = Environment.NewLine;

        CartReader cs = new CartReader();
        NES6502 cpu;
        NESCore nes;
        Cart cr;
        PPUCore ppu;
        IRenderer ren;

        public EmuWindow()
        {
            InitializeComponent();
        }

        public void Log(string txt, int type)
        {
            switch (type)
            {
                case 1:
                    txtConsole.Text += "[INFO]   " + txt;
                    break;
                case 2:
                    txtConsole.Text += "[WARNING]   " + txt;
                    break;
                case 3:
                    txtConsole.Text += "[ERROR]   " + txt;
                    break;
                default: break; //WTF
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtConsole.Text += "CNES by Alex Restifo" + nl;
            txtConsole.Text += "------------------------------------------";
        }

        #region Utilities
        private string ConvertHex(int target)
        {
            return Convert.ToString(target, 16).ToUpper();
        }

        #region Sh*t I need to remove (and some debugging)
        //Plz remove all this stuff later, these methods are only for testing
        public void PrintProcRegs()
        {
            txtConsole.Text += nl + "RUNNING OPCODE 0x" + cpu.current_op + nl;
            txtConsole.Text += "A:      0x" + ConvertHex(cpu.a_reg) +nl;
            txtConsole.Text += "X:      0x" + ConvertHex(cpu.x_reg) + nl;
            txtConsole.Text += "Y:      0x" + ConvertHex(cpu.y_reg) + nl + nl;

            txtConsole.Text += "C:      " + cpu.c_flag + nl;
            txtConsole.Text += "Z:      " + cpu.z_flag + nl;
            txtConsole.Text += "I:      " + cpu.i_flag + nl;
            txtConsole.Text += "D:      " + cpu.d_flag + nl;
            txtConsole.Text += "B:      " + cpu.b_flag + nl;
            txtConsole.Text += "V:      " + cpu.v_flag + nl;
            txtConsole.Text += "N:      " + cpu.n_flag + nl;
            txtConsole.Text += "------------------------------------------" + nl;
            //The following code automatically scrolls to the bottom of the textbox
            txtConsole.SelectionStart = txtConsole.TextLength; txtConsole.ScrollToCaret();
            
        }

        public void PrintByteAt(ushort address)
        {
            txtConsole.Text += "Memory at 0x" + ConvertHex(address) + ": " + ConvertHex(nes.Read8(address));
        }

        private void buttons(bool enable)
        {
            if (enable)
            {
                dissasemblerToolStripMenuItem.Enabled = true;
                memoryToolStripMenuItem.Enabled = true;
                stepToolStripMenuItem.Enabled = true;
                return;
            }

            dissasemblerToolStripMenuItem.Enabled = false;
            stepToolStripMenuItem.Enabled = false;
            memoryToolStripMenuItem.Enabled = false;
        }

        public void ListOpcodesAt()
        {
            ushort offset = 0x8000;
            string msgBuffer = "";

            for (int i = 0; i < 40; i++)
            {
                byte opcodeAt = nes.Read8((ushort)(offset + i));
                msgBuffer += "0x" + ConvertHex(offset + i) + ": " + ConvertHex(opcodeAt).ToUpper() + nl;
            }
            txtConsole.Text += "RESET POINT: 0x" + ConvertHex(nes.GetResetAddr()) + nl;
            txtConsole.Text += msgBuffer + nl;
            txtConsole.Text += ConvertHex(nes.Read8(0x8015)) + nl;
            txtConsole.Text += ConvertHex(nes.Read8(0x8016)) + nl;
            txtConsole.Text += ConvertHex(nes.Read16(0x8015));
        }
        #endregion
        #region Thread-Safety
        private delegate void TSDelegate(Control control, string propertyName, object propertyValue);
        private void UpdateProp(Control control, string propertyName, object propertyValue)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new TSDelegate(UpdateProp), new object[] { control, propertyName, propertyValue });
            }
            else
            {
                control.GetType().InvokeMember(propertyName, BindingFlags.SetProperty, null, control, new object[] { propertyValue });
            }
        }
        #endregion

        #endregion

        private void dissasemblerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DWindow dw = new DWindow(cpu, cr, nes);
            dw.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "NES Roms (*.nes)|*.nes";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                fileName = ofd.FileName;
                //START INIT CODE
                cr = cs.ReadCart(fileName);
                nes = new NESCore(cr);
                cpu = new NES6502(cr, nes);
                ren = new BMP16Renderer(cr);
                ppu = new PPUCore(cr, ren);

                cpu.pc_reg = nes.GetResetAddr();
                //END INIT CODE
                MessageBox.Show("PRG ROM loaded!!!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                buttons(true);
            }
        }

        private void stepTEMPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BMP16Renderer render = new BMP16Renderer(cr);
        }

        private void nTSCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            nTSCToolStripMenuItem.Checked = true;
            pALToolStripMenuItem.Checked = false;
            
        }

        private void dODEBUGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ren.Render(ppu.RenderScanline());
        }
    }
}
