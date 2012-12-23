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
        private int getBandWidthOfConnectivityMatrix(int[][] connMatrix)
        {
            int L = 0;
            int len = connMatrix.Length;
            for(int i=0;i<len;++i)
            {
                int j=0;
                int t1=0,t2=0;
                while(j<i&&connMatrix[i][j]==0) ++j;
                if(j!=i)t1=i-j+1;
                if(t1>L)L=t1;
                
                j=len-1;
                while(j>i&&connMatrix[i][j]==0) --j;
                if(j>i)t2=j-i+1;
                if(t2>L)L=t2;
    
            }
            
            return L*=2;
        }

        private int K0(int i, int L)
        {
	        return (i<L-1) ? 0:(i-L+1);
        }

        private int KN(int i, int L, int N)
        {
	        return (i<2*N-L) ? (i+L):(2*N);
        }

        //-------------------метод Халецкого-------------------------------------------
        private double[] Cholesky(double[][] A, int N, int L, double[] f)
        {
            double[] res = new double[N];

	        double[][] B, C;
	        B=new double[N][];
            C=new double[N][];
	        for(int i=0; i<N; ++i)
	        {
		        B[i]=new double[2*L];
		        C[i]=new double[2*L];
	        }

	        for(int i=0; i<N; ++i)
	        {
		        for(int j=i; j<KN(i, L, N/2); ++j)
		        {
			        double t=0;
			        for(int k=K0(j, L); k<j; ++k)
                    {
				        t+=B[j][k-j+L-1]*C[k][i-k+L-1];
                    }
			        B[j][i-j+L-1]=A[j][i-j+L-1]-t;
		        }

		        for(int j=i; j<KN(i, L, N/2); ++j)
		        {
			        double t=0;
			        for(int k=K0(j, L); k<i; ++k)
                    {
				        t+=B[i][k-i+L-1]*C[k][j-k+L-1];
                    }
			        C[i][j-i+L-1]=(A[i][j-i+L-1]-t)/B[i][L-1];
		        }
	        }

            double[] y = new double[N];

	        for(int i=0; i<N; ++i)
	        {
		        double t=0;
		        for(int k=K0(i, L); k<i; ++k)
                {
			        t+=B[i][k-i+L-1]*y[k];
                }
		        y[i]=(f[i]-t)/B[i][L-1];
	        }

	        for(int i=N-1;i>=0;--i)
	        {
		        double t=0;
		        for(int k=i+1; k<KN(i,L,N/2); ++k)
                {
			        t+=C[i][k-i+L-1]*res[k];
                }
		        res[i]=y[i]-t;
	        }

            return res;
        }

        private void AddTriangle(int[][] A,int a, int b, int c, int k, Point[]C)
        {
            double p;
            p=(C[b].X-C[a].X)*(C[c].Y-C[b].Y)-(C[b].Y-C[a].Y)*(C[c].X-C[b].X);
            if(p<0)
            {
                int t=b;
                b=c;
                c=t;
            }

   	        A[k][0]=a;
	        A[k][1]=b;
	        A[k][2]=c;
        }

//---------------------------------------------------------------------------
        private bool CheckTriangle(int a,int b,int c, int Ni, int[] Bi)
        {
            int i;

	        for(i=0; i<Ni && Bi[i]<=a; ++i);
            if (Bi[i] != a)
            {
                return true;
            }
	        
            for(i=0; i<Ni && Bi[i]<=b; ++i);
            if (Bi[i] != b)
            {
                return true;
            }

	        for(i=0;i<Ni&&Bi[i]<=c;++i);
            if (Bi[i] != c)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
//-----------------Получение массива треугольников-------------------------
        private int[][] GetTriangles(int N, Point[]Coords, int[][]M)
        {
            int[][] Tr = new int[2*N][];
            for(int i=0;i<2*N;++i)
            {
                Tr[i]= new int[3];
            }
          
            int Ntr=0;
            int[] tmp2 = new int[5];
            for(int i=0;i<N;++i)
            {
                for(int j=i+1;j<N;++j)
                {
                    if(M[i][j]>0)
	                {
                        int count=1;
                        for(int k=j+1;k<N;++k)
                        {
                            if((M[i][k] > 0)&& (M[j][k]>0))
                            {
                                count++;
                                tmp2[count]=k;
                            }
                        }

                        if(count>1) 
                        {
                            AddTriangle(Tr, i, j, tmp2[2], Ntr, Coords);
                            Ntr++;
                        }

	                    if(count==3) 
                        {
                            AddTriangle(Tr,i,j,tmp2[3],Ntr,Coords);
                            Ntr++;
                        }
                    }
                } //for j
            } //for i

            Array.Resize(ref Tr, Ntr);

            return Tr;
        }

//-------------------1-ое произведение матриц----------------------------
        private void FirstProductMatrix(double[][] A, double[][] B, double[][] C)
        {
           double sum;
           for (int i = 0; i < 6; i++)
           {
               for (int j = 0; j < 3; j++)
               {
                   sum = 0;
                   for (int k = 0; k < 3; k++)
                   {
                       sum += A[i][k] * B[k][j];
                   }
                   C[i][j] = sum;
               }
           }
        }
//----------------------------------------------------------------------------
        private void SecondProductMatrix(double[][] A, double[][] B, double[][] C)
        {
            double sum;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    sum = 0;
                    for (int k = 0; k < 3; k++)
                    {
                        sum += A[i][k] * B[k][j];
                    }
                    C[i][j] = sum;
                }
            }
        }
