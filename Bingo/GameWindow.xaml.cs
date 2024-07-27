using Bingo.Classes;
using Bingo.Classes.Buttons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Bingo
{

    public partial class GameWindow : Window
    {
        private int size = 5;
        private GameType gameType;
        private Categories category;
        private GameManager gameManager;
        private DispatcherTimer secondTimer;
        private int currentTime = 10;
        private int[] numbers;
        private string path = "Data\\data.xml";
        private int counter;
        private XDocument doc;
        public GameWindow(int size, GameType gameType, Categories category)
        {
            InitializeComponent();
            this.size = size;
            this.gameType = gameType;
            this.category = category;
            gameManager = new GameManager();

            doc = XDocument.Load(path);
            if (category != Categories.Empty)
            {
                Debug.WriteLine(doc.Element("Main").Element(category.ToString()));
                XElement cat = doc.Element("Main").Element(category.ToString());
                counter = cat.Elements("Object").Count();
            }
            CreateGridOfButtons();
            secondTimer = new DispatcherTimer();
            secondTimer.Interval = TimeSpan.FromSeconds(1);
            secondTimer.Tick += SecondTimer_Tick;
            secondTimer.Start();

            txbTimer.Text = currentTime.ToString();

        }
        //zamysł jest taki ,żeby funkcja się wykonywała w nieskończonośc najlepiej żeby ..
        //trzeba by zrobić gettery i settery żeby uaktualniać stan planszy, wejdzie to w funkcje 'Gra' albo 'Update' w pliku 'Siec.cs'


        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            currentTime--;
            if (currentTime == 0)
            { 
                currentTime = 10;
                int generatedInt = gameManager.RandomValue(gameType, counter);
                if (gameType == 0) txbGeneratedNumber.Text = generatedInt.ToString();
                else
                {
                    doc = XDocument.Load(path);
                    XElement? temp = doc
                            .Descendants(category.ToString())
                            .Descendants("Object")
                            .FirstOrDefault(d => (int)d.Element("Id") == generatedInt);
                    if (temp != null)
                    {
                        txbGeneratedNumber.Text = temp.Element("Name").Value;
                    }
                }

                BingoNumberButton.currentGeneratedValue = txbGeneratedNumber.Text;
            }
            txbTimer.Text = currentTime.ToString();
        }

        private void CreateGridOfButtons()
        {
            Grid grid = new Grid();
            grid.Name = "Buttons";
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            grid.Margin = new Thickness(10, 10, 10, 10);
            for(int i=0; i < size; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1,GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }

            RandomNumbers(gameType);

            ButtonFactory buttonFactory;
            if(gameType == GameType.Numbers) buttonFactory = new BingoNumberButtonFactory();
            else buttonFactory = new BingoFindButtonFactory();
            
            for (int i = 0; i < size; i++)
            {
                for(int j=0; j< size; j++)
                {
                    IBingoButton bingoButton = buttonFactory.CreateButton();
                    bingoButton.OnClick += BingoButton_Clicked;
                    bingoButton.Id = i * size + j;
                    if (gameType == 0) bingoButton.Content = numbers[i * size + j].ToString();
                    else
                    {
                        doc = XDocument.Load(path);
                        XElement? temp = doc
                            .Descendants(category.ToString())
                            .Descendants("Object")
                            .FirstOrDefault(d => (int)d.Element("Id") == i * size + j);
                        if(temp != null)
                        {
                            bingoButton.Content = temp.Element("Name").Value;
                        }
                    }
                    Grid.SetRow((UIElement)bingoButton, j);
                    Grid.SetColumn((UIElement)bingoButton, i);
                    grid.Children.Add((UIElement)bingoButton);
                }
            }
            Grid main = (Grid)FindName("Main");
            Grid.SetRow(grid, 1);
            Grid.SetColumn(grid, 0);
            main.Children.Add(grid);
        }


        private void BingoButton_Clicked(object sender, EventArgs e)
        {
            if(CheckWinner())
            {
                secondTimer.Stop();
                MessageBox.Show("Win");
                //Tutaj wyslanie alertu do GameManager ze ktos wygral nie?
            }
            
        }
        
        private bool CheckWinner()
        {
            Grid main = (Grid)FindName("Main");
            foreach (var child in main.Children)
            {
                if (child is Grid buttons)
                {
                    bool[,] tab = new bool[size, size];
                    foreach (var childs in buttons.Children)
                    {
                        if (childs is IBingoButton bingoButton)
                        {
                            tab[bingoButton.Id / size, bingoButton.Id % size] = bingoButton.CurrentState;
                        }
                    }

                    if (CheckRows(tab) || CheckColumns(tab) || CheckDiagonals(tab)) return true;
                }
            }

            return false;
        }

        private bool CheckRows(bool[,] tab)
        {
            for (int i = 0; i < size; i++)
            {
                int sum = 0;
                for (int j = 0; j < size; j++)
                {
                    if (tab[i, j]) sum++;
                }
                if (sum == size)
                    return true;
            }
            return false;
        }

        private bool CheckColumns(bool[,] tab)
        {
            for (int j = 0; j < size; j++)
            {
                int sum = 0;
                for (int i = 0; i < size; i++)
                {
                    if (tab[i, j]) sum++;
                }
                if (sum == size)
                    return true;
            }
            return false;
        }

        private bool CheckDiagonals(bool[,] tab)
        {
            int sum = 0;
            for (int i = 0; i < size; i++)
            {
                if (tab[i, i]) sum++;
            }

            if (sum == size)  return true;
            else sum = 0;

            for (int i = 0; i < size; i++)
            {
                if (tab[i, size - i - 1]) sum++;
            }
            if (sum == size) return true;
            else return false;
        }

        private void RandomNumbers(GameType game)
        {
            numbers = new int[size * size];
            Random random = new Random();
            if (game == GameType.Numbers)
            { 
                int[] tab = { 1, 16, 31, 46, 61 };
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
            else
            {
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        int generated;
                        do
                        {
                            generated = random.Next(0,counter);
                        } while (numbers.Contains(generated));
                        numbers[i * size + j] = generated;
                    }
                }
            }
        }
    }
}
