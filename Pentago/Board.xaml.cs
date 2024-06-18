using Pentago.Klassen;
using System;
using System.Collections.Generic;
using System.Net;
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
        List<GameGrid> Grids;

      
     
        public Board(Game game)
        {
            InitializeComponent();
            Game = game;
            RotationButtons = new List<Image>();
            Grids = new List<GameGrid>();
            game.ComputerMove += OnComputerMove;
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
            TopLeft.Tag = Quadrant.Topleft;
            Grid.SetRow(TopLeft, 0);
            Grid.SetColumn(TopLeft, 0);

            GameGrid.Children.Add(TopLeft);
            Grids.Add(TopLeft);

            TopRight = new GameGrid(0, 3);
            TopRight.Name = "GridTopRight";
            TopRight.HorizontalAlignment = HorizontalAlignment.Left;
            TopRight.VerticalAlignment = VerticalAlignment.Bottom;
            TopRight.GridButtonClick += GridButton_Click;
            TopRight.Tag = Quadrant.Topright;
            Grid.SetRow(TopRight, 0);
            Grid.SetColumn(TopRight, 1);
            GameGrid.Children.Add(TopRight);
            Grids.Add(TopRight);

            BotLeft = new GameGrid(3, 0);
            BotLeft.Name = "GridBotLeft";
            BotLeft.HorizontalAlignment = HorizontalAlignment.Right;
            BotLeft.VerticalAlignment = VerticalAlignment.Top;
            BotLeft.GridButtonClick += GridButton_Click;
            BotLeft.Tag = Quadrant.Botleft;
            Grid.SetRow(BotLeft, 1);
            Grid.SetColumn(BotLeft, 0);
            GameGrid.Children.Add(BotLeft);
            Grids.Add(BotLeft);

            BotRight = new GameGrid(3, 3);
            BotRight.Name = "GridBotRight";
            BotRight.HorizontalAlignment = HorizontalAlignment.Left;
            BotRight.VerticalAlignment = VerticalAlignment.Top;
            BotRight.GridButtonClick += GridButton_Click;
            BotRight.Tag = Quadrant.Botright;
            Grid.SetRow(BotRight, 1);
            Grid.SetColumn(BotRight, 1);
            GameGrid.Children.Add(BotRight);
            Grids.Add(BotRight);
        }
        
        private void ChangePlayerIcon()
        {
            LinearGradientBrush backgroundBrush = new LinearGradientBrush();
            backgroundBrush.StartPoint = new Point(0.5, 0);
            backgroundBrush.EndPoint = new Point(0.5, 1);

            if (Game.CurrentPlayer == Player.Blue)
            {
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x0D, 0x32, 0xC5), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8F, 0x96, 0xD6), 0));
                PlayerIcon.Fill = backgroundBrush;

            }
            else
            {
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDA, 0xBD, 0xC9), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF1, 0x02, 0x02), 0));
                PlayerIcon.Fill = backgroundBrush;
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
                    Game.RotateField(Quadrant.Topleft, Direction.Left);

                    break;
                case "BTLR":
                    Move(TopLeft, Direction.Right);
                    Game.RotateField(Quadrant.Topleft, Direction.Right);
                    break;

                case "BTRL":
                    Move(TopRight, Direction.Left);
                    Game.RotateField(Quadrant.Topright, Direction.Left);
                    break;
                case "BTRR":
                    Move(TopRight, Direction.Right);
                    Game.RotateField(Quadrant.Topright, Direction.Right);
                    break;

                case "BBLL":
                    Move(BotLeft, Direction.Right);
                    Game.RotateField(Quadrant.Botleft, Direction.Right);
                    break;
                case "BBLR":
                    Move(BotLeft, Direction.Left);
                    Game.RotateField(Quadrant.Botleft, Direction.Left);
                    break;

                case "BBRL":
                    Move(BotRight, Direction.Right);
                    Game.RotateField(Quadrant.Botright, Direction.Right);
                    break;
                case "BBRR":
                    Move(BotRight, Direction.Left);
                    Game.RotateField(Quadrant.Botright, Direction.Left);
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
        private void Move(Quadrant quadrant, Direction direction)
        {
            GameGrid gameGrid = GetGridByQuadrant(quadrant); 
            if (gameGrid == null) { return; }
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
        private GameGrid GetGridByQuadrant(Quadrant quadrant)
        {
            for (int i = 0;i< Grids.Count;i++)
            {
                if ((Quadrant)Grids[i].Tag == quadrant)
                {
                    return Grids[i];
                }
            }
            return null;
        }

        public void Changbuttoncolor(int row, int col)
        {

            LinearGradientBrush backgroundBrush = new LinearGradientBrush();
            backgroundBrush.StartPoint = new Point(0.5, 0);
            backgroundBrush.EndPoint = new Point(0.5, 1);

            Button button = GetGridButton(row, col);
            if (button == null)
            {
                return;
            }

            if (Game.CurrentPlayer == Player.Blue)
            {

                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x0D, 0x32, 0xC5), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8F, 0x96, 0xD6), 0));
                button.Background = backgroundBrush;
            }
            else
            {
             
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF1, 0x02, 0x02), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDA, 0xBD, 0xC9), 0));
                button.Background = backgroundBrush;
            }
        }

        private Button GetGridButton(int row, int col)
        {

            if (row < 3 && col < 3) return TopLeft.GetButtonbyTag(row, col);

            if (row < 3 && col > 2) return TopRight.GetButtonbyTag(row, col);

            if (row > 2 && col < 3) return BotLeft.GetButtonbyTag(row, col);

            if (row > 2 && col > 2) return BotRight.GetButtonbyTag(row, col);
                
            
            return null;
        }

        private void EndScreening(string text, LinearGradientBrush colorBrush)
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
            Quadrant quandrant = (Quadrant)grid.Tag;
            Game.MakeMove(e.Row, e.Column,quandrant, Direction.none);
        }
        private void Click_MouseDown(object sender, MouseButtonEventArgs e )
        {
            Image image = (Image)sender;

            ChangeVisibilityRotationButton();
            RotateAnimation(image);

            PrintArray();
            if (Game.IsWin())
            {
                Game.GameOver = true;
                GameEnded();
            }

            Game.SwitchPlayer();
            ChangePlayerIcon();
            
            if (!Game.isSinglePlayer)
                Game.Turned = false;

            if (Game.isSinglePlayer && Game.CurrentPlayer == Player.Red && Game.GameOver == false)
            {
               var move = Game.GetBestMove(2);
               Game.MakeMoveComputer(move.Item1, move.Item2, move.Item3, move.Item4);
                
            }
           
        }
        private void OnMoveMade(int row, int col, Quadrant corner)
        {
            Changbuttoncolor(row,col);
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
            LinearGradientBrush backgroundBrush = new LinearGradientBrush();
            backgroundBrush.StartPoint = new Point(0.5, 0);
            backgroundBrush.EndPoint = new Point(0.5, 1);

            await Task.Delay(1000);

            if (Game.GameResult.Winner == Player.None)
            {
                EndScreening("Unentschieden", null);
            }
            else if (Game.GameResult.Winner == Player.Blue)
            {

                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x0D, 0x32, 0xC5), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8F, 0x96, 0xD6), 0));

                EndScreening("Winner:", backgroundBrush);
            }
            else
            {

                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF1, 0x02, 0x02), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDA, 0xBD, 0xC9), 0));

                EndScreening("Winner:", backgroundBrush);
            }

        }
        private async void OnComputerMove(int r,int c,Quadrant quadrant,Direction direction)
        {
            
            await Task.Delay(1000);
            Changbuttoncolor(r, c);
            
            
            Move(quadrant, direction);
            PrintArray();

            Game.SwitchPlayer();
            ChangePlayerIcon();
            Game.Turned = false;
            if (Game.IsWin())
            {
                Game.GameOver = true;
                GameEnded();
            }
        }
    
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
            ChangePlayerIcon();
            EndScreen.Visibility = Visibility.Hidden;
            GameGrid.Visibility = Visibility.Visible;
            
            GameGrid.Children.Remove(TopLeft);
            GameGrid.Children.Remove(TopRight);
            GameGrid.Children.Remove(BotLeft);
            GameGrid.Children.Remove(BotRight);
            Grids.Clear();
            SetGrids();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainModel.SetNewContent(new Menu());
        }
    }
}
