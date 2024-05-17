using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tEST
{
    class Program
    {

        static void Main(string[] args)
        {
           GameState state = new GameState();
           
            for (int r = 0; r < state.GameGrid.Length ; r++)
            {
                for (int c = 0;c< state.GameGrid.Length ; c++)
                {
                    if (state.CkeckWin(r, c))
                    {
                        break;
                    }
                }
            }

           
            
        }
    }
}