//-----------------------   Транспонирование   ------------------------------
        private void Transpon(double[][] A, int n1, int n2, double[][] C)
        {
            for (int i = 0; i < n2; i++)
            {
                for (int j = 0; j < n1; j++)
                {
                    C[i][j] = A[j][i];
                }
            }
        }
//------------------Площадь треуголиника по 3-м точкам-----------------------
        private double GetTriangleArea(Point[] p)
        {
          return(Math.Abs((p[1].X-p[0].X)*(p[2].Y-p[0].Y)-(p[2].X-p[0].X)*(p[1].Y-p[0].Y))/2);
        }
//---------------------------------------------------------------------------
        private void Fill_Be(double[][] Be, Point[] Coor)
        {
            double[] b = new double[3]; 
            double[] c = new double[3];
           
            b[0]=Coor[1].Y-Coor[2].Y;
            b[1]=Coor[2].Y-Coor[0].Y;
            b[2]=Coor[0].Y-Coor[1].Y;
            c[0]=-Coor[1].X+Coor[2].X;
            c[1]=-Coor[2].X+Coor[0].X;
            c[2]=-Coor[0].X+Coor[1].X;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Be[i][j] = 0;
                }
            }

            int ind=0;
            for (int j=0; j<6; j+=2)
            {
                Be[0][j]=b[ind];
                ind++;
            }
            
            ind=0;
            for (int j=1; j<6; j+=2)
            {
                Be[1][j]=c[ind];
                ind++;
            }
            
            ind=0;
            for (int j=0; j<6; j+=2)
            {
                Be[2][j]=c[ind];
                Be[2][j+1]=b[ind];
                ind++;
            }
            
            double s=GetTriangleArea(Coor);
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 6; ++j)
                {
                    Be[i][j] /= s * 2;
                }
            }
        }
//--------------------------------------------------------------------------
        private void Fill_De(double[][] De, double E, double v)
        {
            double Coef=E/(1-v*v);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    De[i][j] = 0;
                }
            }

            De[0][0]=Coef;
            De[1][1]=Coef;
            De[1][0]=v*Coef;
            De[0][1]=v*Coef;
            De[2][2]=Coef*(1-v)/2;
        }
//--------------Генерация матрицы жёсткости элемента---------------------------
        private void Get_Ke(int num, double[][] Ke, Point[] Coords, int[][] tr, double E, double v)
        {
            //*******************************
            double[][] Be = new double[3][];
            double[][] De = new double[3][];
            double[][] Be_t = new double[6][];
            double[][] Be_t_De = new double[6][];
            Point[] Coor = new Point[3];
            for (int i=0; i<6; i++)
            {
                Be_t[i]= new double[3];
                Be_t_De[i] = new double[3];
            }
            for (int i=0; i<3; i++)
            {
                De[i] = new double[3];
                Be[i] = new double[6];
            }
            //********************************
            Get_Coor(num,Coor, Coords, tr);
            Fill_Be(Be, Coor);
            Fill_De(De, E, v);
            Transpon(Be,3,6,Be_t);
            FirstProductMatrix(Be_t,De,Be_t_De);              //множим Be_t с De получаем Be_t_De
            SecondProductMatrix(Be_t_De, Be, Ke);
        }
//----------------------------------------------------------------------
        private void Get_Coor(int e, Point[] Coor, Point[] Coords, int[][] Tr)
        {
            for (int i = 0; i < 3; ++i)
            {
                Coor[i] = Coords[Tr[e][i]];
            }
           //Не знаю Оксаниных массивов
        }
