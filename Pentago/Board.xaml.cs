using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Pentago
{

    public partial class Board: UserControl
    {
        
        public Board()
        {
            InitializeComponent();
            
            for (int row = 0;row< GridTopL.RowDefinitions.Count; row++)
            { for (int col = 0; col < GridTopL.ColumnDefinitions.Count; col++)
                {
                    Button button = new Button();
                    button.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                    button.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                    button.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
                    button.Tag = $"{row},{col}";
                    button.Height = 80;
                    button.Width = 80;

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

                    // Fügen Sie den Button zum Layout hinzu

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);

                    GridTopL.Children.Add(button);

                    // To DO  Funktion die mir zum Start alle Buttons erstellt. 
                    // gegebenfalls auch die Grids.

                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
