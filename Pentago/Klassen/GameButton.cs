using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago.Klassen
{    public class GameButton
    {
        public Button Button { get; set; }
        public int row { get; set; }
        public int col { get; set; }
        Logik logik;

        public GameButton(int row, int col)
        {
            Button = GetNewGameButton();
            logik = new Logik();
            this.row = row;
            this.col = col;
        }

        private Button GetNewGameButton()
        {
                    Button button = new Button();
                    button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                    button.Height = 50;
                    button.Width = 50;
                    

                    LinearGradientBrush borderBrush = new LinearGradientBrush();
                    borderBrush.StartPoint = new Point(0.5, 0);
                    borderBrush.EndPoint = new Point(0.5, 1);
                    borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
                    borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                    button.BorderBrush = borderBrush;

                    // Background setzen
                    LinearGradientBrush backgroundBrush = new LinearGradientBrush();
                    backgroundBrush.StartPoint = new Point(0.5, 0);
                    backgroundBrush.EndPoint = new Point(0.5, 1);
                    backgroundBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                    backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x67, 0x67, 0x67), 0));
                    button.Background = backgroundBrush;

                    button.Click += Button_Click;



            return button;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            button.Background = new SolidColorBrush(Colors.Blue);
            button.IsEnabled = false;
            logik.SetPressedStone(button.Parent as Grid, row, col);
        }
    }
}
