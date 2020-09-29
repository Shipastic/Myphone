using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Myphone
{
    class Program
    {
        Random rand = new Random();
        static int port = 8000;
        static string host = "127.0.0.1";
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Run();
            Console.WriteLine("Stop");
            Console.ReadKey();
        }
        public void Run()
        {
            Console.Write("1 : Server, 2 : Client >");
            string str = Console.ReadLine();
            Phone phone;

            if (str == "1")
                phone = new PhoneServer(port);
            else phone = new PhoneClient(host, port);
            phone.Recv += Recv1;
            phone.Start();
            while (true)
            {
                phone.Send((byte)rand.Next(10, 100));
                Thread.Sleep(2000);
            }
        }
        public void Recv1(byte data)
        {
            Console.WriteLine("Recv" + data.ToString());

        }
    }
}
