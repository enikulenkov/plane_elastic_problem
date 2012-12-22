using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    public class Point
    {
        double _x, _y;
        double _dx, _dy;

        public Point(double x, double y)
        {
            _x = x;
            _y = y;
            _dx = 0;
            _dy = 0;
        }

        public double X
        {
            get { return _x; }
            set { _x = value; }
        }

        public double Y
        {
            get { return _y; }
            set { _y = value; }
        }

        public double Dx
        {
            get { return _dx; }
            set { _dx = value; }
        }

        public double Dy
        {
            get { return _dy; }
            set { _dy = value; }
        }
    }
}
