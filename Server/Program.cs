using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
//Console.WriteLine("[",DateTime.Now,"]","(","INFO",")","");
/* https://russianblogs.com/article/30793054479/ */
class Server
{
    static void Main(string[] args)
    {
        StartServerAsync();
    }

    public static void StartServerAsync()
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        var Port = 6324;
        IPEndPoint IpAndPort = new IPEndPoint(ipAddress, Port);

        serverSocket.Bind(IpAndPort);
        serverSocket.Listen(0);
        Console.WriteLine("[", DateTime.Now, "]", "(", "INFO", ")", "Server start!");
        Console.WriteLine("[", DateTime.Now, "]", "(", "INFO", ")", "Ip - ", ipAddress);
        Console.WriteLine("[", DateTime.Now, "]", "(", "INFO", ")", "Port - ", Port);
        serverSocket.BeginAccept(AcceptCallBack, serverSocket);
    }

    public static void AcceptCallBack(IAsyncResult ar)
    {
        Socket serverSocket = ar.AsyncState as Socket;
        Socket clientSocket = serverSocket.EndAccept(ar);
        string start_message = "Global server is starting.";
        byte[] data = System.Text.Encoding.UTF8.GetBytes(start_message);
        clientSocket.Send(data);
        clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
        serverSocket.BeginAccept(AcceptCallBack, serverSocket);
    }
    public static byte[] dataBuffer = new byte[1024];
    public static void ReceiveCallBack(IAsyncResult ar)
    {
        Socket clientSocket = null;
        try
        {
            clientSocket = ar.AsyncState as Socket;
            int count = clientSocket.EndReceive(ar);
            if (count == 0)
            {
                clientSocket.Close();
                return;
            }
            string msg = Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine("Получить данные от клиента:" + msg);

            clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);// асинхронный прием

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
        }
    }
}
