using Bingo.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;

namespace Bingo
{

    public partial class GameWindow : Window
    {
        private int size = 5;
        private GameType gameType;
        private GameManager gameManager;
        private DispatcherTimer secondTimer;
        private int currentTime = 10;
        
        public GameWindow(int size, GameType gameType)
        {
            InitializeComponent();
            this.size = size;
            this.gameType = gameType;
            gameManager = new GameManager();
            CreateGridOfButtons();


            secondTimer = new DispatcherTimer();
            secondTimer.Interval = TimeSpan.FromSeconds(1);
            secondTimer.Tick += SecondTimer_Tick;
            secondTimer.Start();

            txbTimer.Text = currentTime.ToString();

        }


        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            currentTime--;
            if (currentTime == 0)
            { 
                currentTime = 10;
                txbGeneratedNumber.Text = gameManager.RandomNumber().ToString();
            }
            txbTimer.Text = currentTime.ToString();
        }


        private void CreateGridOfButtons()
        {
            Grid grid = new Grid();
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.Margin = new Thickness(20, 50, 20, 30);
            for(int i=0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            RandomNumbers();
            
            for (int i = 0; i < size; i++)
            {
                for(int j=0; j< size; j++)
                {
                    BingoButton bingoButton = new BingoButton();
                    
                    if (gameType == 0) bingoButton.Content = numbers[i * size + j];
                    Grid.SetRow(bingoButton, j);
                    Grid.SetColumn(bingoButton, i);
                    grid.Children.Add(bingoButton);
                }
            }
            Grid main = (Grid)FindName("Main");
            Grid.SetRow(grid, 1);
            Grid.SetColumn(grid, 0);
            main.Children.Add(grid);
        }

        private int[] numbers;

        private void RandomNumbers()
        {
            numbers = new int[size*size];
            int[] tab = { 1, 16, 31, 46, 61 };
            Random random = new Random();
            for (int i = 0; i < size; i++) 
            {
                for(int j =0; j < size; j++)
                { 
                    int generated;
                    do
                    {
                        generated = random.Next(tab[i], tab[i] + 15);
                    } while (numbers.Contains(generated));
                    numbers[i * size + j] = generated;
                }
            }
        }
    }
}
