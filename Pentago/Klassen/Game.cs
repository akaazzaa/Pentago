using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago.Klassen
{
    public class Game
    {  
        public Player currentPlayer;
        public int WinCondition;
        public Player[,] GameGrid {  get; set; }
        
        public Game() 
        {
            currentPlayer = Player.Blue;
            WinCondition = 0;
            GameGrid = new Player[6,6];
        }

        private bool IsMarked(int r, int c)
        {


            if (GameGrid[r, c] != currentPlayer)
            {

                return false;
            }

            return true;

        }

        private bool CheckRow()
        {
            for (int r = 0; r < GameGrid.GetLength(0); r++)
            {
                for (int c = 0; c < GameGrid.GetLength(1); c++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        private bool CheckCol()
        {
            for (int r = 0; r < GameGrid.GetLength(0); r++)
            {
                for (int c = 0; c < GameGrid.GetLength(1); c++)
                {
                    if (IsMarked(c, r))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        private bool IsDiagonal()
        {
            for (int r = 0; r <= 1; r++)
            {
                for (int c = 0; c <= 1; c++)
                {
                    if (IsMarked(r, c))
                    {
                        if (CheckDiagonal(r, c))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        private bool CheckDiagonal(int row, int col)
        {
            for (int r = col; r < GameGrid.GetLength(0); r++)
            {
                for (int c = col; c < GameGrid.GetLength(1); c++)
                {
                    if (IsMarked(r, c))
                    {
                        WinCondition++;
                        if (WinCondition == 5)
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        public bool CheckWin()
        {
            if (CheckRow())
            {
                return true;
            }
            if (CheckCol())
            {
                return true;
            }
            if (IsDiagonal())
            {
                return true;
            }

            return false;
        }
        


    }

}
