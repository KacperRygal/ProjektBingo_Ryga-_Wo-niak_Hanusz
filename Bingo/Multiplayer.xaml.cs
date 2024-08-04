using Bingo.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