//---------------Генерация матрицы жёсткости системы-------------------------
        private void GetK(double[][] K, int Ntr, Point[] Coords, int[][] Tr, int L, double E, double v)
        {

            double[][] Ke = new double[6][];

            for (int i = 0; i < 6; ++i)
            {
                Ke[i] = new double[6];
            }

            for(int e=0; e<Ntr; ++e)
            {
                Get_Ke(e,Ke, Coords, Tr, E, v);
                for(int i=0;i<3;i++)
                {
                    for(int j=0;j<3;j++)
                    {
                        int m=Tr[e][i]*2;
                        int n=Tr[e][j]*2;
                        K[m][n-m+L-1]+=Ke[2*i][2*j];
                        K[m+1][n-m+L-1]+=Ke[2*i+1][2*j+1];
                        K[m+1][n-m-1+L-1]+=Ke[2*i+1][2*j];
                        K[m][n-m+L]+=Ke[2*i][2*j+1];
                    }
                }
             }
        }
//----------------------Учёт граничных условий--------------------------
        /* Возвращает true, если хотя бы одно граничное условие найдено; иначе false */
        private bool SetBoundaryConditions(double[][] K, int N, int L, double[] f, double[] u)
        {
            bool ret = false;
            for (int i = 0; i < N; ++i)
            {
                if (u[i] != 0)
                {
                    ret = true;

                    for (int k = K0(i, L); k < KN(i, L, N/2); ++k)
                    {
                        f[k] -= K[k][i - k + L - 1] * u[i];
                        K[k][i - k + L - 1] = K[i][k - i + L - 1] = 0;
                    }
                    K[i][L - 1] = 1;
                    f[i] = u[i];
                }
            }

            return ret;
        }
//-------------------Основная процедура решения---------------------
        double[] Solve_main(int L, int N, int Ntr, Point[] Coords, int[][] tr, double[] u, double E, double v)
        {
            double[] deltas = null;

            double[] f = new double[2 * N];
            double[][] K = new double[2 * N][];
            for(int i=0; i<2*N; ++i)
            {
                f[i]=0;
                K[i] = new double[2 * L];
            }

            GetK(K, Ntr, Coords, tr, L, E, v);
            if (SetBoundaryConditions(K, 2 * N, L, f, u))
            {
                deltas = Cholesky(K, 2 * N, L, f);
            }

            return deltas;
        }

