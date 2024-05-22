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

        public Button Button {  get; set; }
        public GameButton() 
        {
            
            Button = new Button();
            Button.HorizontalAlignment = HorizontalAlignment.Center;
            Button.VerticalAlignment = VerticalAlignment.Center;
            Button.RenderTransformOrigin = new Point(0.5, 0.5);
            Button.Height = 80;
            Button.Width = 80;

            // BorderBrush setzen
            LinearGradientBrush borderBrush = new LinearGradientBrush();
            borderBrush.StartPoint = new Point(0.5, 0);
            borderBrush.EndPoint = new Point(0.5, 1);
            borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
            borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            Button.BorderBrush = borderBrush;

            // Background setzen
            LinearGradientBrush backgroundBrush = new LinearGradientBrush();
            backgroundBrush.StartPoint = new Point(0.5, 0);
            backgroundBrush.EndPoint = new Point(0.5, 1);
            backgroundBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
            backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x67, 0x67, 0x67), 0));
            Button.Background = backgroundBrush;

           
        }

       

       
        
    }
}
