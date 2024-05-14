using Pentago.Klassen;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Forms;

namespace Pentago
{
    public class GameGrid
    {
        Grid _grid;

        DoubleAnimation rotateAnimation;
        Duration duration;

        public Grid Grid { get => _grid; set => _grid = value; }
       
        public GameGrid(Grid grid)
        {
            _grid = grid;
            SetButtons();
          
        
        }

        private void SetButtons()
        {
            for (int i = 0; i < _grid.RowDefinitions.Count; i++)
            {
                for (int j = 0; j < _grid.ColumnDefinitions.Count; j++)
                {
                    GameButton gameButton = new GameButton(i, j);
                    Grid.SetRow(gameButton.Button, i);
                    Grid.SetColumn(gameButton.Button, j);
                    _grid.Children.Add(gameButton.Button);
                }
            }
        }

        public void gridrotate()
        {
            _grid.RenderTransform 
            
            rotateAnimation = new DoubleAnimation
            {
                From = 0,
                To = 90,
                Duration = new Duration(TimeSpan.FromSeconds(10)),
                
            };

          
          

        }

    }
}
