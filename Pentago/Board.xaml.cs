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
         GameButton gameButton;
         List<Button> Buttons;
        public Board()
        {
            InitializeComponent();
            Buttons = new List<Button>();
             game = new Game();
            Setbutton(GridTL,0,0);
            Setbutton(GridTR, 0, 3);
            Setbutton(GridBL, 3, 0);
            Setbutton(GridBR, 3, 3);
        }
        private void Setbutton(Grid grid,int i,int j)
        {
            
            for (int r = 0;r < grid.RowDefinitions.Count ; r++)
            {
                for (int c = 0; c < grid.ColumnDefinitions.Count ; c++)
                {
                    
                    gameButton = new GameButton();
                    gameButton.Button.Click += Button_Click;
                    gameButton.Button.Uid = $"{i},{j}";
                    Buttons.Add(gameButton.Button);
                    Grid.SetColumn(gameButton.Button, c);
                    Grid.SetRow(gameButton.Button, r);
                    grid.Children.Add(gameButton.Button);
                     j++;  
                }
                i++;
                j = 0;
            }
        }

       

        private void Button_Click_Rotation(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            (int row ,int col) = gameButton.IntParse(button.Uid.Split(','));

            ChangeButtonColor(button);
            game.GameGrid[row, col] = game.currentPlayer;
            DisableButton(button.Uid);
            
        }
        private void DisableButton(string uid)
        {
            foreach (var button in Buttons)
            {
                if (button.Uid != uid && button.IsEnabled != false)
                {
                    button.IsEnabled = false;
                    
                }
            }

        }
        private void EnableButton(string uid)
        {
            foreach (var button in Buttons)
            {
                if (!button.IsEnabled)
                {
                    button.IsEnabled = true;
                }
            }
        }
        private void ChangeButtonColor(Button button)
        {

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
