using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Windows.Xps.Serialization;
using System.Diagnostics;
using Bingo.Classes;
using System.Data;

namespace Bingo
{
  

    public enum Multi
    {
        Solo=0,
        Serwer=1,
        Klient=2
    }
    public class Siec
    {
        private int BoardSize { get; set; } = 5;
        private GameType GameType { get; set; } = GameType.Numbers;
        private Categories Category { get; set; } = Categories.Empty;
        static private GameManager gameManager;

        static TcpListener server;
        static TcpClient client;
        static string IP;
        static bool czySerwer;
        static GameWindow gameWindow;
        static string  wiadomosc="";
        static string msgSerwer="";
        static string msgClient="";


        public Siec(Multi multi,string ip,int bSize,GameType gType,Categories Cat)
        {
            IP = ip;
            GameType=gType;
            Category = Cat;
            gameManager = new GameManager();
            switch (multi)
            {
                case Multi.Serwer:
                    StartServerAsync( bSize,gType,Cat);
                    break;
                case Multi.Klient:
                    StartClient(bSize,gType,Cat);
                    break;
                default:
                    break;
            }
        }



        //nie tykać
        static async Task StartServerAsync(int bSize, GameType gType, Categories Cat)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                gameWindow = new GameWindow(bSize, gType, Cat, gameManager);
                gameWindow.Show();
            });
            //gameWindow = new GameWindow(bSize, gType, Cat);
            //gameWindow.Show();

            czySerwer= true;
            IPAddress ipAddress = IPAddress.Parse(GetLocalIPAddress());
            
            int port = 12345;

            server = new TcpListener(ipAddress, port);
            server.Start();
            Debug.WriteLine("Serwer uruchomiony...");
            Debug.WriteLine("Oczekiwanie na drugiego gracza...");

            client = await server.AcceptTcpClientAsync();
            Debug.WriteLine("Połączono z klientem.");


            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            Thread sendThread = new Thread(SendMessages);
            sendThread.Start();

            Thread graThread = new Thread(Gra);
            graThread.Start();

         
            //receiveThread.Join();
            //sendThread.Join();
            //graThread.Join();
        }


        static async Task StartClient(int bSize, GameType gType, Categories Cat)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                gameWindow = new GameWindow(bSize, gType, Cat, gameManager);
                gameWindow.Show();
            });
            //gameWindow = new GameWindow(bSize, gType, Cat);
            //gameWindow.Show();

            czySerwer = false;
            string serverIp = IP;
            int serverPort = 12345;

            client = new TcpClient();
            Debug.WriteLine("Łączenie z klientem...");
            await client.ConnectAsync(serverIp, serverPort);

            Debug.WriteLine("Połączono z serwerem.");

            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();

            Thread sendThread = new Thread(SendMessages);
            sendThread.Start();

            Thread graThread = new Thread(Gra);
            graThread.Start();

            //receiveThread.Join();
            //sendThread.Join();
            //graThread.Join();
        }

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
                    Debug.WriteLine(message + '\n');
                    msgSerwer = message;
                }
            }
            else
            {
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    //wiadomosc przesylana z serwera
                    Debug.WriteLine(message + '\n');
                    msgClient = message;
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
                    //string message = "Serwer: "+test ;
                    string message = wiadomosc;
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
                    //string message = "Klient: "+ test;
                    string message = wiadomosc;
                    byte[] data = Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);

                }
            }


        }
        //Tu ogólne getery/setery
        //przesyłana jest 'wiadomosc'
        static void Update()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                wiadomosc = gameWindow.numer();
            });
        }
        static void Gra()
        {

            while (true)
            {
                Thread.Sleep(120);

                //tu setery/getery szzczególne dla  
                //msgSerwer/msgClient -to informacja DLA Serwera/Klienta
                if (czySerwer)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        gameWindow.przeslana.Text = msgSerwer;
                    });
                    
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        gameWindow.przeslana.Text = msgClient;
                    });
                }

                Update();
            }

        }
    }
}
