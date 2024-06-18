using System;
using System.Collections.Generic;
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
       

        public event Action<GameResult> GameEnded;
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
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            // Feld länge such Diagonal
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c < GameBoard.GetLength(1); r++, c++)
            {// Wennn der stein nich None ist = True;
                if (IsMarked(r, c))
                {
                    Winposi[WinCondition] = (r, c);
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
        private bool CheckAntiDiagonal(int row, int col)
        {
            
            WinCondition = 0;
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c >= 0; r++, c--)
            {
                if (IsMarked(r, c))
                {
                    Winposi[WinCondition] = (r, c);
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
        private bool CheckRow()
        {
            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                WinCondition = 0;
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (IsMarked(r, c))
                    {
                        Winposi[WinCondition] = (r, c);
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
        private bool CheckColumn()
        {
            for (int c = 0; c < GameBoard.GetLength(1); c++)
            {
                WinCondition = 0;
                for (int r = 0; r < GameBoard.GetLength(0); r++)
                {
                    if (IsMarked(r, c))
                    {
                        Winposi[WinCondition] = (r, c);
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
        private bool IsDiagonalWin()
        {
            for (int row = 0; row < GameBoard.GetLength(0); row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1); col++)
                {
                    if (CheckDiagonal(row, col) || CheckAntiDiagonal(row, col))
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
        private bool IsMarked(int r, int c)
        {
            return GameBoard[r, c] == CurrentPlayer;
        }
        /// <summary>
        ///  Gibt zurück ob das Spielfeld voll ist
        /// </summary>
        /// <returns></returns>
        private bool IsGridFull()
        {
            return TurnsPassed == GameBoard.GetLength(0) * GameBoard.GetLength(1);
        }
        /// <summary>
        ///  Gewinnprüfung
        /// </summary>
        /// <returns></returns>
        public bool IsWin()
        {
            if (CheckRow() || CheckColumn() || IsDiagonalWin())
            {
                GameResult = new GameResult { Winner = CurrentPlayer };
                return true;
            }
            else if (IsGridFull())
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
            if(!CanMove(row, col))
            {
                return;
            }
                SetPoint(row, col);
                TurnsPassed++;
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
            SetPoint(row,col);
            RotateField(corner, direction);
 
            TurnsPassed++;
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
        /// Geht das Feld durch und für jedes nicht belegte Feld erstelt eine 2 Tuple mit allen möglcihen Zügen.
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
        /// Start der Minmax algorithmus
        /// Bekommt eine Suchtiefe und geht alle Züge durch und füht dise aus, testet die Züge bewertet die Züge und gibt einen wert zurück.
        /// </summary>
        /// <param name="depth"></param>
        /// <returns>bester Zug</returns>
        public Tuple<int, int, Quadrant, Direction> GetBestMove(int depth)
        {
            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();
            
            Tuple<int, int, Quadrant, Direction> bestMove = null;
            int bestValue = int.MinValue;

            foreach (var move in moves)
            {
                SimulateMove(move, Player.Red);
                int moveValue = Minimieren(depth - 1, int.MinValue, int.MaxValue, Player.Blue);
                UndoMove(move);

                if (moveValue > bestValue)
                {
                    bestMove = move;
                    bestValue = moveValue;
                }
            }

            return bestMove;
        }
        /// <summary>
        ///  Minmax Algorithmus 
        /// </summary>
        /// <param name="depth">Tiefe </param>
        /// <param name="alpha">untergrnze</param>
        /// <param name="beta">obergrenze</param>
        /// <param name="player">Spieler</param>
        /// <returns></returns>
        private int Maximieren(int depth, int alpha, int beta, Player player)
        {
            int score = EvaluateBoard();
            if (depth == 0 || score == int.MaxValue || score == int.MinValue)
                return score;

            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();
            int maxEval = int.MinValue;

            foreach (var move in moves)
            {
                SimulateMove(move, player);
                int eval = Minimieren(depth - 1, alpha, beta, Player.Blue);
                UndoMove(move);

                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return maxEval;
        }
        /// <summary>
        /// Minmax Algorithmus 
        /// </summary>
        /// <param name="depth"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        private int Minimieren(int depth, int alpha, int beta, Player player)
        {
            int score = EvaluateBoard();
            if (depth == 0 || score == int.MaxValue || score == int.MinValue)
                return score;

            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();
            int minEval = int.MaxValue;

            foreach (var move in moves)
            {
                SimulateMove(move, player);
                int eval = Maximieren(depth - 1, alpha, beta, Player.Red);
                UndoMove(move);

                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }
            return minEval;
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

         
            int blueScore = 0;
            switch (bluePoints)
            {
                case 4:
                    blueScore = -100;
                    break;
                case 3:
                    blueScore = -10; 
                    break;
                case 2:
                    blueScore = -1; 
                    break;
            }

           
            int redScore = 0;
            switch (redPoints)
            {
                case 4:
                    redScore = 100;
                    break;
                case 3:
                    redScore = 10; 
                    break;
                case 2:
                    redScore = 1;
                    break;
            }

            
            return blueScore + redScore;
        }


        #endregion

       


    }
}
    


