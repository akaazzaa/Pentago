using System;
using System.Collections.Generic;
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
        ComputerGegner Computer { get; set; }

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
        public Player[,] RotateField(Quadrant corner, Direction direction)
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

            return GameBoard;
        }
        #endregion

        #region WinCkeck
        private bool CheckDiagonal(int row, int col)
        {
            WinCondition = 0;
            for (int r = row; r < GameBoard.GetLength(0);)
            {
                for (int c = col; c < GameBoard.GetLength(1);)
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
            for (int r = row; r < GameBoard.GetLength(0);)
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
        private bool CheckRow()
        {
            WinCondition = 0;
            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
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
                WinCondition = 0;
            }
            return false;
        }
        private bool CheckColumn()
        {
            WinCondition = 0;
            for (int c = 0; c < GameBoard.GetLength(0); c++)
            {
                for (int r = 0; r < GameBoard.GetLength(1); r++)
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
        private bool IsDiagonalWin()
        {
            for (int row = 0; row < GameBoard.GetLength(0) / 2 ; row++)
            {
                for (int col = 0; col < GameBoard.GetLength(1) / 2; col++)
                {
                    if (IsMarked(row, col))
                    {
                        return CheckDiagonal(row, col);
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            for (int row = 0; row < GameBoard.GetLength(0) / 2 ; row++)
            {
                for (int col = 5; col > GameBoard.GetLength(1) / 2 ; col--)
                {
                    if (IsMarked(row, col))
                    {
                        return CheckAntiDiagonal(row, col);
                    }
                    else
                    {
                        continue;
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
            return TurnsPassed == 36;
        }
        public bool IsWin()
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
            }
            else if (IsGridFull())
            {
                GameResult = new GameResult { Winner = Player.None };
                return true;
            }

            GameResult = null;
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
            if (!CanMove(row, col))
            {
                return;
            }
            SetPoint(row,col);
            if (!IsWin())
            {
                RotateField(corner, direction);
            }
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
            if (score == int.MaxValue || score == int.MinValue || depth == 0)
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
            if (score == int.MaxValue || score == int.MinValue || depth == 0)
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

            // Check rows, columns, and diagonals for the current state
            score += EvaluateAllLines(board);

            // Check all possible rotations
            for (int quadrant = 0; quadrant < 4; quadrant++)
            {
                Player[,] tempBoard = (Player[,])board.Clone();

                // Rotate right
                SimulateRotation(tempBoard, (Quadrant)quadrant, Direction.Right);
                score += EvaluateAllLines(tempBoard);

                // Rotate left
                SimulateRotation(tempBoard, (Quadrant)quadrant, Direction.Left);
                score += EvaluateAllLines(tempBoard);
            }

            return score;
        }

        private int EvaluateAllLines(Player[,] board)
        {
            int score = 0;

            // Check rows
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1) - 4; c++)
                {
                    score += EvaluateLine(board, r, c, 0, 1);
                }
            }

            // Check columns
            for (int c = 0; c < board.GetLength(1); c++)
            {
                for (int r = 0; r < board.GetLength(0) - 4; r++)
                {
                    score += EvaluateLine(board, r, c, 1, 0);
                }
            }

            // Check diagonals (top-left to bottom-right)
            for (int r = 0; r < board.GetLength(0) - 4; r++)
            {
                for (int c = 0; c < board.GetLength(1) - 4; c++)
                {
                    score += EvaluateLine(board, r, c, 1, 1);
                }
            }

            // Check diagonals (bottom-left to top-right)
            for (int r = 4; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1) - 4; c++)
                {
                    score += EvaluateLine(board, r, c, -1, 1);
                }
            }

            return score;
        }

        private int EvaluateLine(Player[,] board, int startRow, int startCol, int rowDir, int colDir)
        {
            int player1Count = 0;
            int player2Count = 0;

            for (int i = 0; i < 5; i++)
            {
                Player cell = board[startRow + i * rowDir, startCol + i * colDir];
                if (cell == Player.Blue)
                {
                    player1Count++;
                }
                else if (cell == Player.Red)
                {
                    player2Count++;
                }
            }

            if (player1Count == 5)
            {
                return int.MaxValue;
            }
            else if (player2Count == 5)
            {
                return int.MinValue;
            }
            else if (player1Count > 0 && player2Count == 0)
            {
                return player1Count;
            }
            else if (player2Count > 0 && player1Count == 0)
            {
                return -player2Count;
            }
            else
            {
                return 0;
            }
        }
        #endregion


    }
}


