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
    
    public partial class SoloChoose : Window
    {
        private int BoardSize { get; set; } = 5;
        private GameType GameType { get; set; } = GameType.Numbers;
        private Categories Category { get; set; } = Categories.Empty;

        private GameManager gameManager;

        public SoloChoose()
        {
            InitializeComponent();
            lstBoardSize.IsEnabled = false;
            lstCategory.IsEnabled = false;
            lstBoardSize.Items.Add("5x5");
            lstBoardSize.Items.Add("4x4");
            lstBoardSize.Items.Add("3x3");

            lstGameType.Items.Add("Liczby");
            lstGameType.Items.Add("Znajdz obiekt");

            lstCategory.Items.Add("Miasto");
            lstCategory.Items.Add("Wies");
            gameManager = new GameManager();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow(BoardSize, GameType, Category, gameManager);
            gameWindow.Show();
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
                    lstCategory.IsEnabled=true;
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
            switch(lstCategory.SelectedIndex)
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
