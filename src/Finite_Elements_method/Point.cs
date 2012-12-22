using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    class Point
    {
        double x, y;
        double dx, dy;

        public Point(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Dx
        {
            get { return Dx; }
            set { Dx = value; }
        }

        public double Dy
        {
            get { return Dy; }
            set { Dy = value; }
        }
    }
}
