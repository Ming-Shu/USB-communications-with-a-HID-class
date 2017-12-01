namespace USB_HID_Communication
{
    partial class Form1
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
            this.btn_Set = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.fraByteSet = new System.Windows.Forms.GroupBox();
            this.CboByte = new System.Windows.Forms.ComboBox();
            this.btn_BytesSet = new System.Windows.Forms.Button();
            this.lblVendorID = new System.Windows.Forms.Label();
            this.txtVendorID = new System.Windows.Forms.TextBox();
            this.lblProductID = new System.Windows.Forms.Label();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.fraDeviceIdentifiers = new System.Windows.Forms.GroupBox();
            this.fraTransfers = new System.Windows.Forms.GroupBox();
            this.btn_continues = new System.Windows.Forms.Button();
            this.btn_receive = new System.Windows.Forms.Button();
            this.btn_send = new System.Windows.Forms.Button();
            this.freStatus = new System.Windows.Forms.GroupBox();
            this.LstStatus = new System.Windows.Forms.ListBox();
            this.freInsert = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.fraReadytransfers = new System.Windows.Forms.GroupBox();
            this.LstReadyCmd = new System.Windows.Forms.ListBox();
            this.freResult = new System.Windows.Forms.GroupBox();
            this.LstResult = new System.Windows.Forms.ListBox();
            this.btn_RightSet = new System.Windows.Forms.Button();
            this.btn_leftSet = new System.Windows.Forms.Button();
            this.btn_leftRead = new System.Windows.Forms.Button();
            this.fraByteSet.SuspendLayout();
            this.fraDeviceIdentifiers.SuspendLayout();
            this.fraTransfers.SuspendLayout();
            this.freStatus.SuspendLayout();
            this.freInsert.SuspendLayout();
            this.fraReadytransfers.SuspendLayout();
            this.freResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Set
            // 
            this.btn_Set.Location = new System.Drawing.Point(18, 111);
            this.btn_Set.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.Size = new System.Drawing.Size(74, 26);
            this.btn_Set.TabIndex = 4;
            this.btn_Set.Text = "Set";
            this.btn_Set.UseVisualStyleBackColor = true;
            this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(116, 111);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(74, 26);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // fraByteSet
            // 
            this.fraByteSet.BackColor = System.Drawing.SystemColors.Control;
            this.fraByteSet.Controls.Add(this.CboByte);
            this.fraByteSet.Controls.Add(this.btn_BytesSet);
            this.fraByteSet.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraByteSet.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fraByteSet.Location = new System.Drawing.Point(22, 186);
            this.fraByteSet.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fraByteSet.Name = "fraByteSet";
            this.fraByteSet.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fraByteSet.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fraByteSet.Size = new System.Drawing.Size(217, 165);
            this.fraByteSet.TabIndex = 15;
            this.fraByteSet.TabStop = false;
            this.fraByteSet.Text = "Setting Bytes Number";
            // 
            // CboByte
            // 
            this.CboByte.BackColor = System.Drawing.SystemColors.Window;
            this.CboByte.Cursor = System.Windows.Forms.Cursors.Default;
            this.CboByte.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboByte.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboByte.ForeColor = System.Drawing.SystemColors.WindowText;
            this.CboByte.Location = new System.Drawing.Point(20, 36);
            this.CboByte.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.CboByte.Name = "CboByte";
            this.CboByte.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CboByte.Size = new System.Drawing.Size(157, 28);
            this.CboByte.TabIndex = 13;
            // 
            // btn_BytesSet
            // 
            this.btn_BytesSet.Location = new System.Drawing.Point(20, 88);
            this.btn_BytesSet.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_BytesSet.Name = "btn_BytesSet";
            this.btn_BytesSet.Size = new System.Drawing.Size(157, 50);
            this.btn_BytesSet.TabIndex = 12;
            this.btn_BytesSet.Text = "Bytes Set";
            this.btn_BytesSet.UseVisualStyleBackColor = true;
            this.btn_BytesSet.Click += new System.EventHandler(this.btn_BytesSet_Click);
            // 
            // lblVendorID
            // 
            this.lblVendorID.Location = new System.Drawing.Point(16, 35);
            this.lblVendorID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVendorID.Name = "lblVendorID";
            this.lblVendorID.Size = new System.Drawing.Size(112, 23);
            this.lblVendorID.TabIndex = 0;
            this.lblVendorID.Text = "Vendor ID (hex):";
            // 
            // txtVendorID
            // 
            this.txtVendorID.Location = new System.Drawing.Point(120, 35);
            this.txtVendorID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtVendorID.MaxLength = 4;
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.Size = new System.Drawing.Size(72, 27);
            this.txtVendorID.TabIndex = 1;
            this.txtVendorID.Text = "10C4";
            // 
            // lblProductID
            // 
            this.lblProductID.Location = new System.Drawing.Point(16, 66);
            this.lblProductID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProductID.Name = "lblProductID";
            this.lblProductID.Size = new System.Drawing.Size(112, 23);
            this.lblProductID.TabIndex = 2;
            this.lblProductID.Text = "Product ID (hex):";
            // 
            // txtProductID
            // 
            this.txtProductID.Location = new System.Drawing.Point(120, 66);
            this.txtProductID.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.txtProductID.MaxLength = 4;
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(72, 27);
            this.txtProductID.TabIndex = 3;
            this.txtProductID.Text = "8A8F";
            // 
            // fraDeviceIdentifiers
            // 
            this.fraDeviceIdentifiers.Controls.Add(this.btn_cancel);
            this.fraDeviceIdentifiers.Controls.Add(this.btn_Set);
            this.fraDeviceIdentifiers.Controls.Add(this.txtProductID);
            this.fraDeviceIdentifiers.Controls.Add(this.lblProductID);
            this.fraDeviceIdentifiers.Controls.Add(this.txtVendorID);
            this.fraDeviceIdentifiers.Controls.Add(this.lblVendorID);
            this.fraDeviceIdentifiers.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraDeviceIdentifiers.Location = new System.Drawing.Point(22, 12);
            this.fraDeviceIdentifiers.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fraDeviceIdentifiers.Name = "fraDeviceIdentifiers";
            this.fraDeviceIdentifiers.Padding = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.fraDeviceIdentifiers.Size = new System.Drawing.Size(217, 161);
            this.fraDeviceIdentifiers.TabIndex = 11;
            this.fraDeviceIdentifiers.TabStop = false;
            this.fraDeviceIdentifiers.Text = "Device Identifiers";
            // 
            // fraTransfers
            // 
            this.fraTransfers.BackColor = System.Drawing.SystemColors.Control;
            this.fraTransfers.Controls.Add(this.btn_continues);
            this.fraTransfers.Controls.Add(this.btn_receive);
            this.fraTransfers.Controls.Add(this.btn_send);
            this.fraTransfers.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraTransfers.ForeColor = System.Drawing.SystemColors.ControlText;
            this.fraTransfers.Location = new System.Drawing.Point(22, 363);
            this.fraTransfers.Name = "fraTransfers";
            this.fraTransfers.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.fraTransfers.Size = new System.Drawing.Size(217, 237);
            this.fraTransfers.TabIndex = 16;
            this.fraTransfers.TabStop = false;
            this.fraTransfers.Text = "Command Transfers";
            // 
            // btn_continues
            // 
            this.btn_continues.Location = new System.Drawing.Point(20, 165);
            this.btn_continues.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_continues.Name = "btn_continues";
            this.btn_continues.Size = new System.Drawing.Size(157, 50);
            this.btn_continues.TabIndex = 15;
            this.btn_continues.Text = "Continues Receive (Interrupt)";
            this.btn_continues.UseVisualStyleBackColor = true;
            this.btn_continues.Click += new System.EventHandler(this.btn_continues_Click);
            // 
            // btn_receive
            // 
            this.btn_receive.Location = new System.Drawing.Point(20, 101);
            this.btn_receive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_receive.Name = "btn_receive";
            this.btn_receive.Size = new System.Drawing.Size(157, 50);
            this.btn_receive.TabIndex = 14;
            this.btn_receive.Text = "Receive(Interrupt)";
            this.btn_receive.UseVisualStyleBackColor = true;
            this.btn_receive.Click += new System.EventHandler(this.btn_receive_Click);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(20, 35);
            this.btn_send.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(157, 50);
            this.btn_send.TabIndex = 13;
            this.btn_send.Text = "Send(Control)";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // freStatus
            // 
            this.freStatus.Controls.Add(this.LstStatus);
            this.freStatus.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freStatus.Location = new System.Drawing.Point(267, 12);
            this.freStatus.Name = "freStatus";
            this.freStatus.Size = new System.Drawing.Size(695, 161);
            this.freStatus.TabIndex = 17;
            this.freStatus.TabStop = false;
            this.freStatus.Text = "Status Information";
            // 
            // LstStatus
            // 
            this.LstStatus.FormattingEnabled = true;
            this.LstStatus.ItemHeight = 20;
            this.LstStatus.Location = new System.Drawing.Point(6, 24);
            this.LstStatus.Name = "LstStatus";
            this.LstStatus.Size = new System.Drawing.Size(683, 124);
            this.LstStatus.TabIndex = 0;
            // 
            // freInsert
            // 
            this.freInsert.Controls.Add(this.panel1);
            this.freInsert.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freInsert.Location = new System.Drawing.Point(261, 186);
            this.freInsert.Name = "freInsert";
            this.freInsert.Size = new System.Drawing.Size(325, 165);
            this.freInsert.TabIndex = 18;
            this.freInsert.TabStop = false;
            this.freInsert.Text = "Insert Command";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(5, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(314, 141);
            this.panel1.TabIndex = 0;
            // 
            // fraReadytransfers
            // 
            this.fraReadytransfers.Controls.Add(this.LstReadyCmd);
            this.fraReadytransfers.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fraReadytransfers.Location = new System.Drawing.Point(621, 186);
            this.fraReadytransfers.Name = "fraReadytransfers";
            this.fraReadytransfers.Size = new System.Drawing.Size(341, 165);
            this.fraReadytransfers.TabIndex = 19;
            this.fraReadytransfers.TabStop = false;
            this.fraReadytransfers.Text = "Ready Transfers";
            // 
            // LstReadyCmd
            // 
            this.LstReadyCmd.FormattingEnabled = true;
            this.LstReadyCmd.ItemHeight = 20;
            this.LstReadyCmd.Location = new System.Drawing.Point(6, 24);
            this.LstReadyCmd.Name = "LstReadyCmd";
            this.LstReadyCmd.Size = new System.Drawing.Size(329, 124);
            this.LstReadyCmd.TabIndex = 0;
            // 
            // freResult
            // 
            this.freResult.Controls.Add(this.LstResult);
            this.freResult.Font = new System.Drawing.Font("Buxton Sketch", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.freResult.Location = new System.Drawing.Point(261, 363);
            this.freResult.Name = "freResult";
            this.freResult.Size = new System.Drawing.Size(695, 237);
            this.freResult.TabIndex = 20;
            this.freResult.TabStop = false;
            this.freResult.Text = "Result Information";
            // 
            // LstResult
            // 
            this.LstResult.FormattingEnabled = true;
            this.LstResult.ItemHeight = 20;
            this.LstResult.Location = new System.Drawing.Point(6, 24);
            this.LstResult.Name = "LstResult";
            this.LstResult.Size = new System.Drawing.Size(683, 204);
            this.LstResult.TabIndex = 0;
            // 
            // btn_RightSet
            // 
            this.btn_RightSet.Font = new System.Drawing.Font("Buxton Sketch", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_RightSet.Location = new System.Drawing.Point(587, 208);
            this.btn_RightSet.Name = "btn_RightSet";
            this.btn_RightSet.Size = new System.Drawing.Size(34, 34);
            this.btn_RightSet.TabIndex = 21;
            this.btn_RightSet.Text = ">>";
            this.btn_RightSet.UseVisualStyleBackColor = true;
            this.btn_RightSet.Click += new System.EventHandler(this.btn_RightSet_Click);
            // 
            // btn_leftSet
            // 
            this.btn_leftSet.Font = new System.Drawing.Font("Buxton Sketch", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_leftSet.Location = new System.Drawing.Point(587, 253);
            this.btn_leftSet.Name = "btn_leftSet";
            this.btn_leftSet.Size = new System.Drawing.Size(34, 34);
            this.btn_leftSet.TabIndex = 22;
            this.btn_leftSet.Text = "X";
            this.btn_leftSet.UseVisualStyleBackColor = true;
            this.btn_leftSet.Click += new System.EventHandler(this.btn_leftSet_Click);
            // 
            // btn_leftRead
            // 
            this.btn_leftRead.Font = new System.Drawing.Font("Buxton Sketch", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_leftRead.Location = new System.Drawing.Point(587, 303);
            this.btn_leftRead.Name = "btn_leftRead";
            this.btn_leftRead.Size = new System.Drawing.Size(34, 34);
            this.btn_leftRead.TabIndex = 23;
            this.btn_leftRead.Text = "<<";
            this.btn_leftRead.UseVisualStyleBackColor = true;
            this.btn_leftRead.Click += new System.EventHandler(this.btn_leftRead_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 612);
            this.Controls.Add(this.btn_leftRead);
            this.Controls.Add(this.btn_leftSet);
            this.Controls.Add(this.btn_RightSet);
            this.Controls.Add(this.freResult);
            this.Controls.Add(this.fraReadytransfers);
            this.Controls.Add(this.freInsert);
            this.Controls.Add(this.freStatus);
            this.Controls.Add(this.fraTransfers);
            this.Controls.Add(this.fraByteSet);
            this.Controls.Add(this.fraDeviceIdentifiers);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "Form1";
            this.Text = "HID Communication";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.fraByteSet.ResumeLayout(false);
            this.fraDeviceIdentifiers.ResumeLayout(false);
            this.fraDeviceIdentifiers.PerformLayout();
            this.fraTransfers.ResumeLayout(false);
            this.freStatus.ResumeLayout(false);
            this.freInsert.ResumeLayout(false);
            this.fraReadytransfers.ResumeLayout(false);
            this.freResult.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_Set;
        public System.Windows.Forms.GroupBox fraByteSet;
        public System.Windows.Forms.ComboBox CboByte;
        private System.Windows.Forms.Button btn_BytesSet;
        internal System.Windows.Forms.Label lblVendorID;
        internal System.Windows.Forms.TextBox txtVendorID;
        internal System.Windows.Forms.Label lblProductID;
        internal System.Windows.Forms.TextBox txtProductID;
        internal System.Windows.Forms.GroupBox fraDeviceIdentifiers;
        public System.Windows.Forms.GroupBox fraTransfers;
        private System.Windows.Forms.Button btn_continues;
        private System.Windows.Forms.Button btn_receive;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.GroupBox freStatus;
        private System.Windows.Forms.ListBox LstStatus;
        private System.Windows.Forms.GroupBox freInsert;
        private System.Windows.Forms.GroupBox fraReadytransfers;
        private System.Windows.Forms.ListBox LstReadyCmd;
        private System.Windows.Forms.GroupBox freResult;
        private System.Windows.Forms.ListBox LstResult;
        private System.Windows.Forms.Button btn_RightSet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_leftSet;
        private System.Windows.Forms.Button btn_leftRead;
    }
}

