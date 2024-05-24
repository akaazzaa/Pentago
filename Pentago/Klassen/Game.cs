﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        public int WinCondition { get; set; }
        public int ArrayRowLenght { get; set; }
        public int HalfArrayRowLenght { get; set; }
        public int ArrayColLenght { get; set; }
        public int HalfArrayColLenght { get; set; }
        public bool GameOver { get; set; }
        public int TurnsPassed { get; set; } 
        public bool Turned { get; set; }
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }
        

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
        public event Action MoveMade;

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
        public void Changbuttoncolor(Button button)
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
        public async void RotateArray(string buttonname,GameGrid topLeft,GameGrid topRight,GameGrid botLeft,GameGrid botRight)
        {
            switch (buttonname)
            {
                case ("BTLL"):
                    TopLeft = RotateArrayLeft(TopLeft);
                    Move(topLeft,-90,0);
                    break;
                case ("BTLR"):
                   TopLeft = RotateArrayRight(TopLeft);
                    Move(topLeft,90,0);
                    break;
                case ("BTRL"):
                    TopRight = RotateArrayLeft(TopRight);
                    Move(topRight,-90, 0);
                    break;
                case ("BTRR"):
                   TopRight = RotateArrayRight(TopRight);
                    Move(topRight,90, 0);
                    break;
                case ("BBLL"):
                    BotLeft = RotateArrayRight(BotLeft);
                    Move(botLeft,90, 0);
                    break;
                case ("BBLR"):
                    BotLeft = RotateArrayLeft(BotLeft);
                    Move(botLeft,-90, 0);
                    break;
                case ("BBRL"):
                    BotRight = RotateArrayRight(BotRight);
                    Move(botRight,90, 0);
                    break;
                case ("BBRR"):
                    BotRight = RotateArrayLeft(BotRight);
                    Move(botRight,-90, 0);
                    break;
            }
            Turned = false;
            SwitchPlayer();
        }
        /// <summary>
        ///  Fehler in der Rotation Animation 
        /// </summary>
        /// <param name="gameGrid"></param>
        /// <param name="angle"></param>
        /// <param name="currentRotation"></param>
        private void Move(GameGrid gameGrid, double angle, double currentRotation)
        {
            RotateTransform rotateTransformTopLeft = new RotateTransform();
            rotateTransformTopLeft.Angle = currentRotation;
            rotateTransformTopLeft.CenterX = 155;
            rotateTransformTopLeft.CenterY = 155;
            gameGrid.RenderTransform = rotateTransformTopLeft;
            DoubleAnimation rotationAnimation = new DoubleAnimation();
            rotationAnimation.From = currentRotation;
            rotationAnimation.To = currentRotation + angle;
            rotationAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));

            gameGrid.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
            
            currentRotation += angle;
            
        }
        private Player[,] RotateArrayRight(Player[,] array)
        {
               if (array == null)
               {
                 return null;
                }
                int size = 3;
                Player[,] tmp = new Player[size, size];

                for (int i = 0; i < size; ++i)
                {

                    for (int j = 0; j < size; ++j)
                    {
                        tmp[i, j] = array[size - j - 1, i];
                    }

                }
                return tmp;
        }
        private Player[,] RotateArrayLeft(Player[,] array)
        {
            int size = 3;
            Player[,] tmp = new Player[size, size];

            for(int i = 0;i < size; ++i)
            {
                for(int j = 0;j < size; ++j)
                {
                    tmp[i,j] = array[j, size - i - 1];
                }
            }
            return tmp;
        }
    }
}

