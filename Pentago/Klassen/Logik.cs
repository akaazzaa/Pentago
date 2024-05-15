using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Pentago.Klassen
{
    public class Logik
    {

         
        public int[,] arrayTopLeft = new int[3, 3]
        {
            {0,0,0},
            {0,0,0},
            {0,0,0}
        };
        public  int[,] arrayTopRight = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };
        public  int[,] arrayBotLeft = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };
        public  int[,] arrayBotRight = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };

        public Logik() { }

        public int[,] RotatetArrayRight(int[,] zudrehendesarray, int arryGroeße)
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
        public  int[,] RotatetArrayLeft(int[,] zudrehendesarray, int arryGroeße)
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
            switch (grid.Name)
            {
                case "GridTopL":
                    arrayTopLeft[i, j] = 1;
                    break;
                    case "GridTopR":
                    arrayTopRight[i, j] = 1;
                    break;
                    case "GridBotL":
                    arrayBotLeft[i, j] = 1;
                    break;
                    case "GridBotR":
                    arrayBotRight[i, j] = 1;
                    break;
            }
        }
    }
}
