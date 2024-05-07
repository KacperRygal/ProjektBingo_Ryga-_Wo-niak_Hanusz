using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Bingo.Classes
{
    public class BingoButton : Button
    {
        public static string currentGeneratedValue = "";
        public int id;
        public bool currentState = false;
        public event EventHandler ButtonClicked;

        public BingoButton() 
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Margin = new Thickness(5);
            Width = 100;
            Height = 100;
            Click += BingoButton_Click;
            Background = Brushes.LightGray;
        }

        private void BingoButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(currentGeneratedValue);
            Debug.WriteLine(Content);
            if (currentGeneratedValue.Equals(Content.ToString()))
            {
                if (!currentState)
                {
                    Background = new SolidColorBrush(Colors.Red);
                    currentState = true;
                }
                else
                {
                    Background = new SolidColorBrush(Colors.LightGray);
                    currentState = false;
                }
                OnButtonClicked(EventArgs.Empty);
            }
        }
       
        protected virtual void OnButtonClicked(EventArgs e)
        {
            ButtonClicked?.Invoke(this, e);
        }
        

    }
}
