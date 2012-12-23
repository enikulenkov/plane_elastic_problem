using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Finite_Elements_method;

namespace MathTests
{
    class SolveTestExecutor
    {
        private const string test_samples_dir = "..\\..\\..\\..\\samples\\math_tests\\";
        private string[] input_files = new string[]
        {
            "integration1.txt",
        };

        private double[][] input_u_vectors = new double[][]
        {
            new double[] {0.15, 0.45, 0, 0, 0, 0.31, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        };

        private double[][] expected_output_u_vectors = new double[][]
        {
            new double[] {0.15, 0.45, 0.15, 0.38, 0.15, 0.31, 0.15, 
                            0.24, 0.22, 0.45, 0.22, 0.38, 0.22, 0.31, 
                            0.22, 0.24, 0.29, 0.45, 0.29, 0.38, 0.29, 
                            0.31, 0.29, 0.24, 0.36, 0.45, 0.36, 0.38, 
                            0.36, 0.31, 0.36, 0.24},
        };

        public SolveTestExecutor()
        {
        }

        public void ExecuteTests()
        {
            int numberOfTests = input_files.Length;
            double[] actual_v;
            bool testSuccessful;

            for (int i = 0; i < numberOfTests; i++)
            {
                Console.WriteLine("=====Start solver test №{0}=====", i);
                actual_v = RunSolverTest(input_files[i], input_u_vectors[i]);
                testSuccessful = CheckTestResult(actual_v, expected_output_u_vectors[i]);
                if (testSuccessful)
                {
                    Console.WriteLine("Solver test №{0} passed", i);
                }
                else
                {
                    Console.WriteLine("Solver test №{0} failed", i);
                }
                Console.WriteLine();
            }
        }

        private bool DoublesAreEqual(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.0001;
        }

        private bool CheckTestResult(double[] actual_v, double[] expected_v)
        {
            bool res = true;

            if (actual_v == null)
            {
                return (expected_v == null);
            }
            
            if (actual_v.Length != expected_v.Length)
            {
                return false;
            }

            for (int i = 0; i < expected_v.Length; i++)
            {
                res = res && DoublesAreEqual(actual_v[i], expected_v[i]);
            }

            return res;
        }

        private double[] RunSolverTest(string f, double[] u)
        {
            Console.WriteLine("Loading data from {0}...", f);

            try
            {
                CommonData data = new CommonData();
                data.Load(test_samples_dir + f);
                for (int i = 0; i < u.Length; i++)
                {
                    data.u[i] = u[i];
                }
                Console.WriteLine("Data is loaded");
                Solver s = new Solver();
                s.Solve(data);
                return data.u;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
