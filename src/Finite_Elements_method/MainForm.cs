using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Finite_Elements_method
{
    public partial class MainForm : Form
    {
        private CommonData cd;
        private double x0, x1, y0, y1;
        private int height, width;
        private int currentPoint = -1;
        private int oldX,oldY;
        List<int> changedPoints;

        void GetExtremePosition() 
        {
            double xmax, xmin, ymax, ymin, d1, d2, d;
            xmax = xmin = cd.Coords[0].X;
            ymax = ymin = cd.Coords[0].Y;

            foreach (Point p in cd.Coords) 
            {
                if (p.X > xmax) xmax = p.X;
                if (p.X < xmin) xmin = p.X;
                if (p.Y > ymax) ymax = p.Y;
                if (p.Y < ymin) ymin = p.Y;
            }
            d1 = Math.Abs(xmax - xmin)/2;
            d2 = Math.Abs(ymax - ymin)/2;
            if (d1 > d2) d = d1;
            else d = d2;
            x0 = xmin - d;
            x1 = xmax + d;
            y0 = ymin - d;
            y1 = ymax + d;
        }

        /* Параметры берутся из CommonData */
        /*Функция пока не нужна отрисовка происходит в компоненте. 
         * Возможно понадобиться позже для отрисовки в различных вариантах*/
        void DrawArea()
        {

        }
        
        /*Функции преобразования координат*/
        int CoordXtoScreenX(double x) 
        {
            return Convert.ToInt32(((x - x0) * width/ (x1 - x0)));
        }

        int CoordYtoScreenY(double y)
        {
            return Convert.ToInt32(((y1 - y) * height / (y1 - y0)));
        }

        double ScreenXtoCoordX(double x)
        {
            return x0 + x * (x1 - x0) / width;
        }

        double ScreenYtoCoordY(double y)
        {
            return y1 - y * (y1 - y0) / height ;
        }

        
        public MainForm()
        {
            InitializeComponent();
        }

        private void tsmiLoad_Click(object sender, EventArgs e)
        {
            if (ofdLoad.ShowDialog() == DialogResult.OK) 
            {
                cd = new CommonData();
                cd.Load(ofdLoad.FileName);
                GetExtremePosition();
                changedPoints = new List<int>();
                tsmiCalculate.Enabled = true;
                pbMain.Refresh();
            }
        }

        private void tsmiCalculate_Click(object sender, EventArgs e)
        {
            cd.SetChangedPoints(changedPoints.ToArray());
            Solver s = new Solver();
            s.Solve(cd);
            pbMain.Refresh();
            /* Создать Solver и вызывать Solver.Solve() */
            /* Поменять CommmonData.Coords в соответствии с решением */
            /* Вызвать DrawArea. Пока просто вызвать перерисовку pbMain.Refresh()*/
            Solver solver = new Solver();
            changedPoints = new List<int>(cd.ChangedPoints);
            double[] deltas = solver.Solve(cd);
            if (deltas != null)
            {
                int j;
                for (int i = 0; i < deltas.Length; i+=2)
                {
                    j = i / 2;
                    if (!changedPoints.Contains(j)) 
                    {
                        cd.Coords[i / 2].Dx += deltas[i];
                        cd.Coords[i / 2].Dy += deltas[i + 1];
                    } 
                    
                }
            }
            pbMain.Refresh();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ofdLoad.InitialDirectory = Application.StartupPath;
            tsmiCalculate.Enabled = false;
            width = pbMain.Width;
            height = pbMain.Height;
        }

        void CreateGrid(PaintEventArgs e) 
        {
            Pen pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
            for (int i = 0; i < cd.NTotalNodes; i++) 
            {
                for (int j = 0; j < cd.NTotalNodes; j++) 
                {
                    if (cd.M[i][j] == 1)
                    {
                        e.Graphics.DrawLine(pen,
                            CoordXtoScreenX(cd.Coords[i].X), 0, CoordXtoScreenX(cd.Coords[i].X), height);
                        e.Graphics.DrawLine(pen,
                             0, CoordYtoScreenY(cd.Coords[i].Y), width, CoordYtoScreenY(cd.Coords[i].Y));
                    }
                }
            }
        }
        void draw(PaintEventArgs e, bool withMoves = false)
        {
            Pen whitePen = new Pen(Color.White);
            Pen blackPen = new Pen(Color.Black);
            Pen redPen = new Pen(Color.Red);
            Pen greenPen = new Pen(Color.Green);
            Pen grayPen = new Pen(Color.LightGray);

            whitePen.Width = 2;
            blackPen.Width = 2;
            redPen.Width = 2;
            greenPen.Width = 2;
            grayPen.Width = 2;

            Point pI, pJ;
            for (int i = 0; i < cd.NTotalNodes; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (cd.M[i][j] == 1)
                    {
                        pI = cd.Coords[i];
                        pJ = cd.Coords[j];
                        if (withMoves)
                        {
                            e.Graphics.DrawLine(blackPen,
                                CoordXtoScreenX(pI.X + pI.Dx),
                                CoordYtoScreenY(pI.Y + pI.Dy),
                                CoordXtoScreenX(pJ.X + pJ.Dx),
                                CoordYtoScreenY(pJ.Y + pJ.Dy));
                        }
                        else
                        {
                            e.Graphics.DrawLine(grayPen,
                                CoordXtoScreenX(pI.X),
                                CoordYtoScreenY(pI.Y),
                                CoordXtoScreenX(pJ.X),
                                CoordYtoScreenY(pJ.Y));
                        }
                        
                    }

                }
            }

            int x, y, p;
            for (int i = 0; i < cd.NExternalNodes; i++)
            {
                p = cd.BoundExternal[i];
                pI = cd.Coords[p];
                if (withMoves)
                {
                    x = CoordXtoScreenX(pI.X + pI.Dx);
                    y = CoordYtoScreenY(pI.Y + pI.Dy);
                    e.Graphics.DrawRectangle(greenPen, x - 4, y - 4, 8, 8);
                }
                else
                {
                    x = CoordXtoScreenX(pI.X);
                    y = CoordYtoScreenY(pI.Y);
                    e.Graphics.DrawRectangle(grayPen, x - 4, y - 4, 8, 8);
                }

            }


            for (int i = 0; i < cd.NInternalNodes; i++)
            {
                p = cd.BoundInternal[i];
                pI = cd.Coords[p];
                if (withMoves)
                {
                    x = CoordXtoScreenX(pI.X + pI.Dx);
                    y = CoordYtoScreenY(pI.Y + pI.Dy);
                    e.Graphics.DrawRectangle(redPen, x - 4, y - 4, 8, 8);
                }
                else
                {
                    x = CoordXtoScreenX(pI.X);
                    y = CoordYtoScreenY(pI.Y);
                    e.Graphics.DrawRectangle(grayPen, x - 4, y - 4, 8, 8);
                }
            }
            Font font = new Font("Times New Roman", 10);
            for (int i = 0; i < cd.NTotalNodes; i++)
            {
                pI = cd.Coords[i];
                if (withMoves)
                {
                    x = CoordXtoScreenX(pI.X + pI.Dx);
                    y = CoordYtoScreenY(pI.Y + pI.Dy);
                    e.Graphics.DrawString(i.ToString(), font, blackPen.Brush, x + 3, y + 3);
                }
                else
                {
                    x = CoordXtoScreenX(pI.X);
                    y = CoordYtoScreenY(pI.Y);
                    e.Graphics.DrawString(i.ToString(), font, grayPen.Brush, x + 3, y + 3);
                }

            }

        }

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            if (cd != null) 
            {
                draw(e);
                draw(e, true);
                if (tsmiPaintGrid.Checked)
                {
                    CreateGrid(e);
                }
            }
        }

        private void pbMain_Resize(object sender, EventArgs e)
        {
            height = pbMain.Height;
            width = pbMain.Width;
            pbMain.Refresh();

        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            pbMain.Height = this.height;
            pbMain.Width = this.width;
        }

        private void pbMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (cd == null) return;
            if (currentPoint != -1) return;
            Point p;
            for (int i = 0; i < cd.NExternalNodes; i++)
            {

                p = cd.Coords[cd.BoundExternal[i]];
                if ((Math.Abs(CoordXtoScreenX(p.X+p.Dx)-e.X) < 4)&&
                    (Math.Abs(CoordYtoScreenY(p.Y+p.Dy)-e.Y) < 4))
                {
                    pbMain.Cursor = Cursors.Hand;
                    return;
                }
                
            }
            for (int i = 0; i < cd.NInternalNodes; i++)
            {

                p = cd.Coords[cd.BoundInternal[i]];
                if ((Math.Abs(CoordXtoScreenX(p.X + p.Dx) - e.X) < 4) &&
                    (Math.Abs(CoordYtoScreenY(p.Y + p.Dy) - e.Y) < 4))
                {
                    pbMain.Cursor = Cursors.Hand;
                    return;
                }
            }
            pbMain.Cursor = Cursors.Arrow;
        }


        private void pbMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (cd == null) return;
            if (e.Button == System.Windows.Forms.MouseButtons.Left) 
            {
                Point p;
                int num;
                for (int i = 0; i < cd.NExternalNodes; i++)
                {
                    num = cd.BoundExternal[i];
                    p = cd.Coords[num];
                    if ((Math.Abs(CoordXtoScreenX(p.X + p.Dx) - e.X) < 4) &&
                        (Math.Abs(CoordYtoScreenY(p.Y + p.Dy) - e.Y) < 4))
                    {
                        currentPoint = num;
                        oldX = e.X;
                        oldY = e.Y;
                        pbMain.Cursor = Cursors.Hand;
                    }

                }
                for (int i = 0; i < cd.NInternalNodes; i++)
                {
                    num = cd.BoundInternal[i];
                    p = cd.Coords[num];
                    if ((Math.Abs(CoordXtoScreenX(p.X + p.Dx) - e.X) < 4) &&
                        (Math.Abs(CoordYtoScreenY(p.Y + p.Dy) - e.Y) < 4))
                    {
                        currentPoint = num;
                        oldX = e.X;
                        oldY = e.Y;
                        pbMain.Cursor = Cursors.Hand;
                    }
                }
            }
        }

        private void pbMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (cd == null) return;
            if (currentPoint != -1) 
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left) 
                {
                    changedPoints.Add(currentPoint);
                    cd.Coords[currentPoint].Dx += ScreenXtoCoordX(e.X) - ScreenXtoCoordX(oldX);
                    cd.Coords[currentPoint].Dy += ScreenYtoCoordY(e.Y) - ScreenYtoCoordY(oldY);
                }
            }
            currentPoint = -1;
            pbMain.Cursor = Cursors.Arrow;
            pbMain.Refresh();
        }

        private void tsmiPaintGrid_Click(object sender, EventArgs e)
        {
            if(tsmiPaintGrid.Checked)
            {
                tsmiPaintGrid.Checked = false;
            }
            else
            {
                tsmiPaintGrid.Checked = true;
            }
            pbMain.Refresh();
        }

    }
}
