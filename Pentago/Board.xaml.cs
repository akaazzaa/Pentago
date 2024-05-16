using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        Game game;
        
        public Board()
        {
            InitializeComponent();
            game = new Game();
            Run();
           
        }

        private void Run()
        {
            
             // Stein wählen 
             //win prüfen / Ende Oder Weiter
             // array drehen animation
             // win prüfen / Ende oder Weiter
             // Spieler wechseln
             //wiederholen

        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var gameButton = sender as Button;
            if (game.currentplayer == Player.Blue)
            {
                gameButton.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                gameButton.Background = new SolidColorBrush(Colors.Red);
            }
            string[] pos = gameButton.Uid.Split(',');
            int row = int.Parse(pos[0]);
            int col = int.Parse(pos[1]);
            
            var grid = gameButton.Parent as Grid;
            
            game.SetPressedStone(grid, row, col);
            GetButton(Maingrid, grid.Uid[0]);
        }

        private void GetButton(Grid main,char gridpos)
        {
            
            foreach (var item in main.Children)
            {
                if (item is Button )
                {
                    Button button = (Button)item;
                    if (button.Uid[0] == gridpos)
                    {
                        button.Visibility = System.Windows.Visibility.Visible;
                    }
                }
                
            }
        }

        private void Button_Click_Rotation(object sender, System.Windows.RoutedEventArgs e)
        {
            var gameButton = sender as Button;
            
        }
    }

   
}
