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
        public int ArrayRowLenght;
        public int HalfArrayRowLenght;
        public int ArrayColLenght;
        public int HalfArrayColLenght;
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
            ArrayRowLenght = TopLeft.GetLength(0) + TopRight.GetLength(0);
            HalfArrayRowLenght = TopLeft.GetLength(0);
            ArrayColLenght = ArrayRowLenght;
            HalfArrayColLenght = HalfArrayRowLenght;
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

            if (r < HalfArrayRowLenght  && c < HalfArrayColLenght )
            {
                return TopLeft[r, c] == currentPlayer;
            }
            
            else if (r < HalfArrayRowLenght && c > HalfArrayRowLenght - 1 && c < ArrayColLenght)
            {
                return TopRight[r, c - HalfArrayColLenght] == currentPlayer;
            }

            
            if (r >= HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return BotLeft[r - HalfArrayRowLenght, c] == currentPlayer;
            }
            else
            { 
               return BotRight[r - HalfArrayRowLenght, c - HalfArrayColLenght] == currentPlayer;
            }
           
        }
        private bool CheckRow()
        {
            for (int r = 0; r < ArrayRowLenght; r++)
            {
                for (int c = 0; c < ArrayColLenght; c++)
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
                }

            }
            return false;
        }
        private bool CheckColumn()
        {


            for (int c = 0; c < ArrayColLenght; c++)
            {
                for (int r = 0; r < ArrayRowLenght; r++)
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

                }

            }
            return false;
        }
        private bool IsDigonalPossible()
        {
            //TopLeft to BotRight
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 0; col < HalfArrayColLenght - 1; col++)
                {
                    if (IsMarked(row, col))
                    {
                        if (CheckDiagonal(row, col))
                        {
                            return true;
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            //BotRight to TopLeft
            for (int row = 0; row < HalfArrayRowLenght - 1; row++)
            {
                for (int col = 5; col > HalfArrayColLenght; col--)
                {

                    if (IsMarked(row, col))
                    {
                        if (CheckAntiDiagonal(row, col))
                        {
                            return true;
                        }
                    }

                }
            }


            return false;
        }
        public bool CheckDiagonal(int row, int col)
        {
 
            for (int r = row; r < ArrayRowLenght;)
            {
                for (int c = col; c < ArrayColLenght;)
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
        public bool CheckAntiDiagonal(int row, int col)
        {
            for (int r = row; r < ArrayRowLenght;)
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
        public bool CheckWin()
        {
            if (CheckRow())
            {
                return true;
            }
            if (CheckColumn())
            {
                return true;
            }
            if (IsDigonalPossible()) 
            {
                return true;
            }
            return false;
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

