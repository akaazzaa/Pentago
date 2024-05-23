using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago.Klassen
{
    public class Game
    {
        public Player[,] TopLeft { get; set; }
        public Player[,] TopRight { get; set; }
        public Player[,] BotLeft { get; set; }
        public Player[,] BotRight { get; set; }
        public Player CurrentPlayer { get; set; }
        public int WinCondition { get; set; }
        public int ArrayRowLenght { get; set; }
        public int HalfArrayRowLenght { get; set; }
        public int ArrayColLenght { get; set; }
        public int HalfArrayColLenght { get; set; }
        public bool GameOver { get; set; }
        public int TurnsPassed { get; set; }  
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
    

        public Game()
        {
            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            GameOver = false;
            TopLeft = new Player[3, 3];
            TopRight = new Player[3, 3];
            BotLeft = new Player[3, 3];
            BotRight = new Player[3, 3];
            ArrayRowLenght = TopLeft.GetLength(0) + TopRight.GetLength(0);
            HalfArrayRowLenght = TopLeft.GetLength(0);
            ArrayColLenght = ArrayRowLenght;
            HalfArrayColLenght = HalfArrayRowLenght;
        }
        private bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        private bool CanMove(GameGrid grid,int r,int c)
        {
            switch (grid.Name)
            {
                case ("GridTopLeft"):
                    return !GameOver && TopLeft[r, c] == Player.None;
                case ("GridTopRight"):
                    return !GameOver && TopRight[r, c] == Player.None;
                case ("GridBotLeft"):
                    return !GameOver && BotLeft[r, c] == Player.None;
                case ("GridBotRight"):
                    return !GameOver && BotRight[r, c] == Player.None;
                    
            }
            return false;
        }
        private void Changbuttoncolor(Button button)
        {
            if (CurrentPlayer == Player.Blue)
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
                return TopLeft[r, c] == CurrentPlayer;
            }
            
            else if (r < HalfArrayRowLenght && c > HalfArrayRowLenght - 1 && c < ArrayColLenght)
            {
                return TopRight[r, c - HalfArrayColLenght] == CurrentPlayer;
            }

            
            if (r >= HalfArrayRowLenght && c < HalfArrayColLenght)
            {
                return BotLeft[r - HalfArrayRowLenght, c] == CurrentPlayer;
            }
            else
            { 
               return BotRight[r - HalfArrayRowLenght, c - HalfArrayColLenght] == CurrentPlayer;
            }
           
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
                            WinInfo = new WinInfo{ WinType = WinType.Row, r = r , c= c };
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
                            WinInfo = new WinInfo { WinType=WinType.Column, r = r ,c = c };
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
            }else if (IsGridFull())
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
                            WinInfo = new WinInfo { WinType = WinType.Diagonal, c = c ,r = r };
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
                            WinInfo = new WinInfo { WinType = WinType.Antidiagonal, c = col ,r = row};
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
        public void MakeMove(int row, int col,GameGrid grid,Button button)
        {
            if (!CanMove(grid,row, col))
            {
                return;
            }
            Changbuttoncolor(button);
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
                SwitchPlayer();
                Changbuttoncolor(button);
            }

        }
    }
}

