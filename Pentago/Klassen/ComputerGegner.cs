using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago.Klassen
{
    public class ComputerGegner
    {
        private Player[,] GameBoard {  get; set; }
        private Player[,] GameBoardCopy { get; set; }
        public Player CurrentPlayer { get; set; }

        public ComputerGegner()
        {
            GameBoard = new Player[6,6];
            GameBoardCopy = new Player[6,6];
            CurrentPlayer = Player.Blue;
        }
  
     
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

            int bestValue = int.MinValue;

            Array.Copy(GameBoard, GameBoardCopy, GameBoard.Length);

            foreach (var move in moves)
            {
                SimulateMove(move);

                int moveValue = Minimieren( depth - 1, int.MinValue, int.MaxValue);

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
            GameBoard[move.Item1, move.Item2] = Player.None;
            SimulateRotation(move.Item3, OppositeDirection(move.Item4));
        }

        private Direction OppositeDirection(Direction direction)
        {
           if (direction == Direction.Left)
            {
                return direction = Direction.Right;
            }
           else
            {
               return  direction = Direction.Left;
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

        private void SimulateMove(Tuple<int,int,Quadrant,Direction> move)
        {
            GameBoardCopy[move.Item1, move.Item2] = CurrentPlayer;
            SimulateRotation(move.Item3, move.Item4);
        }

        private void SimulateRotation(Quadrant quadrant, Direction direction)
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
                    tmp[r, c] = GameBoardCopy[rowStart + r, colStart + c];
                }
            }


            Player[,] rotatedarray = Rotate3x3(tmp, direction);


            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    GameBoardCopy[rowStart + r, colStart + c] = rotatedarray[r, c];
                }
            }

            
        }

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

        public int EvaluateBoard(Player[,] board)
        {
            int score = 0;

            // Check rows
           

            // Check columns
          

            // Check diagonals (top-left to bottom-right)
            

            // Check diagonals (bottom-left to top-right)
          

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




    }
}
