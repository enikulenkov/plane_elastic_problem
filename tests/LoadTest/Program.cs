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
            CommonData data = new CommonData();
            foreach (string f in Directory.GetFiles("..\\..\\..\\..\\samples"))
            {
                data.Load(f);
                Console.WriteLine("File {0} was read successfully", f);
            }
        }
    }
}
