namespace Pentago
{
    public class Grids
    {
        public static int[,] grid1 = new int[3,3]
        {
            {1,2,3},
            {4,5,6},
            {7,8,9}
        };
        public static int[,] grid2 = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };
        public static int[,] grid3 = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };
        public static int[,] grid4 = new int[3, 3]
       {
            {0,0,0},
            {0,0,0},
            {0,0,0}
       };


        public static int[,] RotateArrayRight(int[,] zudrehendesarray, int arryGroeße)
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
        public static int[,] RotateArrayLeft(int[,] zudrehendesarray, int arryGroeße)
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


    }
}
