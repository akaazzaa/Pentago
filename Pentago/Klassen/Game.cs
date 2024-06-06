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
        public Player[,] GameBoard { get; set;}
        public Player CurrentPlayer { get; set; }
        public int depth { get; set; }
        public int WinCondition { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; set; }
        public bool Turned { get; set; }
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }
        public bool isSinglePlayer { get; set; }

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
        public event Action<int,int,GameGrid> MoveMade;
 
        public Game()
        {
            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            Turned = false;
            GameOver = false;
            GameBoard = new Player[6, 6];
            isSinglePlayer = false; 
            depth = 3;
        }

        #region Rotation
        private Player[,] Rotate3x3(Player[,] tmp, Direction direction)
        {
            Player[,] rotated = new Player[3, 3];
            if (direction == Direction.Right)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        rotated[r, c] = tmp[2 - c, r];
                    }
                }
            }
            else if (direction == Direction.Left)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        rotated[r, c] = tmp[c, 2 - r];
                    }
                }
            }
            else
            {
                return null;
            }
            return rotated;
        }
        public Player[,] RotateField(Corner corner, Direction direction)
        {
            int rowStart, colStart;
            switch (corner)
            {
                case Corner.Topleft:
                    rowStart = 0;
                    colStart = 0;
                    break;
                case Corner.Topright:
                    rowStart = 0;
                    colStart = 3;
                    break;
                case Corner.Botleft:
                    rowStart = 3;
                    colStart = 0;
                    break;
                case Corner.Botright:
                    rowStart = 3;
                    colStart = 3;
                    break;
                default:
                    return null;
            }


            Player[,] tmp = new Player[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    tmp[r, c] = GameBoard[rowStart + r, colStart + c];
                }
            }


            Player[,] rotatedarray = Rotate3x3(tmp, direction);


            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    GameBoard[rowStart + r, colStart + c] = rotatedarray[r, c];
                }
            }

            return GameBoard;
        }
        #endregion

        #region WinCkeck
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < GameBoard.GetLength(0);)
            {
                for (int c = col; c < GameBoard.GetLength(1);)
                {
                    if (IsMarked(row, col))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {

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
            for (int r = row; r < GameBoard.GetLength(0);)
            {
                for (int c = col; c >= 0;)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {

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
        private bool CheckRow()
        {
            WinCondition = 0;
            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;

                        if (WinCondition == 5)
                        {

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
            for (int c = 0; c < GameBoard.GetLength(0); c++)
            {
                for (int r = 0; r < GameBoard.GetLength(1); r++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {

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
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
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
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 5; col > GameBoard.GetLength(1); col--)
                {
                    if (IsMarked(row, col))
                    {
                        return (CheckAntiDiagonal(row, col));
                    }
                }
            }
            return false;
        }
        private bool IsMarked(int r, int c)
        {
            return GameBoard[r, c] == CurrentPlayer;
        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
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
            //else if (IsDiagonalWin())
            //{
            //    GameResult = new GameResult { Winner = CurrentPlayer, WinInfo = this.WinInfo };
            //    return true;
            //}
            //else if (IsGridFull())
            //{
            //    GameResult = new GameResult { Winner = Player.None };
            //    return true;
            //}

            GameResult = null;
            return false;

        }
        #endregion

        #region Gameloop
        public void MakeMove(int row, int col, GameGrid grid)
        {
            if (!CanMove(row, col))
            {
                return;
            }
            SetPoint(row, col);

            TurnsPassed++;

            if (IsWin())
            {
                GameOver = true;
                GameEnded?.Invoke(GameResult);
            }
            else
            {
                MoveMade?.Invoke(row, col, grid);
            }

        }
        private bool CanMove(int r, int c)
        {
            return !GameOver && Turned == false && GameBoard[r, c] == Player.None;
        }
        public void SetPoint(int x, int y)
        {
            GameBoard[x, y] = CurrentPlayer;
        }
        public void SwitchPlayer()
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
        public void Reset()
        {
            GameBoard = new Player[6, 6];
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            GameOver = false;
            GameRestarted?.Invoke();
        }
        #endregion
















    }
}


