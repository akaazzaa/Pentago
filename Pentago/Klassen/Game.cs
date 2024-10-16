﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.RightsManagement;
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
        public int WinCondition { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; set; }
        public bool Turned { get; set; }
        public GameResult GameResult { get; set; }
        public bool isSinglePlayer { get; set; }
        public (int, int)[] Winposi { get; set; }
        public int Searchdepth { get; set; }
        public Tuple<int, int, Quadrant, Direction> BestMove {  get; set; }
        


        public event Action GameEnded;
        public event Action<int,int,Quadrant> MoveMade;
        public event Action<int, int, Quadrant,Direction> ComputerMove;
  
        public Game()
        {
            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            Turned = false;
            GameOver = false;
            GameBoard = new Player[6, 6];
            isSinglePlayer = false;
            Winposi = new (int, int)[5];
            BestMove = new Tuple<int, int, Quadrant, Direction>(0,0,Quadrant.None,Direction.none);
        }

        #region Rotation
        /// <summary>
        /// Bekommt ein 3x3 array und eine Richtung und dreht das array um 90°
        /// </summary>
        /// <param name="tmp"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Player[,] Rotate3x3(Player[,] tmp, Direction direction)
        {
            Player[,] rotated = new Player[3, 3];
            // drehen nach rechts
            if (direction == Direction.Right)
            {
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        rotated[r, c] = tmp[2 - c, r];
                    }
                }
            }// dehen nach links
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
        /// <summary>
        /// Entscheidet welcher Qudrant gedreht werden soll.
        /// Macht aus dem 6x6 ein 3x3 und ruft Rotate 3x3 auf.
        /// Fügt das gedrehte array wieder dem 6x6 hinzu.
        /// </summary>
        /// <param name="corner"></param>
        /// <param name="direction"></param>
        public void RotateField(Quadrant corner, Direction direction)
        {


            int rowStart = 0;
            int colStart = 0;
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

           
        }
        #endregion

        #region WinCkeck
        /// <summary>
        /// Beckommt eine Row und eine Col als startPunkt und Prüft dann ob der Spieler
        /// Diagonal gewonnen hat.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool CheckDiagonal(int row, int col,Player player)
        {
            WinCondition = 0;
            // Feld länge such Diagonal
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c < GameBoard.GetLength(1); r++, c++)
            {// Wennn der stein nich None ist = True;
                if (IsMarked(r, c,player))
                {
                    if (GameBoard[r, c] == player)
                    {
                        Winposi[WinCondition] = (r, c);
                    }
                    WinCondition++;
                    // WinCondition == 5 = Gewonnen;
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
            return false;
        }
        /// <summary>
        /// Prüft die gegegesetzte Diagonale 
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private bool CheckAntiDiagonal(int row, int col,Player player)
        {
            
            WinCondition = 0;
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c >= 0; r++, c--)
            {
                if (IsMarked(r, c,player))
                {
                    if (GameBoard[r, c] == player)
                    {
                        Winposi[WinCondition] = (r, c);
                    }
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
            return false;
        }
        /// <summary>
        /// Prüft die Reihen auf Sieg
        /// </summary>
        /// <returns></returns>
        private bool CheckRow(Player player)
        {
            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                WinCondition = 0;
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (IsMarked(r, c, player))
                    {
                        if (GameBoard[r, c] == player)
                        {
                            Winposi[WinCondition] = (r, c);
                        }
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
        /// <summary>
        /// Prüft die Spalten auf Sieg
        /// </summary>
        /// <returns></returns>
        private bool CheckColumn(Player player)
        {
            for (int c = 0; c < GameBoard.GetLength(1); c++)
            {
                WinCondition = 0;
                for (int r = 0; r < GameBoard.GetLength(0); r++)
                {
                    if (IsMarked(r, c,player))
                    {
                        if (GameBoard[r, c] == player)
                        {
                            Winposi[WinCondition] = (r, c);
                        }
                        
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
        /// <summary>
        /// Prüft Diagonal
        /// </summary>
        /// <returns></returns>
        private bool IsDiagonalWin(Player player)
        {
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    if (CheckDiagonal(row, col, player) || CheckAntiDiagonal(row, col, player))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// return true wenn das anzuschauende Feld = aktueller Speieler
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        private bool IsMarked(int r, int c,Player player)
        {
            return GameBoard[r, c] == player;
        }
        
        /// <summary>
        ///  Gibt zurück ob das Spielfeld voll ist
        /// </summary>
        /// <returns></returns>
        public bool IsGridFull()
        {
            return TurnsPassed == 36;
        }
        /// <summary>
        ///  Gewinnprüfung
        /// </summary>
        /// <returns></returns>
        public bool IsWin(Player player)
        {
            if (CheckRow(player) || CheckColumn(player) || IsDiagonalWin(player))
            {
                GameResult = new GameResult { Winner = player };
                return true;
            }
            
            return false;
        }
        public bool GridFull()
        {
            if (IsGridFull())
            {
                GameResult = new GameResult { Winner = Player.None };
                return true;
            }
            return false;
        }
        #endregion

        #region Gameloop
        /// <summary>
        ///  Fürt den Spielerzug aus triggert ein event
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="corner"></param>
        /// <param name="direction"></param>
        public void MakeMove(int row,int col, Quadrant corner,Direction direction)
        {
            if (!CanMove(row, col))
            {
                return;
            }
            SetPoint(row, col);
            TurnsPassed++;
            if (IsWin(CurrentPlayer)|| GridFull())
            {
                GameOver = true;
                GameEnded?.Invoke();
            }
                
                MoveMade?.Invoke(row, col, corner);
        }
        /// <summary>
        /// Fürt den Computerzug aus und triggert ein Event
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="corner"></param>
        /// <param name="direction"></param>
        public void MakeMoveComputer(int row, int col, Quadrant corner, Direction direction)
        {
            if (!CanMove(row, col))
            {
                return;
            }
            SetPoint(row,col);
            
            if (IsWin(CurrentPlayer)|| GridFull())
            {
                GameOver = true;
                GameEnded?.Invoke();
            }
            if (!GameOver)
            {
                RotateField(corner, direction);
            }
            TurnsPassed++;
            if (IsWin(Player.Blue) || IsWin(Player.Red) || GridFull())
            {
                GameOver = true;
                GameEnded?.Invoke();
            }
 
            
            ComputerMove?.Invoke(row, col, corner,direction);
        }
        /// <summary>
        /// Prüft ob ein stein gesetzt werden kann. 
        /// </summary>
        /// <param name="r"></param>
        /// <param name="c"></param>
        /// <returns>bool</returns>
        private bool CanMove(int r, int c)
        {
            return !GameOver && Turned == false && GameBoard[r, c] == Player.None;
        }
        /// <summary>
        /// Setzt einen Sein in das Feld.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPoint(int x, int y)
        {
            GameBoard[x, y] = CurrentPlayer;
        }
        /// <summary>
        /// Wechselt den aktuellen Spieler 
        /// </summary>
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
        /// <summary>
        /// Setz alles zurück.
        /// </summary>
        public void Reset()
        {
            GameBoard = new Player[6, 6];
            WinCondition = 0;
            CurrentPlayer = Player.Blue;
            TurnsPassed = 0;
            Turned = false;
            GameOver = false;
        }
        #endregion

        #region Computer
        /// <summary>
        /// Geht das Feld durch und für jedes nicht belegte Feld erstelt eine 2 Tuple mit allen möglichen Zügen.
        /// </summary>
        /// <returns>Liste der Züge</returns>
        public List<Tuple<int, int, Quadrant, Direction>> GetAllMoves()
        {
            List<Tuple<int, int, Quadrant, Direction>> moves = new List<Tuple<int, int, Quadrant, Direction>>();

            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (GameBoard[r, c] == Player.None)
                    {
                        for (int quadrant = 0; quadrant < 4; quadrant++)
                        {
                            moves.Add(Tuple.Create(r, c, (Quadrant)quadrant, Direction.Right));
                            moves.Add(Tuple.Create(r, c, (Quadrant)quadrant, Direction.Left));
                        }
                    }
                }
            }
            return moves;
        }
        /// <summary>
        /// Macht einen ausgeführten Zug wieder rückgängig
        /// </summary>
        /// <param name="move"></param>
        private void UndoMove(Tuple<int, int, Quadrant, Direction> move)
        {
            SimulateRotation(GameBoard, move.Item3, OppositeDirection(move.Item4));
            GameBoard[move.Item1, move.Item2] = Player.None;
        }
        /// <summary>
        /// Bekommt eine Richtung und gibt die gegengesetzte Richtung zurück.
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        private Direction OppositeDirection(Direction direction)
        {
            if (direction == Direction.Left)
            {
                return direction = Direction.Right;
            }
            else
            {
                return direction = Direction.Left;
            }
        }
        /// <summary>
        /// Start der Alpha Beta Susche. Geht die Liste der züge durch und speichert den erhaltenen wert bis es ein besserer gefunden wurde. 
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        public Tuple<int, int, Quadrant, Direction> GetBestMove(int depth)
        {
            Tuple<int, int, Quadrant, Direction> bestMove = null;
            int alpha = int.MinValue;
            int beta = int.MaxValue;
            int bestValue = int.MinValue;

            foreach (var move in GetAllMoves())
            {
                SimulateMove(move,Player.Red);
                int moveValue = Min(depth - 1, alpha, beta);
                UndoMove(move);

                if (moveValue > bestValue)
                {
                    bestValue = moveValue;
                    bestMove = move;
                }

                alpha = Math.Max(alpha, bestValue);
            }

            return bestMove;
        }
        /// <summary>
        ///   Durchläuft alle Züge,führt den Zug der maxSpielers aus,rekusiver aufruf der Methoden Max und Min,SpielerMax und Min wechseln so Züge bis einer gewonnen hat oder die die Tiefe gleich 0 ist
        ///   dann wird der Zug Bewertet. Der beste wert wird der nächste Zug des Computers.
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <returns></returns>
        private int Max(int depth, int alpha, int beta)
        {
            if (depth == 0 )
                return EvaluateBoard();

            int value = int.MinValue;

            foreach (var move in GetAllMoves())
            {
                SimulateMove(move, Player.Red);
                value = Math.Max(value, Min(depth - 1, alpha, beta));
                UndoMove(move);

                if (value >= beta)
                    return value;

                alpha = Math.Max(alpha, value);
            }

            return value;
        }

        private int Min(int depth, int alpha, int beta)
        {
            if (depth == 0 )
                return EvaluateBoard();

            int value = int.MaxValue;

            foreach (var move in GetAllMoves())
            {
                SimulateMove(move, Player.Blue);
                value = Math.Min(value, Max(depth - 1, alpha, beta));
                UndoMove(move);

                if (value <= alpha)
                    return value;

                beta = Math.Min(beta, value);
            }

            return value;
        }
    /// <summary>
    /// Führt einen Zug aus
    /// </summary>
    /// <param name="move"></param>
    /// <param name="player"></param>
    private void SimulateMove(Tuple<int, int, Quadrant, Direction> move,Player player)
        {
            GameBoard[move.Item1, move.Item2] = player;
            SimulateRotation(GameBoard ,move.Item3, move.Item4);
        }
        /// <summary>
        /// Rotiert das Array
        /// </summary>
        /// <param name="tmpboard"></param>
        /// <param name="quadrant"></param>
        /// <param name="direction"></param>
        private void SimulateRotation(Player[,] tmpboard,Quadrant quadrant, Direction direction)
        {
            int rowStart, colStart;
            switch (quadrant)
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
                    return;
            }


            Player[,] tmp = new Player[3, 3];
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    tmp[r, c] = tmpboard[rowStart + r, colStart + c];
                }
            }


            Player[,] rotatedarray = Rotate3x3(tmp, direction);


            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    tmpboard[rowStart + r, colStart + c] = rotatedarray[r, c];
                }
            }


        }
        /// <summary>
        ///  Bewertet die aktuelle positionen auf dem Feld
        /// </summary>
        /// <returns></returns>
        private int EvaluateBoard()
        {
            int score = 0;
            int rows = GameBoard.GetLength(0);
            int cols = GameBoard.GetLength(1);

            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (c <= cols - 5)
                    {
                        score += EvaluateLine(GameBoard, r, c, 0, 1);
                    }
                    if (r <= rows - 5)
                    {
                        score += EvaluateLine(GameBoard, r, c, 1, 0); 
                    }
                    if (r <= rows - 5 && c <= cols - 5)
                    {
                        score += EvaluateLine(GameBoard, r, c, 1, 1); 
                    }
                    if (r >= 4 && c <= cols - 5)
                    {
                        score += EvaluateLine(GameBoard, r, c, -1, 1); 
                    }
                }
            }
            
            return score;
        }
        /// <summary>
        ///  zählt die Stein in einer Reihe und verteilt Punkte.
        ///  für 5 gib es die maximale Punktzahl und dann absteigend
        /// </summary>
        /// <param name="board"></param>
        /// <param name="startRow"></param>
        /// <param name="startCol"></param>
        /// <param name="rowDir"></param>
        /// <param name="colDir"></param>
        /// <returns></returns>
        private int EvaluateLine(Player[,] board, int startRow, int startCol, int rowDir, int colDir)
        {
            int bluePoints = 0;
            int redPoints = 0;

            for (int i = 0; i < 5; i++)
            {
                Player field = board[startRow + i * rowDir, startCol + i * colDir];
                if (field == Player.Blue)
                {
                    bluePoints++;
                }
                else if (field == Player.Red)
                {
                    redPoints++;
                }
            }

            if (bluePoints == 5)
            {
                return int.MinValue;
            }
            if (redPoints == 5)
            {
                return int.MaxValue; 
            }

            switch (bluePoints)
            {
                case 4:
                    return -100;
                    
                case 3:
                    return -10; 
                   
                case 2:
                    return -1; 
                    
            }

            switch (redPoints)
            {
                case 4:
                    return 100;
                    
                case 3:
                    return 10; 
                    
                case 2:
                    return 1;
                    
            }

            return redPoints;
        }


        #endregion

       


    }
}
    


