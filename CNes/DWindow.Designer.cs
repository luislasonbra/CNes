namespace CNes
{
    partial class DWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBytes = new System.Windows.Forms.ListView();
            this.colAddr = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colOpcode = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtAddr = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRun = new System.Windows.Forms.Button();
            this.btnResetInfo = new System.Windows.Forms.Button();
            this.btnGotoAddr = new System.Windows.Forms.Button();
            this.btnStep256 = new System.Windows.Forms.Button();
            this.btnStep128 = new System.Windows.Forms.Button();
            this.btnStep1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblCycles = new System.Windows.Forms.Label();
            this.lblTicks = new System.Windows.Forms.Label();
            this.lblReset = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtIFlag = new System.Windows.Forms.TextBox();
            this.txtVFlag = new System.Windows.Forms.TextBox();
            this.txtBFlag = new System.Windows.Forms.TextBox();
            this.txtNFlag = new System.Windows.Forms.TextBox();
            this.txtZFlag = new System.Windows.Forms.TextBox();
            this.txtCFlag = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkRealTime = new System.Windows.Forms.CheckBox();
            this.txtSPReg = new System.Windows.Forms.TextBox();
            this.txtYReg = new System.Windows.Forms.TextBox();
            this.txtPCReg = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtXReg = new System.Windows.Forms.TextBox();
            this.txtAReg = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBytes
            // 
            this.listBytes.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAddr,
            this.colOpcode});
            this.listBytes.GridLines = true;
            this.listBytes.Location = new System.Drawing.Point(4, 3);
            this.listBytes.Name = "listBytes";
            this.listBytes.Size = new System.Drawing.Size(190, 248);
            this.listBytes.TabIndex = 0;
            this.listBytes.UseCompatibleStateImageBehavior = false;
            this.listBytes.View = System.Windows.Forms.View.Details;
            // 
            // colAddr
            // 
            this.colAddr.Text = "Address";
            this.colAddr.Width = 82;
            // 
            // colOpcode
            // 
            this.colOpcode.Text = "Opcode/ByteAt";
            this.colOpcode.Width = 103;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtAddr);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnRun);
            this.groupBox1.Controls.Add(this.btnResetInfo);
            this.groupBox1.Controls.Add(this.btnGotoAddr);
            this.groupBox1.Controls.Add(this.btnStep256);
            this.groupBox1.Controls.Add(this.btnStep128);
            this.groupBox1.Controls.Add(this.btnStep1);
            this.groupBox1.Location = new System.Drawing.Point(200, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(256, 104);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // txtAddr
            // 
            this.txtAddr.Location = new System.Drawing.Point(87, 50);
            this.txtAddr.MaxLength = 4;
            this.txtAddr.Name = "txtAddr";
            this.txtAddr.Size = new System.Drawing.Size(75, 20);
            this.txtAddr.TabIndex = 31;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Location = new System.Drawing.Point(80, 52);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(10, 13);
            this.label16.TabIndex = 30;
            this.label16.Text = ":";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(87, 77);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(6, 77);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 5;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnResetInfo
            // 
            this.btnResetInfo.Location = new System.Drawing.Point(168, 48);
            this.btnResetInfo.Name = "btnResetInfo";
            this.btnResetInfo.Size = new System.Drawing.Size(75, 23);
            this.btnResetInfo.TabIndex = 4;
            this.btnResetInfo.Text = "Reset info";
            this.btnResetInfo.UseVisualStyleBackColor = true;
            // 
            // btnGotoAddr
            // 
            this.btnGotoAddr.Location = new System.Drawing.Point(6, 48);
            this.btnGotoAddr.Name = "btnGotoAddr";
            this.btnGotoAddr.Size = new System.Drawing.Size(75, 23);
            this.btnGotoAddr.TabIndex = 3;
            this.btnGotoAddr.Text = "Goto addr";
            this.btnGotoAddr.UseVisualStyleBackColor = true;
            this.btnGotoAddr.Click += new System.EventHandler(this.btnGotoAddr_Click);
            // 
            // btnStep256
            // 
            this.btnStep256.Location = new System.Drawing.Point(168, 19);
            this.btnStep256.Name = "btnStep256";
            this.btnStep256.Size = new System.Drawing.Size(75, 23);
            this.btnStep256.TabIndex = 2;
            this.btnStep256.Text = "Step 256";
            this.btnStep256.UseVisualStyleBackColor = true;
            this.btnStep256.Click += new System.EventHandler(this.btnStep256_Click);
            // 
            // btnStep128
            // 
            this.btnStep128.Location = new System.Drawing.Point(87, 19);
            this.btnStep128.Name = "btnStep128";
            this.btnStep128.Size = new System.Drawing.Size(75, 23);
            this.btnStep128.TabIndex = 1;
            this.btnStep128.Text = "Step 128";
            this.btnStep128.UseVisualStyleBackColor = true;
            this.btnStep128.Click += new System.EventHandler(this.btnStep128_Click);
            // 
            // btnStep1
            // 
            this.btnStep1.Location = new System.Drawing.Point(6, 19);
            this.btnStep1.Name = "btnStep1";
            this.btnStep1.Size = new System.Drawing.Size(75, 23);
            this.btnStep1.TabIndex = 0;
            this.btnStep1.Text = "Step 1";
            this.btnStep1.UseVisualStyleBackColor = true;
            this.btnStep1.Click += new System.EventHandler(this.btnStep1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblCycles);
            this.groupBox2.Controls.Add(this.lblTicks);
            this.groupBox2.Controls.Add(this.lblReset);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(462, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(129, 104);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Info";
            // 
            // lblCycles
            // 
            this.lblCycles.AutoSize = true;
            this.lblCycles.Location = new System.Drawing.Point(77, 19);
            this.lblCycles.Name = "lblCycles";
            this.lblCycles.Size = new System.Drawing.Size(13, 13);
            this.lblCycles.TabIndex = 5;
            this.lblCycles.Text = "0";
            // 
            // lblTicks
            // 
            this.lblTicks.AutoSize = true;
            this.lblTicks.Location = new System.Drawing.Point(77, 35);
            this.lblTicks.Name = "lblTicks";
            this.lblTicks.Size = new System.Drawing.Size(13, 13);
            this.lblTicks.TabIndex = 4;
            this.lblTicks.Text = "--";
            // 
            // lblReset
            // 
            this.lblReset.AutoSize = true;
            this.lblReset.Location = new System.Drawing.Point(77, 53);
            this.lblReset.Name = "lblReset";
            this.lblReset.Size = new System.Drawing.Size(18, 13);
            this.lblReset.TabIndex = 3;
            this.lblReset.Text = "0x";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reset vector:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ticks:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Cycles:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.txtIFlag);
            this.groupBox3.Controls.Add(this.txtVFlag);
            this.groupBox3.Controls.Add(this.txtBFlag);
            this.groupBox3.Controls.Add(this.txtNFlag);
            this.groupBox3.Controls.Add(this.txtZFlag);
            this.groupBox3.Controls.Add(this.txtCFlag);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chkInfo);
            this.groupBox3.Controls.Add(this.chkRealTime);
            this.groupBox3.Controls.Add(this.txtSPReg);
            this.groupBox3.Controls.Add(this.txtYReg);
            this.groupBox3.Controls.Add(this.txtPCReg);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtXReg);
            this.groupBox3.Controls.Add(this.txtAReg);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(200, 114);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(391, 138);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Registers and settings";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(99, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 16);
            this.label12.TabIndex = 29;
            this.label12.Text = "Flags:";
            // 
            // txtIFlag
            // 
            this.txtIFlag.Location = new System.Drawing.Point(172, 53);
            this.txtIFlag.Name = "txtIFlag";
            this.txtIFlag.Size = new System.Drawing.Size(12, 20);
            this.txtIFlag.TabIndex = 28;
            this.txtIFlag.Text = "0";
            // 
            // txtVFlag
            // 
            this.txtVFlag.Location = new System.Drawing.Point(172, 94);
            this.txtVFlag.Name = "txtVFlag";
            this.txtVFlag.Size = new System.Drawing.Size(12, 20);
            this.txtVFlag.TabIndex = 27;
            this.txtVFlag.Text = "0";
            // 
            // txtBFlag
            // 
            this.txtBFlag.Location = new System.Drawing.Point(172, 74);
            this.txtBFlag.Name = "txtBFlag";
            this.txtBFlag.Size = new System.Drawing.Size(12, 20);
            this.txtBFlag.TabIndex = 26;
            this.txtBFlag.Text = "0";
            // 
            // txtNFlag
            // 
            this.txtNFlag.Location = new System.Drawing.Point(172, 114);
            this.txtNFlag.Name = "txtNFlag";
            this.txtNFlag.Size = new System.Drawing.Size(12, 20);
            this.txtNFlag.TabIndex = 25;
            this.txtNFlag.Text = "0";
            // 
            // txtZFlag
            // 
            this.txtZFlag.Location = new System.Drawing.Point(172, 33);
            this.txtZFlag.Name = "txtZFlag";
            this.txtZFlag.Size = new System.Drawing.Size(12, 20);
            this.txtZFlag.TabIndex = 22;
            this.txtZFlag.Text = "0";
            // 
            // txtCFlag
            // 
            this.txtCFlag.Location = new System.Drawing.Point(172, 13);
            this.txtCFlag.Name = "txtCFlag";
            this.txtCFlag.Size = new System.Drawing.Size(12, 20);
            this.txtCFlag.TabIndex = 21;
            this.txtCFlag.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(154, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(18, 13);
            this.label15.TabIndex = 20;
            this.label15.Text = "N:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(155, 97);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "V:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(155, 77);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(17, 13);
            this.label13.TabIndex = 18;
            this.label13.Text = "B:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(156, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(13, 13);
            this.label11.TabIndex = 16;
            this.label11.Text = "I:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(155, 36);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 13);
            this.label10.TabIndex = 15;
            this.label10.Text = "Z:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(155, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "C:";
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Location = new System.Drawing.Point(262, 39);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(79, 17);
            this.chkInfo.TabIndex = 13;
            this.chkInfo.Text = "Enable info";
            this.chkInfo.UseVisualStyleBackColor = true;
            // 
            // chkRealTime
            // 
            this.chkRealTime.AutoSize = true;
            this.chkRealTime.Location = new System.Drawing.Point(262, 16);
            this.chkRealTime.Name = "chkRealTime";
            this.chkRealTime.Size = new System.Drawing.Size(123, 17);
            this.chkRealTime.TabIndex = 12;
            this.chkRealTime.Text = "Real-time debugging";
            this.chkRealTime.UseVisualStyleBackColor = true;
            // 
            // txtSPReg
            // 
            this.txtSPReg.Location = new System.Drawing.Point(29, 85);
            this.txtSPReg.Name = "txtSPReg";
            this.txtSPReg.Size = new System.Drawing.Size(18, 20);
            this.txtSPReg.TabIndex = 11;
            this.txtSPReg.Text = "0";
            // 
            // txtYReg
            // 
            this.txtYReg.Location = new System.Drawing.Point(29, 61);
            this.txtYReg.Name = "txtYReg";
            this.txtYReg.Size = new System.Drawing.Size(18, 20);
            this.txtYReg.TabIndex = 10;
            this.txtYReg.Text = "0";
            // 
            // txtPCReg
            // 
            this.txtPCReg.Location = new System.Drawing.Point(29, 108);
            this.txtPCReg.Name = "txtPCReg";
            this.txtPCReg.Size = new System.Drawing.Size(31, 20);
            this.txtPCReg.TabIndex = 9;
            this.txtPCReg.Text = "0000";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "PC:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(5, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "SP:";
            // 
            // txtXReg
            // 
            this.txtXReg.Location = new System.Drawing.Point(29, 39);
            this.txtXReg.Name = "txtXReg";
            this.txtXReg.Size = new System.Drawing.Size(18, 20);
            this.txtXReg.TabIndex = 4;
            this.txtXReg.Text = "0";
            // 
            // txtAReg
            // 
            this.txtAReg.Location = new System.Drawing.Point(29, 16);
            this.txtAReg.Name = "txtAReg";
            this.txtAReg.Size = new System.Drawing.Size(18, 20);
            this.txtAReg.TabIndex = 3;
            this.txtAReg.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 67);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Y:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "A:";
            // 
            // DWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 257);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBytes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DWindow";
            this.Text = "Dissasembler";
            this.Load += new System.EventHandler(this.DWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listBytes;
        private System.Windows.Forms.ColumnHeader colAddr;
        private System.Windows.Forms.ColumnHeader colOpcode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.Button btnResetInfo;
        private System.Windows.Forms.Button btnGotoAddr;
        private System.Windows.Forms.Button btnStep256;
        private System.Windows.Forms.Button btnStep128;
        private System.Windows.Forms.Button btnStep1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.CheckBox chkRealTime;
        private System.Windows.Forms.TextBox txtSPReg;
        private System.Windows.Forms.TextBox txtYReg;
        private System.Windows.Forms.TextBox txtPCReg;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtXReg;
        private System.Windows.Forms.TextBox txtAReg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtCFlag;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtIFlag;
        private System.Windows.Forms.TextBox txtVFlag;
        private System.Windows.Forms.TextBox txtBFlag;
        private System.Windows.Forms.TextBox txtNFlag;
        private System.Windows.Forms.TextBox txtZFlag;
        private System.Windows.Forms.TextBox txtAddr;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblCycles;
        private System.Windows.Forms.Label lblTicks;
        private System.Windows.Forms.Label lblReset;
    }
}