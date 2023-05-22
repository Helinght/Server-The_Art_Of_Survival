using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

//Console.WriteLine("[",DateTime.Now,"]","(","INFO",")","");
class MyDataObject
    {
        public string Name { get; set; }
        public int Port { get; set; }
        public int Ip { get; set; }
    }

class Server
{
    static TcpListener listener;
    static List<TcpClient> clients = new List<TcpClient>();


    static void Main()
    {
        StartServer();
        

    }
    //JSON
    
    //start server
    public static void StartServer()
    {
        // Server config
        string json = System.IO.File.ReadAllText("server_config.json");
        MyDataObject data = JsonConvert.DeserializeObject<MyDataObject>(json);
    
        int port = data.Port;
        string ip = "127.0.0.1";

        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();

        ConsoleFunctionWrite("Server start", "INFO");
        ConsoleFunctionWrite("Ip - " + ip.ToString(), "INFO");
        ConsoleFunctionWrite("Port - " + port.ToString(), "INFO");
       
        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();

            Console.WriteLine("Подключен новый клиент");
            clients.Add(client); // Добавляем клиента в коллекцию

            Thread clientThread = new Thread(HandleClientChat);
            clientThread.Start(client);
        }
    }

    static void HandleClientChat(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead;

        while (true)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.Unicode.GetString(buffer, 0, bytesRead);
                ConsoleFunctionWrite("Message fot client" + message,"INFO");

                // Отправка сообщения всем клиентам
                BroadcastMessage(message);
            }
            catch
            {
                // Обработка отключения клиента
                ConsoleFunctionWrite("Dissconect player","INFO");
                client.Close();
                break;
            }
        }
    }
    static void BroadcastMessage(string message)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(message);

        foreach (TcpClient client in clients)
        {
            NetworkStream stream = client.GetStream();
            stream.Write(buffer, 0, buffer.Length);
        }
    }

    static void ConsoleFunctionWrite(string message, string type)
    {
        //INFO
        if (type == "INFO")
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[" + DateTime.Now + "]" + "(" + type + ")" + message);
            Console.ResetColor();
        }
        //ERROR
        if(type == "ERROR"){
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[" + DateTime.Now + "]" + "(" + type + ")" + message);
            Console.ResetColor();
        }
        //WARNING
        if (type == "WARNING")
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[" + DateTime.Now + "]" + "(" + type + ")" + message);
            Console.ResetColor();
        }

    }


    
 }


