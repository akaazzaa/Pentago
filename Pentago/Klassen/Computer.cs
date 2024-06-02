using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentago.Klassen
{

    public class Computer
    {

        public Computer() { }

        private List<Player[,]> AllPossibleMoves(Player[,] topleft, Player[,] topright, Player[,] botleft, Player[,] botright)
        {
            Player[,] board = GetGameArray(topleft,topright,botleft,botright);

            List<Player[,]> moves = new List<Player[,]>() ;

            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == Player.None)
                    {
                        Player[,] newgrid = (Player[,])board.Clone();
                        newgrid[i, j] = Player.Red;
                        moves.Add(newgrid);
                    }
                }
            }
            return moves;
        }

        private Player[,] GetGameArray(Player[,] topleft, Player[,] topright, Player[,] botleft, Player[,] botright)
        {

            Player[,] board = new Player[6, 6];
            for (int r = 0; r < board.GetLength(0); r++)
            {
                for (int c = 0; c < board.GetLength(1); c++)
                {
                    if (r < 3 && c < 3)
                    {
                         board[r, c] = topleft[r, c];
                    }

                    else if (r < 3 && c > 3 - 1 && c < 6)
                    {
                         board[r, c] = topright[r, c - 3];
                    }


                    if (r >= 3 && c < 3)
                    {
                        board[r, c] = botleft[r - 3, c];
                    }
                    if(r > 3 && r < 6 && c > 3 && c < 6)
                    {
                       board[r,c] = botright[r - 3, c - 3];
                    }
                }
            }

            return board;
        }

         public int  EvaluateBoard(Player[,] topleft, Player[,] topright, Player[,] botleft, Player[,] botright)
        {
            int score = 0;
            // Gewichtungen für die verschiedenen Szenarien
            int[] weights = { 0, 1, 10, 100, 1000, 10000 }; // Gewichtungen für 0, 1, 2, 3, 4, 5 in einer Reihe

            foreach (var board in AllPossibleMoves( topleft,topright,botleft,botright))
            {
                for (int i = 0; i < 6; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        // Horizontale Reihen
                        if (j <= 2) score += EvaluateLine(board, i, j, 0, 1, weights); // von (i, j) nach rechts
                                                                                       // Vertikale Reihen
                        if (i <= 2) score += EvaluateLine(board, i, j, 1, 0, weights); // von (i, j) nach unten
                                                                                       // Diagonale Reihen (rechts unten)
                        if (i <= 2 && j <= 2) score += EvaluateLine(board, i, j, 1, 1, weights); // von (i, j) nach rechts unten
                                                                                                 // Diagonale Reihen (rechts oben)
                        if (i >= 3 && j <= 2) score += EvaluateLine(board, i, j, -1, 1, weights); // von (i, j) nach rechts oben
                    }
                }
            }
            

            return score;
        }

        static int EvaluateLine(Player[,] board, int startX, int startY, int stepX, int stepY, int[] weights)
        {
            int player1Count = 0;
            int player2Count = 0;

            for (int k = 0; k < 5; k++)
            {
                int x = startX + k * stepX;
                int y = startY + k * stepY;

                if (board[x, y] == Player.Blue) player1Count++;
                if (board[x, y] == Player.Red) player2Count++;
            }

            if (player1Count > 0 && player2Count > 0) return 0; // Blockierte Reihe
            if (player1Count > 0) return weights[player1Count]; // Vorteil für Spieler 1
            if (player2Count > 0) return -weights[player2Count]; // Vorteil für Spieler 2

            return 0;
        }
    }
}

