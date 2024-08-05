using Bingo.Classes.Buttons;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Linq;

namespace Bingo.Classes
{
    public class GameManager
    {
        private DispatcherTimer secondTimer;
        private int currentTime;
        private GameType gameType;
        private Categories category;
        private XDocument doc;
        private string path = "Data\\data.xml";
        private int counter;
        private GameWindow gameWindow;
        private bool isHost;

        public GameManager(GameType gameType, Categories category, GameWindow gameWindow, bool isHost)
        {
            
            this.gameType = gameType;
            this.category = category;
            this.gameWindow = gameWindow;
            this.isHost = isHost;
            numbers = new List<int>();
            doc = XDocument.Load(path);
            if (category != Categories.Empty)
            {
                counter = doc
                    .Descendants(category.ToString())
                    .Descendants("Object")
                    .Count();
            }
            secondTimer = new DispatcherTimer();
            secondTimer.Interval = TimeSpan.FromSeconds(1);
            secondTimer.Tick += SecondTimer_Tick;
        }

        public void StartTimer()
        {
            currentTime = 10;
            secondTimer.Start();
            //gameWindow.SetProperties(gameType, category, 3);
        }

        public void zegar(bool stan)
        {
            if (stan)
            {
                Debug.WriteLine("start");
                secondTimer.Start();
            }
            else
            {
                Debug.WriteLine("koniec");
                secondTimer.Stop();
            }
        }

        private void SecondTimer_Tick(object sender, EventArgs e)
        {
            if (isHost && gameType == GameType.Numbers)
            {
                if (currentTime == 0)
                {
                    currentTime = 10;
                    int generatedInt = RandomValue(gameType, counter);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (gameType == 0) gameWindow.txbGeneratedNumber.Text = generatedInt.ToString();
                        else
                        {
                            doc = XDocument.Load(path);
                            XElement? temp = doc
                                    .Descendants(category.ToString())
                                    .Descendants("Object")
                                    .FirstOrDefault(d => (int)d.Element("Id") == generatedInt);
                            if (temp != null)
                            {
                                gameWindow.txbGeneratedNumber.Text = temp.Element("Name").Value;
                            }
                        }

                        BingoNumberButton.currentGeneratedValue = gameWindow.txbGeneratedNumber.Text;
                    });
                    
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    gameWindow.txbTimer.Text = currentTime.ToString();
                });
                currentTime--;
            }
            if(!isHost && gameType == GameType.Numbers)
            {
                BingoNumberButton.currentGeneratedValue = gameWindow.txbGeneratedNumber.Text;
            }
        }



        private List<int> numbers;

        public int RandomValue(GameType gameType, int counter)
        {
            Random random = new Random();
            int max;
            if (gameType == GameType.Numbers)
                max = 75;
            else
                max = counter;
            if (numbers.Count == 74)
                numbers.RemoveAll(x => x <= max);

            int generated;
            do
            {
                generated = random.Next(1, max);
            } while (numbers.Contains(generated));
            numbers.Add(generated);
            return generated;

        }


    }
}