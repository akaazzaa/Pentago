using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Pentago;

namespace tEST
{
    class Program
    {

        static void Main(string[] args)
        {
           Logik logik = new Logik();

            logik.SetPoint(0, 2);
            logik.SetPoint(0, 3);

            logik.PrintArray();
            logik.RotateField(Corner.Topright, Direction.Right);
            Console.WriteLine("");
            logik.PrintArray();

                Console.ReadLine();
        }
    }
}
