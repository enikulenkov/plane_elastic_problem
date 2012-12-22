using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Finite_Elements_method
{
    public class Triangle
    {
        Point i,j,k;

        /* Параметры задаются в порядке обхода треугольника против часовой стрелки */
        public Triangle(Point i, Point j, Point k)
        {
        }

        public Point I
        {
            get { return i; }
        }

        public Point J
        {
            get { return j; }
        }

        public Point K
        {
            get { return k; }
        }

        /* Написать итератор для обхода вершин против часовой стрелки */
    }
}
