using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using System.Threading;

namespace MultiThreadServer
{
    class Server
    {
        public bool copyRecieved;
        public byte[] dataReceived;

        public bool dataSended;
        public byte[] dataToSend;

        public void Start(Socket a, IPEndPoint iPEnd)
        {
            a.Bind(iPEnd);
            a.Listen(int.MaxValue);

            Thread bla = new Thread(() => ServerLife(a));
        }

        public void ServerLife(Socket a)
        {
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
            while (true)
            {
                if (!copyRecieved)
                {
                    Form1.AddLogData((int)client.AddressFamily,(int)client.ProtocolType,(IPEndPoint)client.LocalEndPoint);
                    copyRecieved = true;
                }
                if (!dataSended)
                {
                    client.Send(dataToSend);
                    dataSended = true;
                }
            }
        }

        public void ClientReciever(Socket client)
        {
            while (true)
            {
                byte[] a = new byte[255];
                client.Receive(a);
                while (!copyRecieved)
                {
                    Thread.Sleep(50);
                }
                dataReceived = a;
            }
        }

    }
}
