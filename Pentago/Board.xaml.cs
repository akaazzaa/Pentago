using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Pentago
{
    /// <summary>
    /// Interaktionslogik für Board.xaml
    /// </summary>
    public partial class Board : UserControl
    {
        Game Game;
        GameGrid TopLeft;
        GameGrid TopRight;
        GameGrid BotLeft;
        GameGrid BotRight;
        List<Button> RotationButtons;

       
        
        public Board(Game game)
        {
            InitializeComponent();
            Game = game;
            SetGrids();
            RotationButtons = new List<Button>();
            game.GameEnded += OnGameEnded;
            game.GameRestarted += OnGameRestarted;
            game.MoveMade += OnMoveMade;
            AddButtons();
            ChangePlayerIcon();
            ChangeVisibilityRotationButton();
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
            GameGrid.Children.Add(TopLeft);

            TopRight = new GameGrid();
            TopRight.Name = "GridTopRight";
            TopRight.HorizontalAlignment = HorizontalAlignment.Left;
            TopRight.VerticalAlignment = VerticalAlignment.Bottom;
            TopRight.GridButtonClick += GridButton_Click;
            Grid.SetRow(TopRight, 0);
            Grid.SetColumn(TopRight, 1);
            GameGrid.Children.Add(TopRight);

            BotLeft = new GameGrid();
            BotLeft.Name = "GridBotLeft";
            BotLeft.HorizontalAlignment = HorizontalAlignment.Right;
            BotLeft.VerticalAlignment = VerticalAlignment.Top;
            BotLeft.GridButtonClick += GridButton_Click;
            Grid.SetRow(BotLeft, 1);
            Grid.SetColumn(BotLeft, 0);
            GameGrid.Children.Add(BotLeft);

            BotRight = new GameGrid();
            BotRight.Name = "GridBotRight";
            BotRight.HorizontalAlignment = HorizontalAlignment.Left;
            BotRight.VerticalAlignment = VerticalAlignment.Top;
            BotRight.GridButtonClick += GridButton_Click;
            Grid.SetRow(BotRight, 1);
            Grid.SetColumn(BotRight, 1);
            GameGrid.Children.Add(BotRight);
        }
        private void AddButtons()
        {
            RotationButtons.Add(BTLL);
            RotationButtons.Add(BTLR);

            RotationButtons.Add(BTRL);
            RotationButtons.Add(BTRR);

            RotationButtons.Add(BBLL);
            RotationButtons.Add(BBLR);

            RotationButtons.Add(BBRL);
            RotationButtons.Add(BBRR);


        }
        private void ChangeVisibilityRotationButton()
        {
            foreach (Button b in RotationButtons)
            {
              
                if (b.Visibility == Visibility.Visible)
                {
                    b.Visibility = Visibility.Hidden;
                }
                else
                {
                    b.Visibility = Visibility.Visible;
                }
            }
        }
        private void EndScreening(string text,SolidColorBrush colorBrush)
        {
            GameGrid.Visibility = Visibility.Hidden;
            ResultText.Text = text;
            WinnerIcon.Fill = colorBrush;
            EndScreen.Visibility = Visibility.Visible;
        }
        private void OnMoveMade()
        {
            Game.Turned = true;
            ChangeVisibilityRotationButton();
        }
        private void ChangePlayerIcon()
        {
            if (Game.CurrentPlayer == Player.Blue)
            {
                PlayerIcon.Fill = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                PlayerIcon.Fill = new SolidColorBrush(Colors.Red);
            }
        }
        private async void OnGameEnded(GameResult gameResult)
        {
            await Task.Delay(1000);
           if (gameResult.Winner == Player.None)
            {
                EndScreening("Unentschieden", null);
            }
            else if (gameResult.Winner == Player.Blue)
            {
                EndScreening("Winner:", new SolidColorBrush(Colors.Blue));
            }
            else
            {
                EndScreening("Winner:", new SolidColorBrush(Colors.Red));
            }
            
        }

        private void OnGameRestarted()
        {

        }

        private void GridButton_Click(object sender, GridButtonClickEventArgs e)
        {
            var grid = (GameGrid)sender;
            Game.MakeMove(e.Row,e.Column,grid,e.Button);
        }
        private void Button_Click_Rotation(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Game.RotateArray(button.Name,TopLeft,TopRight,BotLeft,BotRight);
            ChangePlayerIcon();
            ChangeVisibilityRotationButton();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetGrids();
            Game.Reset();
    
        }


    }
}
