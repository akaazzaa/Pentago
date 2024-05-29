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

        public List<Player[,]> AllPossibleMoves(Player[,] topleft, Player[,] topright, Player[,] botleft, Player[,] botright, Player currentPlayer)
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
                        newgrid[i, j] = currentPlayer;
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
    }
}

