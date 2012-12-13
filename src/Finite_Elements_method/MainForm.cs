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
        /* Параметры берутся из CommonData */
        void DrawArea()
        {
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            /* Создать Solver и вызывать Solver.Solve() */
            /* Поменять CommmonData.Coords в соответствии с решением */
            /* Вызвать DrawArea */
        }
    }
}
