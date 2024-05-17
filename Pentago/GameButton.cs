using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago
{
    public class GameButton
    {

        Button button;
        
        public GameButton() 
        {
            Button button = new Button();
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.RenderTransformOrigin = new Point(0.5, 0.5);
            button.Height = 80;
            button.Width = 80;

            // BorderBrush setzen
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

        }
    }
}
