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
    /// Interaktionslogik für Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        List<GameGrid> grids;
        public Board()
        {
            InitializeComponent();

            for (int row = 0; row < MainGrid.RowDefinitions.Count; row++) 
            {
                for(int col = 0;col < MainGrid.ColumnDefinitions.Count; col++)
                {
                    GameGrid gameGrid = new GameGrid();
                    gameGrid.Name = "grid" + row + "_" + col;
                    Grid.SetRow(gameGrid, row);
                    Grid.SetColumn(gameGrid, col);
                    MainGrid.Children.Add(gameGrid);
                }
            }

            
            
        }
    }
}
