using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago.Klassen
{
    public class Pentagoai
    {
       
        
            private Random rand = new Random();

            public Tuple<ComputerMark, ComputerRotation> GetNextMove(Player[,] board, Player player)
            {
                List<Tuple<ComputerMark, ComputerRotation>> possibleMoves = new List<Tuple<ComputerMark, ComputerRotation>>();

                for (int row = 0; row < 6; row++)
                {
                    for (int col = 0; col < 6; col++)
                    {
                        if (board[row, col] == Player.None)
                        {
                            for (int subBoard = 0; subBoard < 4; subBoard++)
                            {
                                foreach (bool clockwise in new bool[] { true, false })
                                {
                                    possibleMoves.Add(new Tuple<ComputerMark, ComputerRotation>(
                                        new ComputerMark(-1, -1, row, col),
                                        new ComputerRotation(board, Direction.Right)));
                                }
                            }
                        }
                    }
                }

                // Bewerten Sie jeden Zug und wählen Sie den besten aus
                Tuple<ComputerMark, ComputerRotation> bestMove = null;
                int bestScore = int.MinValue;

                foreach (var move in possibleMoves)
                {
                    Player[,] newBoard = board.Clone();
                    newBoard.MakeMove(move.Item1.toRow, move.Item1.toCol, player);
                    newBoard.RotateSubBoard(move.Item2.board, move.Item2.Clockwise);

                    int score = EvaluateBoard(newBoard, player);

                    if (score > bestScore)
                    {
                        bestScore = score;
                        bestMove = move;
                    }
                }

                return bestMove;
            }

            private int EvaluateBoard(PentagoBoard board, Player player)
            {
                // Einfache Heuristik: Anzahl der Reihen, Spalten oder Diagonalen mit drei oder mehr Murmeln des Spielers
                int score = 0;

                // Überprüfen Sie Reihen und Spalten
                for (int i = 0; i < 6; i++)
                {
                    score += EvaluateLine(board, player, i, 0, 0, 1); // Reihe
                    score += EvaluateLine(board, player, 0, i, 1, 0); // Spalte
                }

                // Überprüfen Sie Diagonalen
                score += EvaluateLine(board, player, 0, 0, 1, 1); // Hauptdiagonale
                score += EvaluateLine(board, player, 0, 5, 1, -1); // Gegendiagonale

                return score;
            }

            private int EvaluateLine(PentagoBoard board, Player player, int startRow, int startCol, int rowStep, int colStep)
            {
                int count = 0;
                int playerCount = 0;

                for (int i = 0; i < 6; i++)
                {
                    int row = startRow + i * rowStep;
                    int col = startCol + i * colStep;

                    if (board.GetPlayerAt(row, col) == player)
                    {
                        playerCount++;
                    }
                    else
                    {
                        playerCount = 0;
                    }

                    if (playerCount >= 3)
                    {
                        count++;
                    }
                }

                return count;
            }
        }
 }

