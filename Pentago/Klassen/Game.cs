using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Pentago.Klassen
{
    public class Game
    {  
       public Player currentplayer;
        
        public int[,] arrayTopLeft {  get; set; }
        public int[,] arrayTopRight {  get; set; }
        public int[,] arrayBotLeft {  get; set; }
        public int[,] arrayBotRight {  get; set; }
      
        public Game()
        {
            arrayTopLeft = new int[3, 3];
            arrayTopRight = new int[3, 3];
            arrayBotLeft = new int[3, 3];
            arrayBotRight = new int[3, 3];
            currentplayer = Player.Blue;
            
        }

        private int[,] RotatetArrayRight(int[,] zudrehendesarray, int arryGroeße)
        {
            int[,] ret = new int[arryGroeße, arryGroeße];

            for (int i = 0; i < arryGroeße; ++i)
            {
                for (int j = 0; j < arryGroeße; ++j)
                {
                    ret[i, j] = zudrehendesarray[arryGroeße - j - 1, i];
                }
            }

            return ret;
        }
        private int[,] RotatetArrayLeft(int[,] zudrehendesarray, int arryGroeße)
        {
            int[,] ret = new int[arryGroeße, arryGroeße];

            for (int i = 0; i < arryGroeße; ++i)
            {
                for (int j = 0; j < arryGroeße; ++j)
                {
                    ret[i, j] = zudrehendesarray[j, arryGroeße - i - 1];
                }
            }

            return ret;
        }
        public void SetPressedStone(Grid grid, int i, int j)
        {
            switch (grid.Uid)
            {
                case "1":
                    arrayTopLeft[i, j] = 1;
                    break;
                case "2":
                    arrayTopRight[i, j] = 1;
                    break;
                case "3":
                    arrayBotLeft[i, j] = 1;
                    break;
                case "4":
                    arrayBotRight[i, j] = 1;
                    break;
            }
        }

        
        


       
    }
}
