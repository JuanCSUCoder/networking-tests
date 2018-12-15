using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using MultiThreadServer;

namespace MultiThreadServer
{
    class Server
    {
        public bool copyRecieved;
        public byte[] dataReceived;

        public bool dataSended;
        public byte[] dataToSend;

        public bool clientprocessing;

        Form1 formulario;

        public Server(Form1 oliguis)
        {
            formulario = oliguis;
        }

        public void Start(Socket a, IPEndPoint iPEnd)
        {

            Thread bla = new Thread(() => ServerLife(a, iPEnd));
            bla.Start();
        }

        public void ServerLife(Socket a, IPEndPoint iPEnd)
        {

            a.Bind(iPEnd);
            a.Listen(int.MaxValue);
            while (true)
            {

                Socket client = a.Accept();

                Thread clientManag = new Thread(() => ClientManager(client));
                clientManag.Start();
            }
        }

        public void SendThis(byte[] message)
        {
            dataToSend = message;
            dataSended = false;
        }

        public void ClientManager(Socket client)
        {
            copyRecieved = true;
            dataSended = true;
            Thread bieb = new Thread(() => ClientReciever(client));
            bieb.Start();
            while (true)
            {
                if (!clientprocessing)
                {
                    clientprocessing = true;
                    if (!copyRecieved)
                    {
                        MessageBox.Show("0 Success");
                        formulario.AddLogData((int)client.AddressFamily, (int)client.ProtocolType, (IPEndPoint)client.LocalEndPoint, dataReceived);
                        copyRecieved = true;
                    }
                    if (!dataSended)
                    {
                        client.Send(dataToSend);
                        dataSended = true;
                    }
                }
                else { Thread.Sleep(10); }
                clientprocessing = false;
            }
        }

        public void ClientReciever(Socket client)
        {
            bool connected = true;
            while (connected)
            {
                if (IsSocketConnected(client))
                {
                    MessageBox.Show(Convert.ToString(client.Connected));
                    byte[] a = new byte[255];
                    client.Receive(a);
                    while (!copyRecieved)
                    {
                        Thread.Sleep(50);
                    }
                    dataReceived = a;
                    copyRecieved = false;
                }
                else { connected = false; MessageBox.Show("Disconnected"); }
            }
        }

        public bool IsSocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
                return false;
            else
                return true;
        }

    }
}
