using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Finite_Elements_method;

namespace LoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            bool testSuccessfull = true;

            CommonData data = new CommonData();
            foreach (string f in Directory.GetFiles("..\\..\\..\\..\\samples"))
            {
                try
                {
                    data.Load(f);
                    Console.WriteLine("File {0} was read successfully", f);
                }
                catch (Exception e)
                {
                    testSuccessfull = false;
                    Console.WriteLine("Error during reading file {0}!!!", f);
                    Console.WriteLine(e.ToString());
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            Console.WriteLine();
            if (testSuccessfull)
            {
                Console.WriteLine("Test has finished successfully. Press Enter to continue...");
            }
            else
            {
                Console.WriteLine("Test has failed. Press Enter to continue...");
            }
            Console.ReadLine();
        }
    }
}
