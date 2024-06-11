﻿using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
        List<Image> RotationButtons;
     
        public Board(Game game)
        {
            InitializeComponent();
            Game = game;
            RotationButtons = new List<Image>();
         
            game.GameRestarted += OnGameRestarted;
            game.MoveMade += OnMoveMade;
            SetGrids();
            AddImmageButtonToList();
            ChangePlayerIcon();
            
            
        }
        private void AddImmageButtonToList()
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

        #region Start
        private void SetGrids()
        {

            TopLeft = new GameGrid(0, 0);
            TopLeft.Name = "GridTopLeft";
            TopLeft.HorizontalAlignment = HorizontalAlignment.Right;
            TopLeft.VerticalAlignment = VerticalAlignment.Bottom;
            TopLeft.GridButtonClick += GridButton_Click;
            TopLeft.Tag = Corner.Topleft;
            Grid.SetRow(TopLeft, 0);
            Grid.SetColumn(TopLeft, 0);

            GameGrid.Children.Add(TopLeft);

            TopRight = new GameGrid(0, 3);
            TopRight.Name = "GridTopRight";
            TopRight.HorizontalAlignment = HorizontalAlignment.Left;
            TopRight.VerticalAlignment = VerticalAlignment.Bottom;
            TopRight.GridButtonClick += GridButton_Click;
            TopRight.Tag = Corner.Topright;
            Grid.SetRow(TopRight, 0);
            Grid.SetColumn(TopRight, 1);
            GameGrid.Children.Add(TopRight);

            BotLeft = new GameGrid(3, 0);
            BotLeft.Name = "GridBotLeft";
            BotLeft.HorizontalAlignment = HorizontalAlignment.Right;
            BotLeft.VerticalAlignment = VerticalAlignment.Top;
            BotLeft.GridButtonClick += GridButton_Click;
            BotLeft.Tag = Corner.Botleft;
            Grid.SetRow(BotLeft, 1);
            Grid.SetColumn(BotLeft, 0);
            GameGrid.Children.Add(BotLeft);

            BotRight = new GameGrid(3, 3);
            BotRight.Name = "GridBotRight";
            BotRight.HorizontalAlignment = HorizontalAlignment.Left;
            BotRight.VerticalAlignment = VerticalAlignment.Top;
            BotRight.GridButtonClick += GridButton_Click;
            BotRight.Tag = Corner.Botright;
            Grid.SetRow(BotRight, 1);
            Grid.SetColumn(BotRight, 1);
            GameGrid.Children.Add(BotRight);
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
        private void ChangeVisibilityRotationButton()
        {
            foreach (Image image in RotationButtons)
            {

                if (image.Visibility == Visibility.Visible)
                {
                    image.Visibility = Visibility.Hidden;
                }
                else
                {
                    image.Visibility = Visibility.Visible;
                }
            }
        }
        #endregion

        #region Animation 
        private void RotateAnimation(Image imagebutton)
        {
            switch (imagebutton.Name)
            {
                case "BTLL":
                    Move(TopLeft, Direction.Left);
                    Game.RotateField(Corner.Topleft, Direction.Left);

                    break;
                case "BTLR":
                    Move(TopLeft, Direction.Right);
                    Game.RotateField(Corner.Topleft, Direction.Right);
                    break;

                case "BTRL":
                    Move(TopRight, Direction.Left);
                    Game.RotateField(Corner.Topright, Direction.Left);
                    break;
                case "BTRR":
                    Move(TopRight, Direction.Right);
                    Game.RotateField(Corner.Topright, Direction.Right);
                    break;

                case "BBLL":
                    Move(BotLeft, Direction.Right);
                    Game.RotateField(Corner.Botleft, Direction.Right);
                    break;
                case "BBLR":
                    Move(BotLeft, Direction.Left);
                    Game.RotateField(Corner.Botleft, Direction.Left);
                    break;

                case "BBRL":
                    Move(BotRight, Direction.Right);
                    Game.RotateField(Corner.Botright, Direction.Right);
                    break;
                case "BBRR":
                    Move(BotRight, Direction.Left);
                    Game.RotateField(Corner.Botright, Direction.Left);
                    break;
            }
        }
        private void Move(GameGrid gameGrid, Direction direction)
        {
            int angle = 0;
            if (direction == Direction.Left)
            {
                angle = -90;
            }
            else
            {
                angle = 90;
            }

            RotateTransform rotateTransformTopLeft = new RotateTransform();
            rotateTransformTopLeft.Angle = gameGrid.CurrentRotation;
            gameGrid.grid.RenderTransform = rotateTransformTopLeft;
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = gameGrid.CurrentRotation;
            rotationAnimation.To = gameGrid.CurrentRotation + angle;
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            gameGrid.grid.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            gameGrid.CurrentRotation += angle;

            gameGrid.SetNewPositions(direction);

        }
        public void Changbuttoncolor(int row, int col, Corner quandrant)
        {
            
            Button button = GetGridButton(row, col, quandrant);
            if (button == null)
            {
                return;
            }

            if (Game.CurrentPlayer == Player.Blue)
            {
                button.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private Button GetGridButton(int row, int col, Corner quandrant)
        {
            switch (quandrant)
            {
                case Corner.Topleft:
                    return TopLeft.GetButtonbyTag(row, col);
                case Corner.Topright:
                    return TopRight.GetButtonbyTag(row,col);
                case Corner.Botleft:
                    return BotLeft.GetButtonbyTag(row, col);
                case Corner.Botright:
                    return BotRight.GetButtonbyTag(row, col);      
            }
            return null;
        }

        private void EndScreening(string text, SolidColorBrush colorBrush)
        {
            GameGrid.Visibility = Visibility.Hidden;
            ResultText.Text = text;
            WinnerIcon.Fill = colorBrush;
            EndScreen.Visibility = Visibility.Visible;
        }

        #endregion

        #region Events
        private void GridButton_Click(object sender, GridButtonClickEventArgs e)
        {
            var grid = (GameGrid)sender;
            Corner quandrant = (Corner)grid.Tag;
            Game.MakeMovePlayer(e.Row, e.Column,quandrant);
        }
        private void Click_MouseDown(object sender, MouseButtonEventArgs e )
        {
            Image image = (Image)sender;


            ChangeVisibilityRotationButton();
            RotateAnimation(image);
            if (Game.IsWin())
            {
                Game.GameOver = true;
                GameEnded();
            }
            Game.SwitchPlayer();
            ChangePlayerIcon();
            Game.Turned = false;
        }
        private void OnMoveMade(int row, int col, Corner corner)
        {
            Changbuttoncolor(row, col, corner);
            Game.Turned = true;
            ChangeVisibilityRotationButton();
            if (Game.IsWin())
            {
                Game.GameOver = true;
                GameEnded();
            }
        }
        private async void GameEnded()
        {
            await Task.Delay(1000);
            if (Game.GameResult.Winner == Player.None)
            {
                EndScreening("Unentschieden", null);
            }
            else if (Game.GameResult.Winner == Player.Blue)
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetGrids();
            EndScreen.Visibility = Visibility.Hidden;
            Game.Reset();

        }
        #endregion


        public void PrintArray()
        {
            Debugtext.Text = " ";
            for (int i = 0; i < Game.GameBoard.GetLength(0); i++)
            {
                for (int j = 0; j < Game.GameBoard.GetLength(1); j++)
                {
                    Debugtext.Text += Game.GameBoard[i, j] + "\t";
                }
                Debugtext.Text += "\n";
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PrintArray();
        }

        
    }
}
