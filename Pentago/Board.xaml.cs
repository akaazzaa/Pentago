using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Pentago
{

    public partial class Board : UserControl
    {
        GameGrid TopLeft;

        GameGrid TopRight;

        GameGrid BottomLeft;

        GameGrid BottomRight; 
        public Board()
        {
            InitializeComponent();
            
            TopLeft = new GameGrid(GridTopL);
            TopRight = new GameGrid(GridTopR);
            BottomLeft = new GameGrid(GridBotL);
            BottomRight = new GameGrid(GridBotR);
           

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TopLeft.gridrotate();
        }
    }
}
