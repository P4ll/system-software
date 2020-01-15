using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;

namespace TestService
{
    public partial class MyService : ServiceBase
    {
        private const string IP_ADR = "127.0.0.1";
        private const int PORT = 8005;
        private const string MESSAGE = "Танькин Андрей, ИВТВМбд-31";
        private Thread thread;

        public MyService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            thread = new Thread(startListening);
            thread.Start();
        }

        private void startListening()
        {
            try
            {
                IPAddress ipAddr = IPAddress.Parse(IP_ADR);
                TcpListener tcpList = new TcpListener(ipAddr, PORT);
                tcpList.Start();
                Byte[] buff = new Byte[256];

                while (true)
                {
                    Socket socket = tcpList.AcceptSocket();
                    socket.Send(Encoding.Unicode.GetBytes(MESSAGE));
                    socket.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnStop()
        {

        }
    }
}
