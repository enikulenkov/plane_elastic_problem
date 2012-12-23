using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finite_Elements_method;

namespace MathTests
{
    class Program
    {
        private void fill_u(double[] u)
        {
            u[0] = 0.15;
            u[1] = 0.45;
            u[5] = 0.31;
        }

        static void Main(string[] args)
        {
            SolveTestExecutor solveTestExecutor = new SolveTestExecutor();
            solveTestExecutor.ExecuteTests();
            Console.WriteLine("Tests are finished. Press Enter to continue...");
            Console.ReadLine();
        }
    }
}
