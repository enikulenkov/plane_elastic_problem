using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    public class Solver
    {

        public struct delta
        {
            double dx;
            double dy;
        }

        /* Определить ширину ленты матрицы смежности */
        int getBandWidthOfConnectivityMatrix(int[][] connMatrix)
        {
            return 0;
        }

        /* Алгоритм Кахилла-Маки перенумерации вершин */
        int[][] CuthillMcKee(int[][] connMatrix)
        {
            return null;
        }

        double[] getElementStiffnessMatrix(Triangle elem)
        {
            return null;
        }

        void addElementStiffnessMatrixToGlobal(double elemStiffMatrix)
        {
        }

        /* Приведение глобальной матрицы жесткости к LU виду */
        BandMatrix decomposeStiffnessMatrixToLU(BandMatrix stiffMatrix)
        {
            return null;
        }

        delta[] Choletsky(BandMatrix LU, double[] f)
        {
            return null;
        }

        public Solver()
        {
        }
        
        /* Основная процедура решения. Возвращает массив координат точек после деформации */
        public delta[] Solve()
        {
            /* Вызываем Катхилла-Макки для перенумерации */
            /* Инициализируем глобальную матрицу жесткости K */
            /* Для каждого элемента считаем матрицу жесткости с одновременным занесением в глобальную матрицу жесткости */
            /* Преобразовать K к L-U виду */
            /* Посчитать f с учетом граничных условий (CommonData.u) */
            /* Решаем методом Холецкого LU*x=f и возвращаем результат */
            return null;
        }
    }
}
