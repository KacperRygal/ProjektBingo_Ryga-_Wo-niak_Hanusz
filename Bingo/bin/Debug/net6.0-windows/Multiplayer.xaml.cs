using Bingo.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bingo
{
    /// <summary>
    /// Logika interakcji dla klasy Multiplayer.xaml
    /// </summary>
    public partial class Multiplayer : Window
    {
        private int BoardSize { get; set; } = 5;
        private GameType GameType { get; set; } = GameType.Numbers;
        private Categories Category { get; set; } = Categories.Empty;

        public Multiplayer()
        {
            InitializeComponent();
            this.txtIPserw.Text = EncodeIpToBase64(GetLocalIPAddress());

            lstBoardSize.IsEnabled = false;
            lstCategory.IsEnabled = false;
            lstBoardSize.Items.Add("5x5");
            lstBoardSize.Items.Add("4x4");
            lstBoardSize.Items.Add("3x3");

            lstGameType.Items.Add("Liczby");
            lstGameType.Items.Add("Znajdz obiekt");

            lstCategory.Items.Add("Miasto");
            lstCategory.Items.Add("Wies");
        }

        public static string EncodeIpToBase64(string ipAddress)
        {
            string[] octets = ipAddress.Split('.');
            if (octets.Length != 4)
            {
                throw new ArgumentException("Nieprawidłowy adres IP.");
            }

            byte[] bytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = byte.Parse(octets[i]);
            }

            return Convert.ToBase64String(bytes);
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

        private void Serwer_Click(object sender, RoutedEventArgs e)
        {
            Siec siec = new Siec(Multi.Serwer, this.daneIP.Text, BoardSize, GameType, Category);
            this.Close();
        }

        private void Klient_Click(object sender, RoutedEventArgs e)
        {
            Siec siec=new Siec(Multi.Klient,this.daneIP.Text, BoardSize, GameType, Category);
            this.Close();
        }


        private void lstGameType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (lstGameType.SelectedIndex)
            {
                case 0:
                default:
                    GameType = GameType.Numbers;
                    BoardSize = 5;
                    lstBoardSize.IsEnabled = false;
                    lstCategory.IsEnabled = false;
                    break;
                case 1:
                    GameType = GameType.FindObjects;
                    lstBoardSize.IsEnabled = true;
                    lstCategory.IsEnabled = true;
                    break;
            }
        }

        private void lstBoardSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GameType == GameType.FindObjects)
            {
                switch (lstBoardSize.SelectedIndex)
                {
                    case 0:
                    default:
                        BoardSize = 5;
                        break;
                    case 1:
                        BoardSize = 4;
                        break;
                    case 2:
                        BoardSize = 3;
                        break;
                }
            }

        }

        private void lstCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (lstCategory.SelectedIndex)
            {
                case 0:
                default:
                    Category = Categories.Miasto;
                    break;
                case 1:
                    Category = Categories.Wies;
                    break;
            }
        }
    }
}
