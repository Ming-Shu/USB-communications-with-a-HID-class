using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace USB_HID_Communication
{
    ///<summary>
    /// Project: USB_HID
    /// 
    /// ***********************************************************************
    /// Software License Agreement
    ///
    /// Licensor grants any person obtaining a copy of this software ("You") 
    /// a worldwide, royalty-free, non-exclusive license, for the duration of 
    /// the copyright, free of charge, to store and execute the Software in a 
    /// computer system and to incorporate the Software or any portion of it 
    /// in computer programs You write.   
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// ***********************************************************************
    /// 
    /// Author             
    /// Ming-Shu Wu         
    /// 
    /// This software was written using Visual Studio 2013 for Windows
    /// Desktop building for the .NET Framework v4.5.
    /// 
    /// Purpose: 
    /// Demonstrates USB communications with a generic HID-class device(control Output and interrupt Input only in code)
    /// 
    /// Requirements:
    /// Windows Vista or later and an attached USB generic Human Interface Device (HID).
    /// (Does not run on Windows XP or earlier because .NET Framework 4.5 will not install on these OSes.) 
    /// 
    /// Description:
    /// Finds an attached device that matches the vendor and product IDs in the form's text boxes.
    /// 
    /// Retrieves the device's capabilities.
    /// Sends and requests HID reports.
    /// 
    public partial class Form1 : Form
    {
        public const int COLUMN_NUM = 10, ROW_NUM = 4;
        Label[,] label = new Label[ROW_NUM, COLUMN_NUM];
        TextBox[,] textbox = new TextBox[ROW_NUM, COLUMN_NUM];
        public const int X_SHIFT = 120, Y_SHIFT = 33;
        int item_no = 0;//One command consist of continuous bytes of ID
        int ByteNum;
        bool sendWaitData_flag = false;
        bool DeviceConnected_flag = false;
        public struct FieldCount
        {
            public bool mutiFlag;
            public int ColumnMod;
            public int ColumnNum;
            public int columnShift;
        }

        String[,] sendWaitData = new String[100, COLUMN_NUM * ROW_NUM];
        int[] recordWaitDataNum = new int[COLUMN_NUM * ROW_NUM];//Store Wait Data byte number,and array No is the same as Wait Data No.
        int WaitDateNo = 0; //The command consist of continuous bytes of item number.
        int recordWaitDateNo = 0;//Store Wait Data No.
        bool Read_flag = false;

        public Form1()
        {
            InitializeComponent();
            sendWaitDataInit();
            periodicTimer();
        }

        /// <summary>
        /// Create a array as buffer to save about data 
        /// </summary>
        private void sendWaitDataInit()
        {
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < COLUMN_NUM * ROW_NUM; j++)
                {
                    sendWaitData[i, j] = null;
                    recordWaitDataNum[j] = 0;
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DeviceConnected_flag = false;
            txtVendorID.Text = "";
            txtProductID.Text = "";
            LstResult.Items.Clear();
            LstStatus.Items.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            CboByte.Items.Add("Default");
            for (i = 1; i <= 20; i++)
            {
                String byteNumber = String.Format("{0} Byte", i);
                CboByte.Items.Insert(i, byteNumber);
            }
            CboByte.SelectedIndex = 8;
            panel1.AutoScroll = true;
            LstReadyCmd.SelectedIndex = LstReadyCmd.Items.Count - 1;
            LstReadyCmd.HorizontalScrollbar = true;

        }

        private void btn_BytesSet_Click(object sender, EventArgs e)
        {
            if (DeviceConnected_flag)
            {
                item_no = 0;
                sendWaitData_flag = true;
                Read_flag = false;
                panel1.Controls.Clear();
                ByteNum = Convert.ToInt32(CboByte.SelectedIndex);
                if (ByteNum > _myHid.Capabilities.OutputReportByteLength)
                {
                    ByteNum = _myHid.Capabilities.OutputReportByteLength;
                    MessageBox.Show("The Device buffer only " + ByteNum.ToString() + "Byte");
                }

                InitialTextBox(ByteNum);
            }
            else
            {
                MessageBox.Show("Device hasn't been detected!");
            }
        }

        private FieldCount Field_Count(int Number)
        {
            FieldCount _field = new FieldCount();
            _field.columnShift = 0;
            _field.ColumnMod = 0;


            if (Number > ROW_NUM)
            {
                _field.mutiFlag = true;
                _field.ColumnNum = Number / ROW_NUM;
                _field.ColumnMod = Number % ROW_NUM;

                if (_field.ColumnMod == 0)
                    _field.columnShift = 0;
                else
                    _field.columnShift = 1;
            }
            else
            {
                _field.mutiFlag = false;
                _field.ColumnNum = 1;
                _field.columnShift = 0;
                _field.ColumnMod = Number % ROW_NUM;
            }
            return _field;
        }

        private void InitialTextBox(int Number)
        {
            FieldCount _field = Field_Count(Number);

            for (int j = 0; j < _field.ColumnNum + _field.columnShift; j++)
            {
                if (_field.ColumnMod == 0 || (_field.mutiFlag == true && j < _field.ColumnNum))
                {
                    for (int i = 0; i < ROW_NUM; i++)
                    {
                        label[i, j] = new Label();
                        label[i, j].Name = item_no.ToString();
                        label[i, j].Text = "Byte " + item_no.ToString();
                        label[i, j].AutoSize = true;
                        label[i, j].Location = new Point(33 + j * X_SHIFT, Y_SHIFT * i + 2);
                        label[i, j].Font = new System.Drawing.Font("Buxton Sketch", 10);
                        panel1.Controls.Add(label[i, j]);

                        textbox[i, j] = new TextBox();
                        textbox[i, j].Name = item_no.ToString();
                        textbox[i, j].Width = 50;
                        textbox[i, j].Height = 22;
                        textbox[i, j].Location = new Point(80 + j * X_SHIFT, Y_SHIFT * i);
                        textbox[i, j].Font = new System.Drawing.Font("Buxton Sketch", 13);
                        textbox[i, j].MaxLength = 2;
                        textbox[i, j].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
                        panel1.Controls.Add(textbox[i, j]);
                        item_no++;
                    }
                }
                else
                {
                    for (int i = 0; i < _field.ColumnMod; i++)
                    {
                        label[i, j] = new Label();
                        label[i, j].Name = item_no.ToString();
                        label[i, j].Text = "Byte " + item_no.ToString();
                        label[i, j].AutoSize = true;
                        label[i, j].Location = new Point(33 + j * X_SHIFT, Y_SHIFT * i + 2);
                        label[i, j].Font = new System.Drawing.Font("Buxton Sketch", 10);
                        panel1.Controls.Add(label[i, j]);

                        textbox[i, j] = new TextBox();
                        textbox[i, j].Name = item_no.ToString();
                        textbox[i, j].Width = 50;
                        textbox[i, j].Height = 22;
                        textbox[i, j].Location = new Point(80 + j * X_SHIFT, Y_SHIFT * i);
                        textbox[i, j].Font = new System.Drawing.Font("Buxton Sketch", 13);
                        textbox[i, j].MaxLength = 2;
                        textbox[i, j].KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
                        panel1.Controls.Add(textbox[i, j]);
                        item_no++;
                    }
                }
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {//Only input 0~9 and A~F
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar >= '0' && e.KeyChar <= '9') || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;//mean is pass
            }
        }

        private void btn_RightSet_Click(object sender, EventArgs e)
        {
            FieldCount field;
            String cmd = "";
            int lastNum = 0;
            LstReadyCmd.Enabled = true;
            if (Read_flag == true)//User click read data button,and data item from show wait data of listbox
                field = Field_Count(recordWaitDataNum[recordWaitDateNo]);
            else
                field = Field_Count(ByteNum);

            if (sendWaitData_flag == true)
            {
                for (int j = 0; j < field.ColumnNum + field.columnShift; j++)
                {
                    if (field.ColumnMod == 0 || (field.mutiFlag == true && j < field.ColumnNum))
                    {
                        for (int i = 0; i < ROW_NUM; i++)
                        {
                            if (string.IsNullOrWhiteSpace(textbox[i, j].Text))
                                textbox[i, j].Text = "00";

                            if (Read_flag == true)
                            {
                                sendWaitData[recordWaitDateNo, Convert.ToInt32(textbox[i, j].Name)] = textbox[i, j].Text;
                                cmd = cmd + "0x" + textbox[i, j].Text + " , ";
                            }
                            else//User click 'send data button' that send data from 'textbox group'to wait data of listbox.
                            {
                                sendWaitData[WaitDateNo, Convert.ToInt32(textbox[i, j].Name)] = textbox[i, j].Text;
                                lastNum = Convert.ToInt32(textbox[i, j].Name);
                                cmd = cmd + "0x" + textbox[i, j].Text + " , ";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < field.ColumnMod; i++)
                        {
                            if (string.IsNullOrWhiteSpace(textbox[i, j].Text))
                                textbox[i, j].Text = "00";

                            if (Read_flag == true)
                            {
                                sendWaitData[recordWaitDateNo, Convert.ToInt32(textbox[i, j].Name)] = textbox[i, j].Text;
                                cmd = cmd + "0x" + textbox[i, j].Text + " , ";
                            }
                            else
                            {
                                sendWaitData[WaitDateNo, Convert.ToInt32(textbox[i, j].Name)] = textbox[i, j].Text;
                                lastNum = Convert.ToInt32(textbox[i, j].Name);
                                cmd = cmd + "0x" + textbox[i, j].Text + " , ";
                            }
                        }
                    }
                }

                if (Read_flag == true)
                {
                    LstReadyCmd.Items.RemoveAt(recordWaitDateNo);//Remove the item  at the specified index of the number.
                    LstReadyCmd.Items.Insert(recordWaitDateNo, cmd);
                    Read_flag = false;
                }
                else
                {
                    recordWaitDataNum[WaitDateNo] = lastNum + 1;
                    LstReadyCmd.Items.Add(cmd);
                    WaitDateNo++;
                }

            }

        }

        private void btn_leftSet_Click(object sender, EventArgs e)//Delete  which item is selected
        {
            object item = LstReadyCmd.SelectedItem;
            int itemNo = LstReadyCmd.SelectedIndex;
            panel1.Controls.Clear();
            Read_flag = false;
            if (sendWaitData_flag == true && itemNo >= 0)
            {
                LstReadyCmd.Items.Remove(item);
                for (int i = itemNo; i < WaitDateNo; i++)//itemNo is selected ,and WaitDateNo is total wait number in listbox. 
                {
                    for (int j = 0; j < recordWaitDataNum[i + 1]; j++)
                    {
                        sendWaitData[i, j] = sendWaitData[i + 1, j];
                    }
                    recordWaitDataNum[i] = recordWaitDataNum[i + 1];
                }
                WaitDateNo--;
            }
        }

        private void btn_leftRead_Click(object sender, EventArgs e)
        {
            //object item = LstReadyCmd.SelectedItem;
            int itemNo = LstReadyCmd.SelectedIndex;//itemNo is item of ID that is selected
            item_no = 0;//One command consist of continuous bytes of ID
            recordWaitDateNo = itemNo;
            LstReadyCmd.Enabled = false;//Lock to Item that Selected
            if (sendWaitData_flag == true && itemNo >= 0)
            {
                Read_flag = true;
                panel1.Controls.Clear();

                int packetLength = recordWaitDataNum[itemNo];
                if (packetLength > _myHid.Capabilities.OutputReportByteLength)
                    packetLength = _myHid.Capabilities.OutputReportByteLength;

                FieldCount field = Field_Count(packetLength);//recordWaitDataNum[itemNo] is continuous bytes of number that is selected in listbox.
                InitialTextBox(recordWaitDataNum[itemNo]);

                for (int j = 0; j < field.ColumnNum + field.columnShift; j++)
                {
                    if (field.ColumnMod == 0 || (field.mutiFlag == true && j < field.ColumnNum))
                    {
                        for (int i = 0; i < ROW_NUM; i++)
                        {
                            textbox[i, j].Text = sendWaitData[itemNo, Convert.ToInt32(textbox[i, j].Name)];//textbox[i, j].Name is same as bytes of ID
                        }
                    }
                    else
                    {
                        for (int i = 0; i < field.ColumnMod; i++)
                        {
                            textbox[i, j].Text = sendWaitData[itemNo, Convert.ToInt32(textbox[i, j].Name)];
                        }
                    }
                }//for
            }
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceConnected_flag = true;
                FindMyHid();
            }
            catch (Exception ex)
            {
                DisplayException(Name, ex);
                throw;
            }
        }// private void btn_leftRead_Click()

        private void btn_send_Click(object sender, EventArgs e)
        {
            int itemCount = 0;
            int itemNo = LstReadyCmd.SelectedIndex;

            if (itemNo >= 0 && _deviceHandleObtained && timerFlag!=true)
            {
                btn_send.Enabled = false;
                LstReadyCmd.Enabled = false;//Lock to Item that Selected
                var output_Buffer = new Byte[recordWaitDataNum[itemNo]];
                FieldCount field = Field_Count(recordWaitDataNum[itemNo]);//recordWaitDataNum[itemNo] is continuous bytes of number that is selected in listbox.

                for (int j = 0; j < field.ColumnNum + field.columnShift; j++)
                {
                    if (field.ColumnMod == 0 || (field.mutiFlag == true && j < field.ColumnNum))
                    {
                        for (int i = 0; i < ROW_NUM; i++, itemCount++)
                            output_Buffer[itemCount] = Convert.ToByte(sendWaitData[itemNo, Convert.ToInt32(textbox[i, j].Name)], 16);//textbox[i, j].Name is same as bytes of ID
                    }
                    else
                    {
                        for (int i = 0; i < field.ColumnMod; i++, itemCount++)
                            output_Buffer[itemCount] = Convert.ToByte(sendWaitData[itemNo, Convert.ToInt32(textbox[i, j].Name)], 16);
                    }
                }//for
                SendOutputReport(output_Buffer, recordWaitDataNum[itemNo]);
                ShowInfoToForm(FormTypes.ListBoxResult, "Send an Output report message:  " + LstReadyCmd.Items[itemNo].ToString());
                LstReadyCmd.Enabled = true;
                btn_send.Enabled = true;
            }
        }

        private void btn_receive_Click(object sender, EventArgs e)
        {
            if (_deviceHandleObtained)
                ReceiveInputReport();
        }

        private void btn_continues_Click(object sender, EventArgs e)
        {
            if (btn_continues.Text != "Stop")
            {
                btn_continues.Text = "Stop";
                _periodicSend.Start();
            }
            else 
            {
                _periodicSend.Stop();
                btn_continues.Text = "Continues Receive (Interrupt)";
            }
        }

        ///  <summary>
        ///  Provides a central mechanism for exception handling.
        ///  Displays a message box that describes the exception.
        ///  </summary>
        ///  
        internal static void DisplayException(String moduleName, Exception e)
        {
            //  Create an error message.
            String message = "Exception: " + e.Message + Environment.NewLine + "Module: " + moduleName + Environment.NewLine + "Method: " + e.TargetSite.Name;
            const String caption = "Unexpected Exception";
            MessageBox.Show(message, caption, MessageBoxButtons.OK);
            Debug.Write(message);

            // Get the last error and display it. 
            Int32 error = Marshal.GetLastWin32Error();
            Debug.WriteLine("The last Win32 Error was: " + error);
        }//DisplayException



    }
}
