using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;

namespace MultiThreadServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        public void AddLogData(int family, int protocol, IPEndPoint ipandport, byte[] message)
        {
            ASCIIEncoding b = new ASCIIEncoding();
            string mess = b.GetString(message);
            Invoke(new Action(() => listBox5.Items.Add(mess)));
        }

        //Boton Start Server
        private void button1_Click(object sender, EventArgs e)
        {

            AddressFamily family = AddressFamily.InterNetwork;
            SocketType type;
            ProtocolType protocol = ProtocolType.IP;
            IPAddress ip;
            Int32 port;

            type = SocketType.Stream;

            bool gooddata = true;
            bool tcptrue = false;

            if (comboBox1.Items[this.comboBox1.SelectedIndex].ToString() == "IPv4") {
                family = AddressFamily.InterNetwork;
            }
            else
            {
                if(comboBox1.Items[this.comboBox1.SelectedIndex].ToString() == "IPv6")
                {
                    family = AddressFamily.InterNetworkV6;
                }
                else
                {
                    MessageBox.Show("Select an Address Type");
                    gooddata = false;
                }
            }

            if(comboBox2.Items[this.comboBox2.SelectedIndex].ToString() == "TCP")
            {
                protocol = ProtocolType.Tcp;
                type = SocketType.Stream;
                tcptrue = true;
            }
            else
            {
                if (comboBox2.Items[this.comboBox2.SelectedIndex].ToString() == "UDP")
                {
                    protocol = ProtocolType.Udp;
                    type = SocketType.Dgram;
                    tcptrue = false;
                }
                else
                {
                    MessageBox.Show("Select a Protocol");
                    gooddata = false;
                }
            }

            ip = IPAddress.Parse("127.0.0.1");

            port = 80;

            try
            {
                ip = IPAddress.Parse(textBox1.Text);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                gooddata = false;
            }

            try
            {
                port = Int32.Parse(textBox2.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                gooddata = false;
            }

            if(gooddata) {
                IPEndPoint ipend = new IPEndPoint(ip, port);

                Socket a = new Socket(family,type,protocol);


                if(tcptrue)
                {
                    Server servidorer = new Server(this);
                    servidorer.Start(a, ipend);
                }
                else
                {

                }
            }

        }
    }
}
