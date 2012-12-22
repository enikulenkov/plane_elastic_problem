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
        private double[] _u;           //вектор перемещений
        private Point[] _Coords;       //массив координат вершин
        private double _E, _v;          //параметры материала
        //double x0,x1,y0,y1;   //min и max координаты области
 
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
            int.TryParse(inputLines[lineIdx++], out this._NTotalNodes);
            this._M = new int[this._NTotalNodes][];
            for (int i = 0; i < this._NTotalNodes; i++)
            {
                M[i] = new int[this._NTotalNodes];
                for (int j = 0; j < this._NTotalNodes; j++)
                {
                    /* предполагаем, что элементы матрицы смежности разделены одним пробелом
                     * и возможные значения матрицы 1 или 0 */
                    int.TryParse((inputLines[lineIdx + i][2 * j]).ToString(), out adjValue);
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
                coordPair = inputLines[lineIdx].Split(' ');
                double.TryParse(coordPair[0], out x);
                double.TryParse(coordPair[1], out y);
                Coords[i] = new Point(x, y);
            }
            lineIdx += this._NTotalNodes;

            int.TryParse(inputLines[lineIdx++], out this._NExternalNodes);
            this._BoundExternal = new int[this._NExternalNodes];
            if (this._NExternalNodes > 0)
            {
                string[] extBoundPoints = inputLines[lineIdx++].Split(' ');
                for (int i = 0; i < this._NExternalNodes; i++)
                {
                    int.TryParse(extBoundPoints[i], out this._BoundExternal[i]);
                }
            }

            int.TryParse(inputLines[lineIdx++], out this._NInternalNodes);
            this._BoundInternal = new int[this._NInternalNodes];
            if (this._NInternalNodes > 0)
            {
                string[] innerBoundPoints = inputLines[lineIdx++].Split(' ');
                for (int i = 0; i < this._NInternalNodes; i++)
                {
                    int.TryParse(innerBoundPoints[i], out this._BoundInternal[i]);
                }
            }

            int.TryParse(inputLines[lineIdx++], out this._NBndConditions);
            string[] bndCondition;
            int vertexNo;
            double dx, dy;
            for (int i = 0; i < this._NBndConditions; i++)
            {
                bndCondition = inputLines[lineIdx + i].Split(' ');
                int.TryParse(bndCondition[0], out vertexNo);
                double.TryParse(bndCondition[1], out dx);
                this._Coords[vertexNo].Dx = dx;
                double.TryParse(bndCondition[2], out dy);
                this._Coords[vertexNo].Dy = dy;
            }
            lineIdx += this._NBndConditions;

            double.TryParse(inputLines[lineIdx++], out this._E);
            double.TryParse(inputLines[lineIdx++], out this._v);
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
        }

        public int[] BoundExternal
        {
            get { return _BoundExternal; }
        }

        public int[] BoundInternal
        {
            get { return _BoundInternal; }
        }

        public Triangle[] Triangles
        {
            get { return _Triangles; }
        }

        public double[] u
        {
            get { return _u; }
            set { _u = value; }
        }

        public Point[] Coords
        {
            get { return _Coords; }
        }

        public double E
        {
            get {return _E; }
        }

        public double v
        {
            get { return _v; }
        }
    }
}
