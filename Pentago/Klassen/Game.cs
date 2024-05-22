using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public Player[,] TopLeft { get; set; }
        public Player[,] TopRight { get; set; }
        public Player[,] BotLeft { get; set; }
        public Player[,] BotRight { get; set; }

        public Game()
        {
            currentPlayer = Player.Blue;
            WinCondition = 0;
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
        }

        public void Changbuttoncolor(Button button)
        {
            if (currentPlayer == Player.Blue)
            {
                button.Background = new SolidColorBrush(Colors.Blue);
            }
            else
            {
                button.Background = new SolidColorBrush(Colors.Red);
            }
        }
        private bool IsMarked(int r, int c)
        {

            int rowsTop = TopLeft.GetLength(0);
            int colsLeft = TopLeft.GetLength(1);

            if (r < rowsTop && c < colsLeft)
            {
                return TopLeft[r, c] == currentPlayer;
            }
            else if (r < rowsTop && c >= colsLeft)
            {
                return TopRight[r, c - colsLeft] == currentPlayer;
            }
            else if (r >= rowsTop && c < colsLeft)
            {
                return BotLeft[r - rowsTop, c] == currentPlayer;
            }
            else
            {
                return BotRight[r - rowsTop, c - colsLeft] == currentPlayer;
            }

        }

        private bool CheckRow()
        {
            int rows = TopLeft.GetLength(0) + BotLeft.GetLength(0);
            int cols = TopLeft.GetLength(1) + TopRight.GetLength(1);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
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
            int rows = TopLeft.GetLength(0) + BotLeft.GetLength(0);
            int cols = TopLeft.GetLength(1) + TopRight.GetLength(1);

            for (int c = 0; c < cols; c++)
            {
                for (int r = 0; r < rows; r++)
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
                WinCondition = 0; // Reset WinCondition after each column
            }
            return false;
        }


        public bool CheckDiagonal()
        {
            int rows = TopLeft.GetLength(0) + BotLeft.GetLength(0);
            int cols = TopLeft.GetLength(1) + TopRight.GetLength(1);

            // Diagonal from top-left to bottom-right
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (r == c && IsMarked(r, c))
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
            }

            WinCondition = 0;

            // Diagonal from top-right to bottom-left
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (r + c == cols - 1 && IsMarked(r, c))
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
            }

            return false;
        }

        public bool CheckWin()
        {
            
            return CheckRow() || CheckColumn() || CheckDiagonal();
        }

        public void Ausgabe()
        {
            for (int i = 0; i < TopLeft.GetLength(0); i++)
            {
                for (int j = 0; j < TopLeft.GetLength(1); j++)
                {
                    Console.Write(TopLeft[i, j] + " ");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < TopRight.GetLength(0); i++)
            {
                for (int j = 0; j < TopRight.GetLength(1); j++)
                {
                    Console.Write(TopRight[i, j] + " ");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < BotLeft.GetLength(0); i++)
            {
                for (int j = 0; j < BotLeft.GetLength(1); j++)
                {
                    Console.Write(BotLeft[i, j] + " ");
                }
                Console.WriteLine();
            }
            for (int i = 0; i < BotRight.GetLength(0); i++)
            {
                for (int j = 0; j < BotRight.GetLength(1); j++)
                {
                    Console.Write(BotRight[i, j] + " ");
                }
                Console.WriteLine();
            }
        }


        public void SwitchPlayer()
        {
            if (currentPlayer == Player.Blue)
            {
                currentPlayer = Player.Red;

            }
            else
            {
                currentPlayer = Player.Blue;
            }
        }
    }
}

