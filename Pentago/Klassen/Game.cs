using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Pentago.Klassen
{
    public class Game
    {

        GameGrid GridTL;
        GameGrid GridTR;
        GameGrid GridBL;
        GameGrid GridBR;
        bool isRunnig = false;
        Player player;
        Site site;

        public Game(Grid main) 
        {
           GameGrid gameGrid = new GameGrid();
            main.Children.Add(gameGrid.Grid);
        }

        internal void Run()
        {

            while (isRunnig)
            {


            }
           
        }
    }
}
