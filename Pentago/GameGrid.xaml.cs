using Pentago.Klassen;
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

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class GameGrid : UserControl
    {
        public event EventHandler<GridButtonClickEventArgs> GridButtonClick;
        public List<Button> buttons {  get; set; }  
        public GameGrid()
        {
            InitializeComponent();
            buttons = new List<Button>();
            Process();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            
            var row = Grid.GetRow(button);
            var column = Grid.GetColumn(button);
            GridButtonClick?.Invoke(this, new GridButtonClickEventArgs(row, column,button));

        }
        private void Process()
        {
            for (int row = 0; row < grid.RowDefinitions.Count; row++)
            {
                for (int col = 0; col < grid.ColumnDefinitions.Count; col++)
                {
                   Button button = new Button();
                    button.HorizontalAlignment = HorizontalAlignment.Center;
                    button.VerticalAlignment = VerticalAlignment.Center;
                    button.RenderTransformOrigin = new Point(0.5, 0.5);
                    button.Height = 80;
                    button.Width = 80;
                    button.Name = $"B{row}{col}";
                    button.Content = button.Name;

                    
                    LinearGradientBrush borderBrush = new LinearGradientBrush();
                    borderBrush.StartPoint = new Point(0.5, 0);
                    borderBrush.EndPoint = new Point(0.5, 1);
                    borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 0));
                    borderBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                    button.BorderBrush = borderBrush;

                    
                    LinearGradientBrush backgroundBrush = new LinearGradientBrush();
                    backgroundBrush.StartPoint = new Point(0.5, 0);
                    backgroundBrush.EndPoint = new Point(0.5, 1);
                    backgroundBrush.GradientStops.Add(new GradientStop(Colors.Black, 1));
                    backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x67, 0x67, 0x67), 0));
                    button.Background = backgroundBrush;

                    Grid.SetColumn(button, col);
                    Grid.SetRow(button, row);

                    grid.Children.Add(button);

                    button.Click += Button_Click;
                    buttons.Add(button);
                }

            }
        }
        public void Reset()
        {
            foreach(Button button in buttons)
            {
                button.HorizontalAlignment = HorizontalAlignment.Center;
                button.VerticalAlignment = VerticalAlignment.Center;
                button.RenderTransformOrigin = new Point(0.5, 0.5);
                button.Content = button.Name;

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

        
}