/*----------------------------------------------------------------*/
        /* Алгоритм Кахилла-Маки перенумерации вершин */
        /* Функция возвращает список вершин, находящихся на последнем уровне графа для вершины beginTop, 
         * и количество уровней levelNum*/
        int[] lastLevel(int[][] connMatrix, int N, int beginTop, out int levelNum) 
        {
            int level = 0;
            Queue<int[]> qLevels = new Queue<int[]>();
            List<int[]> ll = new List<int[]>();
            List<int> passed = new List<int>();
            List<int> l = new List<int>();
            int[] l1 = new int[1] { beginTop };
            passed.Add(beginTop);
            ll.Add(l1);
            qLevels.Enqueue(l1);
            while (qLevels.Count != 0) 
            {
                l1 = qLevels.Dequeue();
                l = new List<int>();
                foreach (int i in l1) 
                {
                    for (int j = 0; j < N; j++) 
                    {
                        if ((i != j)&&(connMatrix[i][j] == 1)&&(!passed.Contains(j)))
                        {
                            l.Add(j);
                            passed.Add(j);
                        }
                    }
                }
                if (l.Count != 0) 
                {
                    level++;
                    qLevels.Enqueue(l.ToArray());
                    ll.Add(l.ToArray());
                }
            }
            levelNum = level;
            return ll[level];
        }

        /*Возвращает список соседних вершин для вершины top*/
        int[] pairedTops(int[][] connMatrix, int N, int top, List<int> passed)
        {
            List<int> l = new List<int>();
            for (int j = 0; j < N; j++) 
            {
                if ((top != j)&&(connMatrix[top][j] == 1)&&(!passed.Contains(j)))
                {
                    l.Add(j);
                }
            }
            return l.ToArray();
        }

        /*Функция, определяющая количество связей у узла top*/
        int countLink(int[][] connMatrix, int N, int top) 
        {
            int count = 0;
            for (int j = 0; j < N; j++)
            {
                if ((top != j) && (connMatrix[top][j] == 1))
                {
                    count++;
                }
            }
            return count;
        }

        /*Алгоритм Катхилла-Макки*/
        int[] CuthillMcKee(int[][] connMatrix, out bool ok)
        {
            int N = connMatrix[0].Length;
            int levelFirst; 
            int[] levelCurrL;
            int[] first, curr;
            first = lastLevel(connMatrix, N, 0, out levelFirst);
            int top;
            top = first[0];
            if (first.Length > 1)
            {
                //Определяем вершину, из тех что на последнем уровне, с минимальным числом связей
                int count, min;
                top = first[0];
                min = countLink(connMatrix, N, top);
                foreach (int i in first)
                {
                    count = countLink(connMatrix, N, i);
                    if (count < min)
                    {
                        min = count;
                        top = i;
                    }
                }
            }
            first = lastLevel(connMatrix, N, top, out levelFirst);
            levelCurrL = new int[first.Length];
            int k = 0;
            foreach (int i in first) 
            {
                curr = lastLevel(connMatrix, N, i, out levelCurrL[k]);
                k++;
            }
            ok = true;
            foreach (int i in levelCurrL) 
            {
                if (i != levelFirst) 
                {
                   ok = false;
                    break;
                }
            }
            int[] result = new int[N];
            /*Если алгоритм сразу не нашёл начальную вершину для 
             перенумерации, то он не сработает и далее решение пойдёт по старой нумерации*/
            if (!ok)
            {
                return null;
            }
            else 
            {
                /*Генерирование результирующего вектора, в котором индексы новые номера вершин,
                 * а содержимое старые номера*/
                int i = N-1;
                Queue<int> q = new Queue<int>();
                q.Enqueue(top);
                List<int> l = new List<int>();
                int[] l1;
                l.Add(top);
                while (q.Count != 0)  
                {
                    top = q.Dequeue();
                    result[i] = top;
                    i--;
                    l1 = pairedTops(connMatrix, N, top, l);
                    foreach (int j in l1) 
                    {
                        q.Enqueue(j);
                        l.Add(j);
                    }

                }
            }
            return result;   
        }

        /*Применение изменений, последовавших в ходе алгоритма Ктахилла-Макки*/
        void applyCuthillMcKee(CommonData c, int[] newTopsNum) 
        {
            List<int> l = newTopsNum.ToList<int>();
            List<int[]> newConnMatrix = new List<int[]>();
            int[] connString;
            for (int i = 0; i < c.NTotalNodes; i++) 
            {
                connString = new int[c.NTotalNodes];
                int oldTop = l[i];
                connString[i] = 1;
                for (int j = 0; j < c.NTotalNodes; j++) 
                {
                    if ((oldTop != j) && (c.M[oldTop][j] == 1)) 
                    {
                        connString[l.IndexOf(j)] = 1;
                    }
                }
                newConnMatrix.Add(connString);
            }
            Point[] newCoord = new Point[c.NTotalNodes];
            for(int i = 0; i < c.NTotalNodes; i++)
            {
                newCoord[i] = c.Coords[l[i]];
            }
            int[] newBoundExternal = new int[c.NExternalNodes];
            int k = 0;
            foreach (int i in c.BoundExternal) 
            {
                newBoundExternal[k] = l.IndexOf(i);
                k++;
            }
            int[] newBoundInternal = new int[c.NInternalNodes];
            k = 0;
            foreach (int i in c.BoundInternal)
            {
                newBoundInternal[k] = l.IndexOf(i);
                k++;
            }

            int[] newChangedPoints = new int[c.ChangedPoints.Length];
            k = 0;
            foreach (int i in c.ChangedPoints)
            {
                newChangedPoints[k] = l.IndexOf(i);
                k++;
            }
            if (getBandWidthOfConnectivityMatrix(c.M)>getBandWidthOfConnectivityMatrix(newConnMatrix.ToArray()))
            {
                c.M = newConnMatrix.ToArray();
                c.BoundExternal = newBoundExternal;
                c.BoundInternal = newBoundInternal;
                c.Coords = newCoord;
                c.SetChangedPoints(newChangedPoints, true);
            }
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
        public double[] Solve(CommonData cd)
        {
            /* Вызываем Катхилла-Макки для перенумерации */
            /* Инициализируем глобальную матрицу жесткости K */
            /* Для каждого элемента считаем матрицу жесткости с одновременным занесением в глобальную матрицу жесткости */
            /* Преобразовать K к L-U виду */
            /* Посчитать f с учетом граничных условий (CommonData.u) */
            /* Решаем методом Холецкого LU*x=f и возвращаем результат */

            bool ok;
            int[] m = CuthillMcKee(cd.M, out ok);
            if (ok)
            {
                applyCuthillMcKee(cd, m);
            }
            int L = getBandWidthOfConnectivityMatrix(cd.M);
            int[][] tr = GetTriangles(cd.NTotalNodes, cd.Coords, cd.M);
            return Solve_main(L, cd.NTotalNodes, tr.Length, cd.Coords, tr, cd.u, cd.E, cd.v);
        }
    }
}