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
            logik.PrintArray();
            logik.SetPoint(0,0);
            logik.SetPoint(1, 0);
            logik.SetPoint(2, 0);
            logik.SetPoint(3, 0);
            logik.SetPoint(4, 0);
            

            Console.WriteLine(logik.Auswertung(logik.CurrentPlayer));

            Console.ReadLine();
        }
    }
}
