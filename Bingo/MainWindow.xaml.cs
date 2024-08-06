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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bingo
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Top = Properties.Settings.Default.WindowTop;
            Left = Properties.Settings.Default.WindowLeft;
        }

        private void btnSolo_Click(object sender, RoutedEventArgs e)
        {
            
            SoloChoose soloChoose = new SoloChoose();
            soloChoose.Show();
            this.Close();
        }

        private void btnMulti_Click(object sender, RoutedEventArgs e)
        {
            Multiplayer multiplayer = new Multiplayer();
            multiplayer.Show();
            this.Close();
        }
    }
}
