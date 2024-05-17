using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Remoting.Channels;
using System.Windows;
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
            Setbutton();
            

        }

        private void Setbutton()
        {
            for (int i = 0;i< grid.)
        }

        private void Button_Click_Rotation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            string[] buttonpos = button.Uid.Split(',');
            int row = int.Parse(buttonpos[0]);
            int col = int.Parse(buttonpos[1]);

            game.GameGrid[row, col] = game.currentPlayer;

            if (game.currentPlayer == Player.Blue)
            {
                button.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }


        }



    }
}
