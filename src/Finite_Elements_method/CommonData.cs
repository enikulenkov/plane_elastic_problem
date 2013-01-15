using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Finite_Elements_method
{
    public class CommonData
    {
        /********************** Private members ***************************/

        private int _NTotalNodes;        /* кол-во узлов */
        private int _NExternalNodes;     /* кол-во узлов во внешней границе */
        private int _NInternalNodes;     /* кол-во узлов во внутренней границе */
        private int _NTriangles;         /* число треугольников */
        private int _NBndConditions;     /* кол-во граничных условий */

        private int[][] _M;            //матрица смежности
        private int[] _BoundExternal;  //номера узлов, образующих внешнюю границу
        private int[] _BoundInternal;  //номера узлов, образующих внутреннюю границу
        private Triangle[] _Triangles; //треугольники
        //private double[] _u;           //вектор перемещений
        private Point[] _Coords;       //массив координат вершин
        private double _E, _v;          //параметры материала
        private int[] _changedPoints; //Список изменённых вершин на форме 
        /************************* Methods ********************************/
        public void Load(String fileName)
        {
            List<string> inputLines = new List<string>();

            /* Считывание входных параметров из файла в локальный буфер.
               Фильтрация строк не содержащих полезные данные */
            using (StreamReader streamReader = new StreamReader(fileName))
            {
                string curLine;
                while (!streamReader.EndOfStream)
                {
                    curLine = streamReader.ReadLine().Trim();
                    if ((curLine.StartsWith("#")) || curLine.Equals(""))
                    {
                        /* Строки без данных */
                    }
                    else
                    {
                        inputLines.Add(curLine);
                    }
                }
            }

            int lineIdx = 0;
            /*  Считывание данных в поля класса */
            int adjValue;
            this._NTotalNodes = int.Parse(inputLines[lineIdx++]);
            this._M = new int[this._NTotalNodes][];
            for (int i = 0; i < this._NTotalNodes; i++)
            {
                M[i] = new int[this._NTotalNodes];
                for (int j = 0; j < this._NTotalNodes; j++)
                {
                    /* предполагаем, что элементы матрицы смежности разделены одним пробелом
                     * и возможные значения матрицы 1 или 0 */
                    adjValue = int.Parse((inputLines[lineIdx + i][2 * j]).ToString());
                    this._M[i][j] = adjValue;
                }
            }
            lineIdx += this.NTotalNodes;
            
            string[] coordPair;
            double x, y;
            this._Coords = new Point[this._NTotalNodes];
            for (int i = 0; i < this._NTotalNodes; i++)
            {
                /* Координаты вершин разделены одним пробелом */
                coordPair = inputLines[lineIdx+i].Split(' ');
                x = double.Parse(coordPair[0]);
                y = double.Parse(coordPair[1]);
                Coords[i] = new Point(x, y);
            }
            lineIdx += this._NTotalNodes;

            /*Считывание внешней границы*/
            this._NExternalNodes = int.Parse(inputLines[lineIdx++]);
            this._BoundExternal = new int[this._NExternalNodes];
            if (this._NExternalNodes > 0)
            {
                string[] extBoundPoints = inputLines[lineIdx++].Split(' ');
                for (int i = 0; i < this._NExternalNodes; i++)
                {
                    this._BoundExternal[i] = int.Parse(extBoundPoints[i]);
                }
            }

            /*Считывание внутренних границ*/
            this._NInternalNodes = int.Parse(inputLines[lineIdx++]);
            this._BoundInternal = new int[this._NInternalNodes];
            if (this._NInternalNodes > 0)
            {
                string[] innerBoundPoints = inputLines[lineIdx++].Split(' ');
                for (int i = 0; i < this._NInternalNodes; i++)
                {
                    this._BoundInternal[i] = int.Parse(innerBoundPoints[i]);
                }
            }

            /*Считывание граничных условий*/
            this._NBndConditions = int.Parse(inputLines[lineIdx++]);
            string[] bndCondition;
            int vertexNo;
            double dx, dy;
            this._changedPoints = new int[this._NBndConditions];
            for (int i = 0; i < this._NBndConditions; i++)
            {
                bndCondition = inputLines[lineIdx + i].Split(' ');
                vertexNo = int.Parse(bndCondition[0]);
                dx = double.Parse(bndCondition[1]);
                dy = double.Parse(bndCondition[2]);
                if ((dx == 0)) dx = 1e-10;
                if (dy == 0) dy = 1e-10;

                this._Coords[vertexNo].Dx = dx;
                this._Coords[vertexNo].Dy = dy;
                _changedPoints[i] = vertexNo;
            }
            lineIdx += this._NBndConditions;
            
            this._E = double.Parse(inputLines[lineIdx++]);
            this._v = double.Parse(inputLines[lineIdx++]);
        }

        /********************* Properties *********************/
        public int NTotalNodes
        {
            get { return _NTotalNodes; }
        }

        public int NExternalNodes
        {
            get { return _NExternalNodes; }
        }

        public int NInternalNodes
        {
            get { return _NInternalNodes; }
        }

        public int NTriangles
        {
            get { return _NTriangles; }
        }

        public int NBndConditions
        {
            get { return _NBndConditions; }
            set { _NBndConditions = value; }
        }

        public int[][] M
        {
            get { return _M; }
            set { _M = value; }
        }

        public int[] BoundExternal
        {
            get { return _BoundExternal; }
            set { _BoundExternal = value; }
        }

        public int[] BoundInternal
        {
            get { return _BoundInternal; }
            set { _BoundInternal = value; }
        }

        public Triangle[] Triangles
        {
            get { return _Triangles; }
        }

        public double[] u
        {
            get 
            {
                double[] res = new double[NTotalNodes*2];
                for (int i = 0; i < NTotalNodes * 2; i += 2)
                {
                    res[i] = this._Coords[i / 2].Dx;
                    res[i + 1] = this._Coords[i / 2].Dy;
                }

                return res;
            }
        }

        public Point[] Coords
        {
            get { return _Coords; }
            set { _Coords = value; }
        }

        public double E
        {
            get {return _E; }
        }

        public double v
        {
            get { return _v; }
        }

        public void ChangedPointsClear() 
        {
            _changedPoints = new int[0];
        }

        public void SetChangedPoints(int[] value, bool afterCM) 
        {
            if (_changedPoints != null)
            {
                List<int> l = new List<int>(_changedPoints);
                if (afterCM)
                {
                    _changedPoints = value;
                }
                else
                {
                    l.AddRange(value);
                    _changedPoints = l.ToArray();
                }
            }
            else 
            {
                _changedPoints = value;
            }

        }

        public int[] ChangedPoints 
        {
            get { return _changedPoints; }
        }
    }
}
