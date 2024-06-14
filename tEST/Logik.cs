using Pentago;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace tEST
{
    public class Logik
    {
        public int WinCondition;
        public Player CurrentPlayer;
        public Player[,] board;
        public int TurnsPassed;
        public int[,] werte;
        public Logik() 
        {
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            board =  new Player[6, 6];
            werte = new int[,]
            {
                { 3,3,1  ,1,3,3 }, 
                { 3,3,1  ,1,3,3 }, 
                { 1,1,0  ,0,1,1 }, 

                { 1,1,0  ,0,1,1 },
                { 3,3,1  ,1,3,3 },
                { 3,3,1  ,1,3,3 },
            };      
        }

        private bool IsDiagonalWin(Player player)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    if (IsMarked(row, col, player))
                    {
                        return (CheckDiagonal(row, col,player) || CheckAntiDiagonal(row,col,player));
                    }
                    else
                    {
                        continue;
                    }

                }
            }
        
            return false;
        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        private bool CheckRow(Player player)
        {
            WinCondition = 0;
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (IsMarked(r, c,player))
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
        private bool CheckColumn(Player player)
        {
            WinCondition = 0;
            for (int c = 0; c < board.GetLength(0); c++)
            {
                for (int r = 0; r < board.GetLength(1); r++)
                {
                    if (IsMarked(r, c, player))
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
        private bool IsMarked(int r, int c,Player player)
        {

           return board[r, c] == player;

    }
        private bool CheckDiagonal(int row, int col,Player player)
        {
            WinCondition = 0;
            for (int r = row; r < board.GetLength(0);)
            {
                for (int c = col; c < board.GetLength(1);)
                {
                    if (IsMarked(row, col, player))
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
        private bool CheckAntiDiagonal(int row, int col,Player player)
        {
            WinCondition = 0;
            for (int r = row; r < board.GetLength(0);)
            {
                for (int c = col; c >= 0;)
                {
                    if (IsMarked(r, c, player))
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
        static Player[,] Rotate3x3(Player[,] tmp, Direction direction )
        {
            Player[,] rotated = new Player[3,3];
            if (direction == Direction.Right)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        rotated[i, j] = tmp[2 - j, i];
                    }

                }
            return tmp;
            }
            else if (direction == Direction.Left)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        rotated[i, j] = tmp[j, 2 - i];
                    }
                }
            return tmp;
            }
            else
            {
                return null;
            }
            return rotated;
        }

        public Player[,] RotateField( Quadrant corner, Direction direction)
        {
            int rowStart, colStart;
            switch (corner)
            {
                case Quadrant.Topleft:
                    rowStart = 0;
                    colStart = 0;
                    break;
                case Quadrant.Topright:
                    rowStart = 0;
                    colStart = 3;
                    break;
                case Quadrant.Botleft:
                    rowStart = 3;
                    colStart = 0;
                    break;
                case Quadrant.Botright:
                    rowStart = 3;
                    colStart = 3;
                    break;
                default:
                    return null;
            }


            Player[,] tmp = new Player[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    tmp[i, j] = board[rowStart + i, colStart + j];
                }
            }

           

            Player[,] rotatedarray = Rotate3x3(tmp, direction);


            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[rowStart + i, colStart + j] = rotatedarray[i, j];
                }
            }

            return board;
        }

        public void SetPoint(int x, int y)
        {
            board[x,y] = CurrentPlayer;
        }
        public void PrintArray()
        {
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    Console.Write(board[i, j] + "\t");
                    
                }
                
                Console.WriteLine();
                Console.WriteLine();    
            }
        }
        public int Auswertung(Player player)
        {
           if (CheckRow(player)|| CheckColumn(player) || IsDiagonalWin(player))
           {
                if (player == Player.Blue)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return 0;

        }
        private int Bewertung()
        {
            int wert = 1; 
            for (int r = 0;r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (board[r,c] == Player.Blue)
                    {
                        wert = wert - werte[r,c];
                    }
                    if (board[r,c] == Player.Red)
                    {
                        wert = wert + werte[r,c];
                    }
                }
               
            }
            return wert;
        }

        //private int Max(int depth)
        //{
        //    if (depth == 0)
        //    {
        //        return Bewertung();
        //    }
        //    int maxvalue = -999;

        //}
    }
}
