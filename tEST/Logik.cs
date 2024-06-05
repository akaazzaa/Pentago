using Pentago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace tEST
{
    public class Logik
    {
        public int WinCondition;
        public Player CurrentPlayer;
        public Player[,] board;
        public int TurnsPassed;

        public Logik() 
        {
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            board =  new Player[6, 6];
        }

        private bool IsDiagonalWin()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
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
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 5; col > board.GetLength(1); col--)
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
                
                return true;
            }
            else if (CheckColumn())
            {
               
                return true;
            }
            else if (IsDiagonalWin())
            {
               
                return true;
            }
            else if (IsGridFull())
            {
               
                return true;
            }

            
            return false;

        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        private bool CheckRow()
        {
            WinCondition = 0;
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
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
            for (int c = 0; c < board.GetLength(0); c++)
            {
                for (int r = 0; r < board.GetLength(1); r++)
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
        private bool IsMarked(int r, int c)
        {

           return board[r, c] == CurrentPlayer;

    }
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < board.GetLength(01);)
            {
                for (int c = col; c < board.GetLength(0);)
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
            for (int r = row; r < board.GetLength(0);)
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
        public Player[,] RotateArrayRight()
        {
            int size = board.GetLength(0) / 2;
            Player[,] tmp = new Player[6, 6];

            for (int r = 0; r < size; ++r)
            {

                for (int c = 0; c < size; ++c)
                {
                    tmp[c, size - 1 - r] = board[r, c];
                }

            }
            return tmp;
        }
        public Player[,] RotateTopLeftArrayLeft()
        {
            
            int size = board.GetLength(0) / 2;
            Player[,] tmp = new Player[6, 6];

            for (int r = 0; r < size; ++r)
            {
                for (int c = 0; c < size; ++c)
                {
                    tmp[size - 1 - c, r] = board[r, c];
                }
            }
            return tmp;
        }
        public void RotateCorner(Corner corner, bool rotateLeft)
        {
            int size = board.GetLength(0) / 2;
            Player[,] tmp = new Player[6, 6];

            int startRow = 0, startCol = 0;

            switch (corner)
            {
                case Corner.Topleft:
                    startRow = 0;
                    startCol = 0;
                    break;
                case Corner.Topright:
                    startRow = 0;
                    startCol = size;
                    break;
                case Corner.Botleft:
                    startRow = size;
                    startCol = 0;
                    break;
                case Corner.Botright:
                    startRow = size;
                    startCol = size;
                    break;
            }
            // Temporäres Array zum Drehen vorbereiten
            
            
        }

    

        public void SetPoint(int x, int y)
        {
            board[x,y] = CurrentPlayer;
        }
        public void Ausgabe()
        {
            for (int r = 0;r< board.GetLength(0); ++r)
            {
                for(int c = 0;c < board.GetLength(1); ++c)
                {
                    Console.Write(board[r, c]);
                    Console.Write(" ");
                }
                Console.WriteLine();
            }
        }
    }
}
