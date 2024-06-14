using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Player[,] GameBoardCopy { get; set;}
        public Player CurrentPlayer { get; set; }
        public int WinCondition { get; set; }
        public int TurnsPassed { get; set; }
        public bool GameOver { get; set; }
        public bool Turned { get; set; }
        public WinInfo WinInfo { get; set; }
        public GameResult GameResult { get; set; }
        public bool isSinglePlayer { get; set; }
       

        public event Action<GameResult> GameEnded;
        public event Action GameRestarted;
        public event Action<int,int,Quadrant> MoveMade;
        public event Action<int, int, Quadrant,Direction> ComputerMove;
  
        public Game()
        {
            
            GameBoardCopy = new Player[6, 6];
            CurrentPlayer = Player.Blue;
            WinCondition = 0;
            Turned = false;
            GameOver = false;
            GameBoard = new Player[6, 6];
            isSinglePlayer = false; 
            
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
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c < GameBoard.GetLength(1); r++, c++)
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
            return false;
        }
        private bool CheckAntiDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row, c = col; r < GameBoard.GetLength(0) && c >= 0; r++, c--)
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
            return false;
        }
        private bool CheckRow()
        {
            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                WinCondition = 0;
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
            }
            return false;
        }
        private bool CheckColumn()
        {
            for (int c = 0; c < GameBoard.GetLength(1); c++)
            {
                WinCondition = 0;
                for (int r = 0; r < GameBoard.GetLength(0); r++)
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
            }
            return false;
        }
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
        private bool IsMarked(int r, int c)
        {
            return GameBoard[r, c] == CurrentPlayer;
        }
        private bool IsGridFull()
        {
            return TurnsPassed == GameBoard.GetLength(0) * GameBoard.GetLength(1);
        }
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
        public void MakeMoveComputer(int row, int col, Quadrant corner, Direction direction)
        {
            
            SetPoint(row,col);
            
            
            RotateField(corner, direction);
            
            TurnsPassed++;
            ComputerMove?.Invoke(row, col, corner,direction);
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

        #region Computer
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

        private Quadrant GetRightQuadrant(int r, int c)
        {
            if (r < 3 && c < 3) return Quadrant.Topleft;

            if (r < 3 && c > 2) return Quadrant.Topright;

            if (r > 2 && c < 3) return Quadrant.Botleft;

            if (r > 2 && c > 2) return Quadrant.Botright;

            return Quadrant.None;
        }

        public Tuple<int, int, Quadrant, Direction> GetBestMove(int depth)
        {
            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();

            Tuple<int, int, Quadrant, Direction> bestMove = null;

            Array.Copy(GameBoard, GameBoardCopy, GameBoard.Length);

            int bestValue = int.MinValue;

            foreach (var move in moves)
            {
                SimulateMove(move);

                int moveValue = Minimieren(depth - 1, int.MinValue, int.MaxValue);

                UndoMove(move);


                if (moveValue > bestValue)
                {
                    bestMove = move;
                    bestValue = moveValue;
                }
            }

            return bestMove;
        }

        private void UndoMove(Tuple<int, int, Quadrant, Direction> move)
        {
            GameBoardCopy[move.Item1, move.Item2] = Player.None;
            SimulateRotation(GameBoardCopy,move.Item3, OppositeDirection(move.Item4));
        }

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

        private int Maximieren(int depth, int alpha, int beta)
        {
            int score = EvaluateBoard(GameBoardCopy);
            if (depth == 0 || score == int.MaxValue || score == int.MinValue)
                return score;

            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();
            int maxEval = int.MinValue;

            foreach (var move in moves)
            {
                SimulateMove(move);
                int eval = Minimieren(depth - 1, alpha, beta);
                UndoMove(move);
                maxEval = Math.Max(maxEval, eval);
                alpha = Math.Max(alpha, eval);
                if (beta <= alpha)
                    break;
            }
            return maxEval;
        }

        private int Minimieren(int depth, int alpha, int beta)
        {
            int score = EvaluateBoard(GameBoardCopy);
            if (depth == 0 || score == int.MaxValue || score == int.MinValue)
                return score;

            List<Tuple<int, int, Quadrant, Direction>> moves = GetAllMoves();
            int minEval = int.MaxValue;

            foreach (var move in moves)
            {
                SimulateMove(move);
                int eval = Maximieren(depth - 1, alpha, beta);
                UndoMove(move);
                minEval = Math.Min(minEval, eval);
                beta = Math.Min(beta, eval);
                if (beta <= alpha)
                    break;
            }
            return minEval;
        }

        private void SimulateMove(Tuple<int, int, Quadrant, Direction> move)
        {
            GameBoardCopy[move.Item1, move.Item2] = CurrentPlayer;
            SimulateRotation(GameBoardCopy ,move.Item3, move.Item4);
        }

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

        private int EvaluateBoard(Player[,] board)
        {
            int score = 0;
            int rows = board.GetLength(0);
            int cols = board.GetLength(1);

            // Check rows, columns, and diagonals
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    if (c <= cols - 5)
                    {
                        score += EvaluateLine(board, r, c, 0, 1); // Check rows
                    }
                    if (r <= rows - 5)
                    {
                        score += EvaluateLine(board, r, c, 1, 0); // Check columns
                    }
                    if (r <= rows - 5 && c <= cols - 5)
                    {
                        score += EvaluateLine(board, r, c, 1, 1); // Check diagonals (top-left to bottom-right)
                    }
                    if (r >= 4 && c <= cols - 5)
                    {
                        score += EvaluateLine(board, r, c, -1, 1); // Check diagonals (bottom-left to top-right)
                    }
                }
            }
            score += EvaluateCentralControl(board);
            return score;
        }
        private int EvaluateCentralControl(Player[,] board)
        {
            int centralControlScore = 0;
            // Assuming central positions are more valuable
            (int,int)[] centralPositions = { ( 2, 2 ), ( 2, 3 ), ( 3, 2 ), ( 3, 3 ) };

            foreach (var pos in centralPositions)
            {
                
                if (board[pos.Item1, pos.Item1] == Player.Red)
                {
                    centralControlScore += 3; // Increase value for controlling the center
                }
                else if (board[pos.Item1, pos.Item2] == Player.Blue)
                {
                    centralControlScore -= 3;
                }
            }

            return centralControlScore;
        }

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

            // Gewinnszenarien
            if (bluePoints == 5)
            {
                return int.MinValue; // Blue gewinnt
            }
            if (redPoints == 5)
            {
                return int.MaxValue; // Red gewinnt
            }

            // Bewertung für Blue
            int blueScore = 0;
            switch (bluePoints)
            {
                case 4:
                    blueScore = -100; // Sehr nah am Gewinnen
                    break;
                case 3:
                    blueScore = -10; // Gute Position
                    break;
                case 2:
                    blueScore = -1; // Anfang eines möglichen Zuges
                    break;
            }

            // Bewertung für Red
            int redScore = 0;
            switch (redPoints)
            {
                case 4:
                    redScore = 100; // Sehr nah am Gewinnen
                    break;
                case 3:
                    redScore = 10; // Gute Position
                    break;
                case 2:
                    redScore = 1; // Anfang eines möglichen Zuges
                    break;
            }

            // Balancierte Bewertung
            return blueScore + redScore;
        }

    
        #endregion


    }
}
    


