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
           Logik logik = new Logik();

            logik.SetPoint(0, 0);

            logik.Ausgabe();
            logik.board = logik.RotateArrayLeft();
            Console.WriteLine("");
            logik.Ausgabe();

                Console.ReadLine();
        }
    }
}
