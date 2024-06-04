using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Pentago.Klassen
{
    public class Game
    {
        public Player[,] TopLeft { get; set; }
        public Player[,] TopRight { get; set; }
        public Player[,] BotLeft { get; set; }
        public Player[,] BotRight { get; set; }
        public Player CurrentPlayer { get; set; }
        private List<Button> Buttons { get; set; }
        public int Suchtiefe { get; set; }
        public int WinCondition { get; set; }
        public int ArrayRowLenght { get; set; }
        public int HalfArrayRowLenght { get; set; }
        public int ArrayColLenght { get; set; }
        public int HalfArrayColLenght { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; set; }
        public bool Turned { get; set; }
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }
        public bool isSinglePlayer { get; set; }

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
        public event Action MoveMade;
        public event Action ComputerMovemade;
        public static event Action RotateButtonsLeft;
        public static event Action RotateButtonsRight;

        public Game()
        {

            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            Turned = false;
            GameOver = false;
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
            ArrayRowLenght = TopLeft.GetLength(0) + TopRight.GetLength(0);
            HalfArrayRowLenght = TopLeft.GetLength(0);
            ArrayColLenght = ArrayRowLenght;
            HalfArrayColLenght = HalfArrayRowLenght;
            isSinglePlayer = false;
            Suchtiefe = 3;


        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        private bool CanMove(GameGrid grid, int r, int c)
        {
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    return !GameOver && Turned == false && TopLeft[r, c] == Player.None;
                case ("GridTopRight"):
                    return !GameOver && Turned == false && TopRight[r, c] == Player.None;
                case ("GridBotLeft"):
                    return !GameOver && Turned == false && BotLeft[r, c] == Player.None;
                case ("GridBotRight"):
                    return !GameOver && Turned == false && BotRight[r, c] == Player.None;

            }
            return false;
        }
        public void Changbuttoncolor(int row, int col)
        {
            Button button = GetButtonbyTag(row, col);
            if (CurrentPlayer == Player.Blue)
            {
                button.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }
        }
        private Button GetButtonbyTag(int row, int col)
        {
            foreach (var button in Buttons)
            {
                var pos = (Positions)button.Tag;

                if (pos.Row == row && pos.Column == col)
                    return button;


            }
            return null;
        }
        private bool IsMarked(int r, int c)
        {

            if (r < HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return TopLeft[r, c] == CurrentPlayer;
            }
            else if (r < HalfArrayRowLenght && c > HalfArrayRowLenght - 1 && c < ArrayColLenght)
            {
                return TopRight[r, c - HalfArrayColLenght] == CurrentPlayer;
            }
            else if (r >= HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return BotLeft[r - HalfArrayRowLenght, c] == CurrentPlayer;
            }
            else if (r >= HalfArrayRowLenght && r < ArrayRowLenght && c >= ArrayColLenght && c > ArrayColLenght)
            {
                return BotRight[r - HalfArrayRowLenght, c - HalfArrayColLenght] == CurrentPlayer;
            }

            return false;

        }
        private bool CheckRow()
        {
            WinCondition = 0;
            for (int r = 0; r < ArrayRowLenght; r++)
            {
                for (int c = 0; c < ArrayColLenght; c++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;

                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Row, r = r, c = c };
                            return true;
                        }

                    }
                    else
                    {
                        WinCondition = 0;
                    }
                }
                WinCondition = 0;
            }
            return false;
        }
        private bool CheckColumn()
        {
            WinCondition = 0;
            for (int c = 0; c < ArrayColLenght; c++)
            {
                for (int r = 0; r < ArrayRowLenght; r++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Column, r = r, c = c };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;

                    }
                }
                WinCondition = 0;
            }
            return false;
        }
        private bool IsDiagonalWin()
        {
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 0; col < HalfArrayColLenght - 1; col++)
                {
                    if (IsMarked(row, col))
                    {
                        return (CheckDiagonal(row, col));
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 5; col > HalfArrayColLenght; col--)
                {
                    if (IsMarked(row, col))
                    {
                        return (CheckAntiDiagonal(row, col));
                    }
                }
            }
            return false;
        }
        private bool IsWin()
        {
            if (CheckRow())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (CheckColumn())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (IsDiagonalWin())
            {
                GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
                return true;
            }
            else if (IsGridFull())
            {
                GameResult = new GameResult { Winner = Player.None };
                return true;
            }

            GameResult = null;
            return false;

        }
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < ArrayRowLenght;)
            {
                for (int c = col; c < ArrayColLenght;)
                {
                    if (IsMarked(row, col))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Diagonal, c = c, r = r };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;
                        return false;
                    }
                    row++;
                    col++;
                }
            }

            return false;
        }
        private bool CheckAntiDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < ArrayRowLenght;)
            {
                for (int c = col; c >= 0;)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            WinInfo = new WinInfo { WinType = WinType.Antidiagonal, c = col, r = row };
                            return true;
                        }
                    }
                    else
                    {
                        WinCondition = 0;
                        return false;
                    }
                    r++;
                    c--;
                }
            }

            return false;
        }
        private void SwitchPlayer()
        {
            if (CurrentPlayer == Player.Blue)
            {
                CurrentPlayer = Player.Red;
            }
            else
            {
                CurrentPlayer = Player.Blue;
            }

        }
        public void MakeMove(int row, int col, GameGrid grid, List<Button> buttons)
        {
            if (!CanMove(grid, row, col))
            {
                return;
            }

            Buttons = buttons;

            Changbuttoncolor(row, col);
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    TopLeft[row, col] = CurrentPlayer;
                    break;
                case ("GridTopRight"):
                    TopRight[row, col] = CurrentPlayer;
                    break;
                case ("GridBotLeft"):
                    BotLeft[row, col] = CurrentPlayer;
                    break;
                case ("GridBotRight"):
                    BotRight[row, col] = CurrentPlayer;
                    break;
            }

            TurnsPassed++;
            if (IsWin())
            {
                GameOver = true;
                GameEnded?.Invoke(GameResult);
            }
            else
            {
                MoveMade?.Invoke();
            }

        }
        public void Reset()
        {
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            GameOver = false;
            GameRestarted?.Invoke();
        }
        public void RotateArray(string buttonname, GameGrid topLeft, GameGrid topRight, GameGrid botLeft, GameGrid botRight)
        {
            switch (buttonname)
            {
                case ("BTLL"):
                    TopLeft = RotateArrayLeft(TopLeft);
                    topLeft.SetNewPositions(Direction.Left);
                    Move(topLeft, -90);
                    break;
                case ("BTLR"):
                    TopLeft = RotateArrayRight(TopLeft);
                    topLeft.SetNewPositions(Direction.Right);
                    Move(topLeft, 90);
                    break;
                case ("BTRL"):
                    TopRight = RotateArrayLeft(TopRight);
                    topRight.SetNewPositions(Direction.Left);
                    Move(topRight, -90);
                    break;
                case ("BTRR"):
                    TopRight = RotateArrayRight(TopRight);
                    topRight.SetNewPositions(Direction.Right);
                    Move(topRight, 90);
                    break;
                case ("BBLL"):
                    BotLeft = RotateArrayRight(BotLeft);
                    botLeft.SetNewPositions(Direction.Right);
                    Move(botLeft, 90);
                    break;
                case ("BBLR"):
                    BotLeft = RotateArrayLeft(BotLeft);
                    botLeft.SetNewPositions(Direction.Left);
                    Move(botLeft, -90);
                    break;
                case ("BBRL"):
                    BotRight = RotateArrayRight(BotRight);
                    botRight.SetNewPositions(Direction.Right);
                    Move(botRight, 90);
                    break;
                case ("BBRR"):
                    BotRight = RotateArrayLeft(BotRight);
                    botRight.SetNewPositions(Direction.Left);
                    Move(botRight, -90);
                    break;
            }
            Turned = false;
            SwitchPlayer();
        }
        private void Move(GameGrid gameGrid, double angle)
        {
            RotateTransform rotateTransformTopLeft = new RotateTransform();
            rotateTransformTopLeft.Angle = gameGrid.CurrentRotation;
            rotateTransformTopLeft.CenterX = 155;
            rotateTransformTopLeft.CenterY = 155;
            gameGrid.RenderTransform = rotateTransformTopLeft;
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = gameGrid.CurrentRotation;
            rotationAnimation.To = gameGrid.CurrentRotation + angle;
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            gameGrid.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);

            gameGrid.CurrentRotation += angle;

        }
        private Player[,] RotateArrayRight(Player[,] array)
        {
            if (array == null)
            {
                return null;
            }
            int size = 3;
            Player[,] tmp = new Player[size, size];

            for (int r = 0; r < size; ++r)
            {

                for (int c = 0; c < size; ++c)
                {
                    tmp[c, size - 1 - r] = array[r, c];
                }

            }
            return tmp;
        }
        private Player[,] RotateArrayLeft(Player[,] array)
        {
            if (array == null)
            {
                return null;
            }
            int size = 3;
            Player[,] tmp = new Player[size, size];

            for (int r = 0; r < size; ++r)
            {
                for (int c = 0; c < size; ++c)
                {
                    tmp[size - 1 - c, r] = array[r, c];
                }
            }
            return tmp;
        }
        
    }
}


