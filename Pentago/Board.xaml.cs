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
        Game game = new Game(); 
        GameGrid TopLeft;
        GameGrid TopRight;
        GameGrid BotLeft;
        GameGrid BotRight;
        bool hasTurned = false;
        public Board()
        {
            InitializeComponent();

            SetGrids();

        }

        private void SetGrids()
        {
            TopLeft = new GameGrid();
            TopLeft.Name = "GridTopLeft";
            TopLeft.HorizontalAlignment = HorizontalAlignment.Right;
            TopLeft.VerticalAlignment = VerticalAlignment.Bottom;
            TopLeft.GridButtonClick += GridButton_Click;
            Grid.SetRow(TopLeft, 0);
            Grid.SetColumn(TopLeft, 0);
            MainGrid.Children.Add(TopLeft);

            TopRight = new GameGrid();
            TopRight.Name = "GridTopRight";
            TopRight.HorizontalAlignment = HorizontalAlignment.Left;
            TopRight.VerticalAlignment = VerticalAlignment.Bottom;
            TopRight.GridButtonClick += GridButton_Click;
            Grid.SetRow(TopRight, 0);
            Grid.SetColumn(TopRight, 1);
            MainGrid.Children.Add(TopRight);

            BotLeft = new GameGrid();
            BotLeft.Name = "GridBotLeft";
            BotLeft.HorizontalAlignment = HorizontalAlignment.Right;
            BotLeft.VerticalAlignment = VerticalAlignment.Top;
            BotLeft.GridButtonClick += GridButton_Click;
            Grid.SetRow(BotLeft, 1);
            Grid.SetColumn(BotLeft, 0);
            MainGrid.Children.Add(BotLeft);

            BotRight = new GameGrid();
            BotRight.Name = "GridBotRight";
            BotRight.HorizontalAlignment = HorizontalAlignment.Left;
            BotRight.VerticalAlignment = VerticalAlignment.Top;
            BotRight.GridButtonClick += GridButton_Click;
            Grid.SetRow(BotRight, 1);
            Grid.SetColumn(BotRight, 1);
            MainGrid.Children.Add(BotRight);
        }

        private void GridButton_Click(object sender, GridButtonClickEventArgs e)
        {
            var grid = (GameGrid)sender;
            game.Changbuttoncolor(e.Button);

            if (hasTurned)
            {
                return;
            }
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    game.TopLeft[e.Row, e.Column] = game.currentPlayer;
                    break;
                case ("GridTopRight"):
                    game.TopRight[e.Row, e.Column] = game.currentPlayer;
                    break;
                case ("GridBotLeft"):
                    game.BotLeft[e.Row, e.Column] = game.currentPlayer;
                    break;
                case ("GridBotRight"):
                    game.BotRight[e.Row, e.Column] = game.currentPlayer;
                    break;
            }
            PlayerTurned();

           if (game.CheckWin())
            {
                MessageBox.Show("Win");
                game.Ausgabe();
            }
        }

        

        private void PlayerTurned()
        {
                hasTurned = true;
                //game.SwitchPlayer();
                hasTurned = false;
            
        }

        private void Button_Click_Rotation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    game.TopLeft[i,j] = Player.None;
                    
                    game.TopRight[i,j] = Player.None;
                    
                    game.BotLeft[i,j] = Player.None;
                    
                    game.BotRight[i,j] = Player.None;
                    

                }
            }
            TopLeft.Reset();
            TopRight.Reset();
            BotLeft.Reset();
            BotRight.Reset();
            

           
        }
    }
}
