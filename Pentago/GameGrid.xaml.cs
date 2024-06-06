
using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class GameGrid : UserControl
    {
        public event EventHandler<GridButtonClickEventArgs> GridButtonClick;
        public double CurrentRotation;
        
        public List<Button> Buttons {get; set; }  
        public GameGrid(int startrow , int startcol)
        {

            InitializeComponent();
            
             
            Buttons = new List<Button>();
            CurrentRotation = 0;
            Process(startrow,startcol);

        }

        public void SetNewPositions(Direction direction)
        {    foreach (var button in Buttons)
            {
                switch (direction)
                {
                    case Direction.Left:
                        RotateLeft(button);
                        break;
                    case Direction.Right:
                        RotateRight(button);
                        break;
                }
            }       
        }

        private void RotateRight(Button button)
        {
            var currentpos = (Positions)button.Tag;

            int translateRow = currentpos.Row % 3;
            int translateCol = currentpos.Column % 3;

            int newrotateRow = translateCol;
            int newrotateCol = 2 - translateRow;

            int newRow = (currentpos.Row / 3) * 3 + newrotateRow;
            int newCol = (currentpos.Column / 3) * 3 + newrotateCol;

            button.Tag = new Positions(newRow,newCol);

        }

        private void RotateLeft(Button button)
        {

            var currentpos = (Positions)button.Tag;

            int translateRow = currentpos.Row % 3;
            int translateCol = currentpos.Column % 3;

            int newrotateRow = 2 - translateCol;
            int newrotateCol = translateRow;

            int newRow = (currentpos.Row / 3) * 3 + newrotateRow;
            int newCol = (currentpos.Column / 3) * 3 + newrotateCol;

            button.Tag = new Positions(newRow, newCol);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            var pos = (Positions)button.Tag;
            var row = pos.Row;
            var column = pos.Column;
            GridButtonClick?.Invoke(this, new GridButtonClickEventArgs(row, column,Buttons));

        }
        public Button GetButtonbyTag(int row, int col)
        {
            foreach (var button in Buttons)
            {
                var pos = (Positions)button.Tag;

                if (pos.Row == row && pos.Column == col)
                    return button;


            }
            return null;
        }
        private void Process(int r,int c)
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
                    button.Name = $"B{row + r}{col +c}";
                    button.Tag = new Positions(row + r,col + c);
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

                    Grid.SetRow(button, row);
                    Grid.SetColumn(button, col);
                    

                    grid.Children.Add(button);

                    button.Click += Button_Click;
                    Buttons.Add(button);
                    
                }
                
            }
        }
        public void Reset()
        {
            foreach(Button button in Buttons)
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

