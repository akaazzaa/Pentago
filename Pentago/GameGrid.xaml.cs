
using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Grid.xaml
    /// </summary>
    public partial class GameGrid : UserControl
    {
        public event EventHandler<GridButtonClickEventArgs> GridButtonClick;
        public double CurrentRotation;
        public List<Ellipse> EllipseList {  get; set; } 
        public List<Button> Buttons {get; set; }  
        private int startRow { get; set; }
        private int startCol { get; set; }


        public GameGrid(int startrow , int startcol)
        {

            InitializeComponent();
            startRow = startrow;
            startCol = startcol;
            EllipseList = new List<Ellipse>();
            Buttons = new List<Button>();
            CurrentRotation = 0;
            

            LoadVisuals();
           

        }
      

        public void SetNewPositions(Direction direction)
        {
            foreach (var ellipse in EllipseList)
            {
                switch (direction)
                {
                    case Direction.Left:
                        RotateLeft(ellipse);
                        break;
                    case Direction.Right:
                        RotateRight(ellipse);
                        break;
                }
            }

            foreach (var button in Buttons)
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
        private void RotateRight(Ellipse ellipse)
        {
            var currentpos = (Positions)ellipse.Tag;

            int translateRow = currentpos.Row % 3;
            int translateCol = currentpos.Column % 3;

            int newrotateRow = translateCol;
            int newrotateCol = 2 - translateRow;

            int newRow = (currentpos.Row / 3) * 3 + newrotateRow;
            int newCol = (currentpos.Column / 3) * 3 + newrotateCol;

            ellipse.Tag = new Positions(newRow, newCol);

        }
        private void RotateLeft(Ellipse ellipse)
        {

            var currentpos = (Positions)ellipse.Tag;

            int translateRow = currentpos.Row % 3;
            int translateCol = currentpos.Column % 3;

            int newrotateRow = 2 - translateCol;
            int newrotateCol = translateRow;

            int newRow = (currentpos.Row / 3) * 3 + newrotateRow;
            int newCol = (currentpos.Column / 3) * 3 + newrotateCol;

            ellipse.Tag = new Positions(newRow, newCol);
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
        public Ellipse GetEllipsebyTag(int row,int col)
        {
            foreach(var ellipse in EllipseList)
            {
                var pos = (Positions)ellipse.Tag;
                if (pos.Row == row && pos.Column == col)
                {
                    return ellipse;
                }
                
            }
            return null;
        }
        private void  LoadVisuals()
        {
            
            GenerateElipses();
            GenerateButtons();
            GenerateBorder();
        }

        private void GenerateBorder()
        {
            for (int row = 0; row < grid.RowDefinitions.Count; row++)
            {
                for (int col = 0; col < grid.ColumnDefinitions.Count; col++)
                {
                    Border border = new Border();
                    border.BorderThickness = new Thickness(1);
                    border.BorderBrush = new SolidColorBrush(Colors.Black);

                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);

                    grid.Children.Add(border);

                    
                }
            }

        }

        private void GenerateElipses()
        {
            for (int row = 0; row < grid.RowDefinitions.Count; row++)
            {
                for (int col = 0; col < grid.ColumnDefinitions.Count; col++)
                {
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 90;
                    ellipse.Height = 90;
                    ellipse.HorizontalAlignment = HorizontalAlignment.Center;
                    ellipse.VerticalAlignment = VerticalAlignment.Center;
                    ellipse.Fill = new SolidColorBrush(Colors.Aqua);
                    ellipse.Tag = new Positions(row + startRow, col + startCol);

                    Grid.SetRow(ellipse, row);
                    Grid.SetColumn(ellipse, col);
                    EllipseList.Add(ellipse);
                    grid.Children.Add(ellipse);
                }
            }
        }
        private void GenerateButtons()
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
                    button.Name = $"B{row + startRow}{col + startCol}";
                    button.Tag = new Positions(row + startRow,col + startCol);
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

