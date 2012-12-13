using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    static public class CommonData
    {
        /* N - кол-во узлов, 
         * Ne - кол-во узлов во внешней границе, 
         * Ni -  кол-во узлов во внутренней границе, 
         * Ntr - число треугольников
         */
        static int N, Ne, Ni, Ntr; 
        static int[][] M; //матрица смежности
        static int[] Be;  //внешняя граница
        static int[] Bi;  //внутренняя граница
        static Triangle[] Triangles; //треугольники
        static double[] u; //вектор перемещений
        static Point[] Coords; //массив координат вершин
        static double E,v; //параметры материала
        static double x0,x1,y0,y1; //min и max координаты области
 
        static public void Load(String FileName)
        {
        }
        //TODO: Описать свойства для private-членов
    }
}
