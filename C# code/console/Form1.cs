using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace console
{
    public partial class Form1 : Form
    {
        int mode = 0;
        const int PressUp = 0x000A;
        const int PressDown = 0x0008;
        const int UpKey = 0xC8;
        const int LeftKey = 0xCB;
        const int RightKey = 0xCD;
        const int DownKey = 0xD0;
        DirectX ip = new DirectX();
        string indata;
        Boolean portOpened = true;
        
        public Form1()
        {
            InitializeComponent();
            getPorts();
        }

        void getPorts()
        {
            try
            {
                String[] ports = SerialPort.GetPortNames();
                comboBox1.Items.AddRange(ports);
            }
            catch
            {
                MessageBox.Show("Unable To Get Port Names");
            }
        }

        void setPort(String port)
        {
            serialPort1.PortName = port;
            serialPort1.BaudRate = 9600;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String port = comboBox1.SelectedItem.ToString();
            button1.Enabled = true;
            setPort(port);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
            try
            {
                if (!serialPort1.IsOpen)
                    serialPort1.Open();
                portOpened = true;
                comboBox1.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (portOpened)
            {
                indata = serialPort1.ReadLine();
                try
                {
                    if (mode != 0)
                        this.Invoke(new EventHandler(displaydata));
                    else
                        this.Invoke(new EventHandler(pcMode));
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                }
            }
        }

        void pcMode(Object sender, EventArgs e)
        {   
            string str = indata;
            try
            {
                string[] axis = str.Split(':');
                int x = int.Parse(axis[0]);
                int y = int.Parse(axis[1]);
                int z = int.Parse(axis[2]);
                int scale = 5 * trackBar3.Value;
                textBox1.Text = x + "\t       " + y + "\t\t     " + z;

                    if (x >= 260 && x <= 270 + scale)//turn left
                    {
                            SendKeys.Send("{LEFT 1}");
                            up.Image = Properties.Resources.btn_play_up;
                            dn.Image = Properties.Resources.btn_play_dn;
                            rt.Image = Properties.Resources.btn_play_rt;
                            lt.Image = Properties.Resources.btn_play_alt;
                    }
                    else if (x >= 390 - scale && x <=400)//turn right
                    {
                            SendKeys.Send("{RIGHT 1}");
                            up.Image = Properties.Resources.btn_play_up;
                            dn.Image = Properties.Resources.btn_play_dn;
                            rt.Image = Properties.Resources.btn_play_art;
                            lt.Image = Properties.Resources.btn_play_lt;
                    }
                    if (y >= 395 - scale && y <= 400)//forward
                    {
                            SendKeys.Send("{UP 1}");
                            up.Image = Properties.Resources.btn_play_aup;
                            dn.Image = Properties.Resources.btn_play_dn;
                            rt.Image = Properties.Resources.btn_play_rt;
                            lt.Image = Properties.Resources.btn_play_lt;
                    }
                    else if (y>=265 && y <= 275 + scale)//backward
                    {
                        SendKeys.Send("{DOWN 1}");
                        up.Image = Properties.Resources.btn_play_up;
                        dn.Image = Properties.Resources.btn_play_adn;
                        rt.Image = Properties.Resources.btn_play_rt;
                        lt.Image = Properties.Resources.btn_play_lt;
                    }
                }
            catch
            {

            }
        }

        void displaydata(Object sender, EventArgs e)
        {
            string str = indata;
            try
            {
                string[] axis = str.Split(':');
                int x = int.Parse(axis[0]);
                int y = int.Parse(axis[1]);
                int z = int.Parse(axis[2]);

                textBox1.Text = x + "\t       " + y + "\t\t       " + z;
                if (x >= 270 && x <= 300)//turn left
                {
                        ip.Send_Key(LeftKey, PressDown);
                        ip.Send_Key(RightKey, PressUp);   
                }
                else if (x >= 350 && x <= 400)//turn right
                {
                        ip.Send_Key(RightKey, PressDown);
                        ip.Send_Key(LeftKey, PressUp);
                }
                else if (x > 301 && x < 349)
                {
                    ip.Send_Key(RightKey, PressUp);
                    ip.Send_Key(LeftKey, PressUp);
                }

                if (y >= 345 && y <= 390)//forward
                {
                        ip.Send_Key(UpKey, PressDown);
                        ip.Send_Key(DownKey, PressUp);
                 }
                else if (y >= 290 && y <= 310)//backward
                {
                        ip.Send_Key(DownKey, PressDown);
                        ip.Send_Key(UpKey, PressUp);
                }
                else if (y > 311 && y < 344)
                {
                        ip.Send_Key(UpKey, PressUp);
                        ip.Send_Key(DownKey, PressUp);
                }
            }
            catch
            {

            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            mode = 0;//pc mode
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            mode = 1;//game mode
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                portOpened = false;
                serialPort1.Close();
                serialPort1.DataReceived -= new System.IO.Ports.SerialDataReceivedEventHandler(serialPort1_DataReceived);
                button2.Enabled = false;
                button1.Enabled = true;
                comboBox1.Enabled = true;
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }
    }
}
