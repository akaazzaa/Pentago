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

        public int Minimax(int depth, bool isMaximizingPlayer)
        {
            if (depth == 0)
            {
                return EvaluateBoard();
            }

            List<Tuple<int, int, Corner, Direction>> allMoves = GetAllMoves();
            int bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

            foreach (var move in allMoves)
            {
                int row = move.Item1;
                int col = move.Item2;
                Corner corner = move.Item3;
                Direction direction = move.Item4;

                SimulateMove(row, col, corner, direction);
                int score = Minimax(depth - 1, !isMaximizingPlayer);
                DeletMove(row, col, corner, direction);

                bestScore = isMaximizingPlayer ? Math.Max(bestScore, score) : Math.Min(bestScore, score);
            }

            return bestScore;
        }
        private int EvaluateBoard()
        {
            throw new NotImplementedException();
        }
        private List<Tuple<int, int, Corner, Direction>> GetAllMoves()
        {
            List<Tuple<int, int, Corner, Direction>> moves = new List<Tuple<int, int, Corner, Direction>>();

            for (int r = 0; r < GameBoard.GetLength(0); r++)
            {
                for (int c = 0; c < GameBoard.GetLength(1); c++)
                {
                    if (GameBoard[r, c] == Player.None)
                    {
                        for (int quadrant = 0; quadrant < 4; quadrant++)
                        {
                            moves.Add(Tuple.Create(r, c, (Corner)quadrant, Direction.Right));
                            moves.Add(Tuple.Create(r, c, (Corner)quadrant, Direction.Left));
                        }
                    }
                }
            }
            return moves;
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
        public Player[,] SimulateRotation(Corner corner, Direction direction)
        {
            int rowStart, colStart;
            switch (corner)
            {
                case Corner.Topleft:
                    rowStart = 0;
                    colStart = 0;
                    break;
                case Corner.Topright:
                    rowStart = 0;
                    colStart = 3;
                    break;
                case Corner.Botleft:
                    rowStart = 3;
                    colStart = 0;
                    break;
                case Corner.Botright:
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

            return GameBoardCopy;
        }
        private void DeletMove(int row, int col, Corner corner, Direction direction)
        {
            GameBoardCopy[row, col] = Player.None;

            // Drehe den Quadranten in die entgegengesetzte Richtung zurück
            SimulateRotation(corner, direction == Direction.Right ? Direction.Left : Direction.Right);
        }
        private void SimulateMove(int row, int col, Corner corner, Direction direction)
        {
            GameBoardCopy[row, col] = CurrentPlayer;

            SimulateRotation(corner, direction);

        }

    }
}
