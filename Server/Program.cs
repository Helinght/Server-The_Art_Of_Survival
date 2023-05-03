using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Program

//Console.WriteLine("[",DateTime.Now,"]","(","INFO",")","");
/* https://russianblogs.com/article/30793054479/ */
{
    static void Main(string[] args)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        var Port = 6324;
        IPEndPoint IpAndPort = new IPEndPoint(ipAddress, Port);

        serverSocket.Bind(IpAndPort);
        serverSocket.Listen(0);
        Console.WriteLine("[",DateTime.Now,"]","(","INFO",")","Server start!");
        Console.WriteLine("[", DateTime.Now, "]", "(", "INFO", ")", "Ip - ",ipAddress);
        Console.WriteLine("[", DateTime.Now, "]", "(", "INFO", ")", "Port - ",Port);
        serverSocket.BeginAccept(AcceptCallBack, serverSocket);
    }
}
