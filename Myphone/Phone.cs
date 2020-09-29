using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Myphone
{
    public delegate void deRecv(byte data);
   abstract class Phone
    {
        protected NetworkStream ns;
        protected int port;
        protected string host;
        public deRecv Recv;

        public void Start()
        {
            Thread thread = new Thread(Waiter);
            thread.Start();
        }
        public void Waiter()
        {
            while (true)
            {
                Connect();
                while (true)
                {
                    
                try
                    {
                        int data = ns.ReadByte();
                        if (data != -1)
                            Recv((byte)data);
                    }
                    catch
                    {
                        Thread.Sleep(100);
                        break;
                    }
                }
            }
        }
        abstract public void Connect ();
     
        public bool Send(byte data)
        {
            try
            {
                ns.WriteByte(data);
                Console.WriteLine("Send " + data.ToString());
                return true;
            }
            catch
            {
                Thread.Sleep(100);
                Console.WriteLine("Error sending " + data.ToString());
                return false;

            }
        }
    }
    class PhoneServer : Phone
    {
        public PhoneServer(int port)
        {
            this.port = port;
        }
       override public void Connect()
        {
            try
            {
                Console.WriteLine("Starting Server");
                TcpListener listener = new TcpListener(IPAddress.Any, port);
                listener.Start();
                TcpClient tcpClient = listener.AcceptTcpClient();
                ns = tcpClient.GetStream();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
    class PhoneClient : Phone
    {
        public PhoneClient(string host, int port)
        {
            this.host = host;
            this.port = port;
        }
       override public void Connect()
        {
            try
            {
                Console.WriteLine("Starting Server");
                TcpClient tcpClient = new TcpClient(host, port);
                ns = tcpClient.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
