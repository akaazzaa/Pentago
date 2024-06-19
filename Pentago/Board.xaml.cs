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
            game.GameEnded += GameEnded;
            game.ComputerMove += OnComputerMove;
            game.MoveMade += OnMoveMade;
            SetGrids();
            AddImmageButtonToList();
            ChangePlayerIcon();
        }

        


        #region Start
        /// <summary>
        /// Fügt die Pfeilimages einer Liste hinzu
        /// </summary>
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
        /// <summary>
        /// Erzeugt die Grids und fügt sie dem MainGrid hinzu.
        /// </summary>
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
        /// <summary>
        /// ändert das Icon zu dem Spieler der am zug ist.
        /// </summary>
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
        /// <summary>
        /// Vesteckt oder zeigt die Images  zum drehen an.
        /// </summary>
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
        /// <summary>
        /// Dreht das richtig UserControl
        /// </summary>
        /// <param name="imagebutton"></param>
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
        /// <summary>
        /// Zeigt die Gewinnreihe an
        /// </summary>
        private void GetWinPosi()
        {
            foreach (var pos in Game.Winposi)
            {
                if (pos.Item1 < 3 && pos.Item2 < 3)
                {
                    TopLeft.GetEllipsebyTag(pos.Item1, pos.Item2).Fill = new SolidColorBrush(Colors.Yellow);
                }
                else if (pos.Item1 < 3 && pos.Item2 > 2)
                {
                    TopRight.GetEllipsebyTag(pos.Item1, pos.Item2).Fill = new SolidColorBrush(Colors.Yellow);
                }
                else if (pos.Item1 > 2 && pos.Item2 < 3)
                {
                    BotLeft.GetEllipsebyTag(pos.Item1, pos.Item2).Fill = new SolidColorBrush(Colors.Yellow);
                }
                else if (pos.Item1 > 2 && pos.Item2 > 2)
                {
                    BotRight.GetEllipsebyTag(pos.Item1, pos.Item2).Fill = new SolidColorBrush(Colors.Yellow);
                }
            }
        }
        /// <summary>
        /// Dreh Animation
        /// </summary>
        /// <param name="gameGrid"></param>
        /// <param name="direction"></param>
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
        /// <summary>
        /// Dreh animaton ohne Grid übergabe
        /// </summary>
        /// <param name="quadrant"></param>
        /// <param name="direction"></param>
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
        /// <summary>
        /// Sucht das richtige grid anhand des Enums
        /// </summary>
        /// <param name="quadrant"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Ändert die Buttonfarbe in Spielerfrabe
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
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
        /// <summary>
        /// Gibt mir den richtigen Button anhand der Row und Col
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private Button GetGridButton(int row, int col)
        {

            if (row < 3 && col < 3) return TopLeft.GetButtonbyTag(row, col);

            if (row < 3 && col > 2) return TopRight.GetButtonbyTag(row, col);

            if (row > 2 && col < 3) return BotLeft.GetButtonbyTag(row, col);

            if (row > 2 && col > 2) return BotRight.GetButtonbyTag(row, col);
                
            
            return null;
        }
        /// <summary>
        /// Zeigt den Endscreen an 
        /// </summary>
        /// <param name="text"></param>
        /// <param name="colorBrush"></param>
        private void EndScreening(string text, LinearGradientBrush colorBrush)
        {
            PlayerTurnPanel.Visibility = Visibility.Hidden;
            ResultText.Text = text;
            WinnerIcon.Fill = colorBrush;
            EndScreen.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// Wechselt das Usercontrol 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MainModel.SetNewContent(new Menu());
        }

        #endregion

        #region Events
        /// <summary>
        /// ButtonClick event fürt den Zug aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridButton_Click(object sender, GridButtonClickEventArgs e)
        {
            if (Game.isSinglePlayer && Game.CurrentPlayer == Player.Red)
            {
                return;
            }
            var grid = (GameGrid)sender;
            Quadrant quandrant = (Quadrant)grid.Tag;
            Game.MakeMove(e.Row, e.Column,quandrant, Direction.none);
        }
        /// <summary>
        /// Fürt die Drehung aus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Click_MouseDown(object sender, MouseButtonEventArgs e )
        {
            Image image = (Image)sender;

            ChangeVisibilityRotationButton();
            RotateAnimation(image);
            if (Game.IsWin(Player.Red)|| Game.IsWin(Player.Blue)|| Game.GridFull())
            {
                Game.GameOver = true;
                GameEnded();
                return;
            }
                

            //PrintArray();
          
            Game.SwitchPlayer();
            ChangePlayerIcon();
            
            
                Game.Turned = false;
                
            // Singleplayermodus
            if (Game.isSinglePlayer && Game.CurrentPlayer == Player.Red && Game.GameOver == false)
            {
                
                Game.BestMove = Game.GetBestMove(3);

               
                 Game.MakeMoveComputer(Game.BestMove.Item1, Game.BestMove.Item2, Game.BestMove.Item3, Game.BestMove.Item4);
                
            }
           
        }
        /// <summary>
        /// wenn der Zug gemacht wurde. Visuele änderung. 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="corner"></param>
        private void OnMoveMade(int row, int col, Quadrant corner)
        {
            Changbuttoncolor(row,col);
            Game.Turned = true;
            ChangeVisibilityRotationButton();
         
        }
        /// <summary>
        /// pürft wer gewonnen hat und zeigt dies im EndScreen
        /// </summary>
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
                GetWinPosi();
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x0D, 0x32, 0xC5), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0x8F, 0x96, 0xD6), 0));

                EndScreening("Winner:", backgroundBrush);
            }
            else
            {
                GetWinPosi();
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xF1, 0x02, 0x02), 1));
                backgroundBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xDA, 0xBD, 0xC9), 0));

                EndScreening("Winner:", backgroundBrush);
            }

        }
        /// <summary>
        /// Visuele änderung, Spielerwechsel,Gewinnprüfung
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <param name="quadrant"></param>
        /// <param name="direction"></param>
        private async void OnComputerMove(int r,int c,Quadrant quadrant,Direction direction)
        {
            if (Game.GameOver == true)
            {
                Changbuttoncolor(r, c);
                Move(quadrant, direction);
                return;
            }

            Changbuttoncolor(r, c);
            await Task.Delay(1000);
            Move(quadrant, direction);
            PrintArray();
            Game.SwitchPlayer();
            ChangePlayerIcon();
            Game.Turned = false;
        }
        /// <summary>
        /// Game neustart 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Game.Reset();
            ChangePlayerIcon();
            
            EndScreen.Visibility = Visibility.Hidden;
            PlayerTurnPanel.Visibility = Visibility.Visible;
            foreach (Image image in RotationButtons)
            {
                if (image.Visibility == Visibility.Visible)
                  image.Visibility = Visibility.Hidden;
            }
            
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

        

    }
}
