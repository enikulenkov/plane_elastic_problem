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
            "integration2.txt",
        };

        private double[][] input_u_vectors = new double[][]
        {
            new double[] {0.15, 0.45, 0, 0, 0, 0.31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},

            new double[] {0, 0, 0.8256, 0.04, 0, 0},
        };

        private double[][] expected_output_u_vectors = new double[][]
        {
            new double[] {0.15, 0.45, 0.15, 0.38, 0.15, 0.31, 0.15, 
                            0.24, 0.22, 0.45, 0.22, 0.38, 0.22, 0.31, 
                            0.22, 0.24, 0.29, 0.45, 0.29, 0.38, 0.29, 
                            0.31, 0.29, 0.24, 0.36, 0.45, 0.36, 0.38, 
                            0.36, 0.31, 0.36, 0.24},

            new double[] {1.016975, 0.040000, 0.825600, 0.040000, 0.729913, 
                          0.135687, 1.016975, 0.231375, 0.825600, 0.231375, 
                          0.921288, 0.327062, 0.634225, 0.040000, 0.442850, 
                          0.040000, 0.634225, 0.231375, 1.016975, 0.422750, 
                          1.016975, 0.614125, 0.825600, 0.422750, 0.442850, 
                          0.231375, 0.538538, 0.327062, 0.634225, 0.422750, 
                          0.825600, 0.614125, 0.729913, 0.518437, 0.442850, 
                          0.422750, 0.634225, 0.614125, 0.442850, 0.614125},
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
                for (int i = 0; i < u.Length; i+=2)
                {
                    data.Coords[i / 2].Dx = u[i];
                    data.Coords[i / 2].Dy = u[i+1];
                }
                Console.WriteLine("Data is loaded");
                Solver s = new Solver();
                return s.Solve(data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }
}
