using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace tEST
{
    class Program
    {

        static void Main(string[] args)
        {
            CustomGame game = new CustomGame();
            game.Max(game.Suchtiefe); // Start the Minimax algorithm

            // Output the best move or other debugging info
            Console.WriteLine("Best move found:");
            void PrintBoard(Player[,] board)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Console.Write(board[i, j] + " ");
                    }
                    Console.WriteLine();
                }
            }

            PrintBoard(game.bestmove.Item1);
            PrintBoard(game.bestmove.Item2);
            PrintBoard(game.bestmove.Item3);
            PrintBoard(game.bestmove.Item4);

            Console.ReadLine();

        }
    }
}
