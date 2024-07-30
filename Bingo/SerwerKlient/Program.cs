using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class Program
{
    static TcpListener server;
    static TcpClient client;

    static string IP;
    static bool czySerwer;

    //nie tykać
    static void StartServer()
    {
        Console.Title = "Serwer";
        czySerwer = true;
        IPAddress ipAddress = IPAddress.Parse(GetLocalIPAddress()); 
        int port = 12345; 

        server = new TcpListener(ipAddress, port);
        server.Start();
        Console.WriteLine("Serwer uruchomiony...");
        Console.WriteLine("IP serwera: " + ipAddress);
        Console.WriteLine("Oczekiwanie na drugiego gracza...");
        client = server.AcceptTcpClient();
        Console.WriteLine("Połączono z klientem.");

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        Thread sendThread = new Thread(SendMessages);
        sendThread.Start();

        Thread graThread = new Thread(Gra);
        graThread.Start();

        receiveThread.Join();
        sendThread.Join();
        graThread.Join();
    }
    //nie tykać
    static void StartClient()
    {
        Console.Title = "Klient";

        czySerwer = false;
        string serverIp = IP; 
        int serverPort = 12345; 

        client = new TcpClient();
        client.Connect(serverIp, serverPort);
        Console.WriteLine("Połączono z serwerem.");

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        Thread sendThread = new Thread(SendMessages);
        sendThread.Start();

        Thread graThread = new Thread(Gra);
        graThread.Start();

        receiveThread.Join();
        sendThread.Join();
        graThread.Join();
    }
    //nie tykać
    static string GetLocalIPAddress()
    {
        string ipAddress = "";
        IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());

        foreach (IPAddress ip in hostEntry.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                ipAddress = ip.ToString();
                break;
            }
        }
      
        return ipAddress;
    }

    static void ReceiveMessages()
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];

        if (czySerwer)
        {
            while (true)
            {
               int bytesRead = stream.Read(buffer, 0, buffer.Length);
               string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //wiadomosc przesylana z klienta
                Console.Write(message+'\n');
            }
        }
        else
        {
            while(true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                //wiadomosc przesylana z serwera
                Console.Write(message + '\n');
            }
        }
    }

    static void SendMessages()
    {
        NetworkStream stream = client.GetStream();

        if (czySerwer)
        {
            while (true)
            {
                Thread.Sleep(500);
                //wiadomość przesyłana do klienta
                string message = "Serwer: "+Console.ReadLine();

                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }
        else 
        {
            while (true)
            {
                Thread.Sleep(500);
                //wiadomość przesyłana do serwera
                string message = "Klient: "+Console.ReadLine();
                
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                
            }
        }

       
    }
    static void Update()
    {

        //aktualizująca i zapętlająca się logika gry
    }
    static void Gra()
    {

        //pętla zarządzająca logiką gry
        //wszelkie jej ustawienia powinny być ustawiane przed pętlą
        while (true)
        {
            Thread.Sleep(120);

                    //nie wiem czy ruchy bedą się różniły 
                    //nie powinny ale daje możliwość rozróżnienia
                if (czySerwer)
                {
                   // 'ruchy' serwera
                }
                else
                {
                    // 'ruchy' klienta
                }
 
            Update();
        }
    }

    static void Main()
    {
        Console.Write("Wybierz opcję:\n1. Uruchom jako serwer\n2. Uruchom jako klient\nWybór: ");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1:
                StartServer();
                break;
            case 2:
                Console.Write("Podaj IP: \n");
                IP = Console.ReadLine();
                StartClient();
                break;
            default:
                Console.WriteLine("Niepoprawny wybór. Program zostanie zamknięty.");
                break;
        }
    }
}