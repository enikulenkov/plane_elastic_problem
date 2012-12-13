using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    /* Предположение: ленточная матрица всегда квадратная */
    public class BandMatrix
    {
        public BandMatrix(){}
        public BandMatrix(int[][] m)
        {
            
        }

        /* Возвращает по порядку ненулевые элементы для строки с номером strIndex */
        public System.Collections.IEnumerable StrElements(int strIndex)
        {
            yield return 0;
        }

        public void setElement(int i, int j, double val)
        {
        }

        public int Length
        {
            get { return 0; }
            set { }
        }
    }
}