using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms.DataVisualization.Charting;

namespace Hesh
{
    public partial class Form1 : Form
    {
        Series s1;
        Label[] LA, LB;
        TextBox[,] items;
        int[,] val;
        int[] leng;
        Button solve, gener, cleanGraph, onGraph;
        Random rnd = new Random();
        TextBox[] PA, QB;
        int[,] bestStrat;
        double[] answ;
        DataGridView tableAns;
        List<float> Vgr;
        int[,] inA;
        int[,] inB;
        int[] indx;
        int ser;
        double[] Pa, Qb;
        DataGridViewCell Num0, P0, A0, Q0, B0, Vn0, Vs0, Vm0;
        Chart graph1;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            answ = new double[8];
            label1.Visible = false;
            label2.Visible = false;
            /////tryty4rtyytuy8
            label3.Visible = false;
            button1.Visible = false;
            bestStrat = new int[2, 2];
            GT1(Convert.ToInt32(numericUpDown1.Value), Convert.ToInt32(numericUpDown2.Value));
            GT2();
            numericUpDown1.Visible = false;
            numericUpDown2.Visible = false;
            Height = SystemInformation.PrimaryMonitorSize.Height;
            Width = SystemInformation.PrimaryMonitorSize.Width;
            inA = new int[leng[0], 2];
            inB = new int[leng[1], 2];
            indx = new int[2];
        }

        private void RndGen(object sender, EventArgs e)
        {
            for (int i = 0; i < leng[0]; i++)
            {
                for (int j = 0; j < leng[1]; j++)
                {
                    items[i, j].Text = Convert.ToString(rnd.Next(-5, 6));
                    items[i, j].BackColor = Color.White;
                }
            }
        }
        private void BestStrat()
        {
            int[] pA = new int[leng[0]];
            int Mm;
            int Vv;
            int[] pB = new int[leng[1]];
            for (int i = 0; i < leng[0]; i++)
            {
                Mm = 10;
                for (int j = 0; j < leng[1]; j++)
                {
                    if (val[i, j] < Mm)
                    {
                        Mm = val[i, j];
                        inA[i, 0] = i;
                        inA[i, 1] = j;
                    }
                }
                pA[i] = Mm;
            }
            for (int i = 0; i < leng[1]; i++)
            {
                Vv = -10;
                for (int j = 0; j < leng[0]; j++)
                {
                    if (val[j, i] > Vv)
                    {
                        Vv = val[j, i];
                        inB[i, 0] = j;
                        inB[i, 1] = i;
                    }
                }
                pB[i] = Vv;
            }
            int[] bb = new int[2];
            bb[0] = pA.Max();
            bb[1] = pB.Min();
            indx[0] = Array.IndexOf(pA, bb[0]);
            indx[1] = Array.IndexOf(pB, bb[1]);
            bestStrat[0, 0] = inA[indx[0], 0];
            bestStrat[0, 1] = inA[indx[0], 1];
            bestStrat[1, 0] = inB[indx[1], 0];
            bestStrat[1, 1] = inB[indx[1], 1];
            SedlPoint();
        }
        private void SedlPoint()
        {
            if (bestStrat[0, 0] == bestStrat[1, 0] && bestStrat[0, 1] == bestStrat[1, 1])
            {
                items[bestStrat[0, 0], bestStrat[0, 1]].BackColor = Color.Red;
            }
            else
            {
                items[bestStrat[0, 0], bestStrat[0, 1]].BackColor = Color.Green;
                items[bestStrat[1, 0], bestStrat[1, 1]].BackColor = Color.Yellow;
            }
        }
        private void GT2()
        {
            graph1 = new Chart();
            graph1.Size = new Size(840, 360);
            graph1.Location = new Point(580, 420);
            ChartArea ch1 = new ChartArea();
            ch1.AxisX.Minimum = 0;
            ch1.AxisX.Maximum = 40;
            ch1.AxisY.Minimum = -5;
            ch1.AxisY.Maximum = 5;
            graph1.ChartAreas.Add(ch1);
            Controls.Add(graph1);
            tableAns = new DataGridView();
            tableAns.Size = new Size(840, 360);
            tableAns.Location = new Point(580, 20);
            DataGridViewTextBoxColumn Num = new DataGridViewTextBoxColumn();
            Num.Name = "№";
            Num.HeaderText = "№";
            DataGridViewTextBoxColumn P = new DataGridViewTextBoxColumn();
            P.Name = "P";
            P.HeaderText = "P";
            DataGridViewTextBoxColumn A = new DataGridViewTextBoxColumn();
            A.Name = "A";
            A.HeaderText = "A";
            DataGridViewTextBoxColumn Q = new DataGridViewTextBoxColumn();
            Q.Name = "Q";
            Q.HeaderText = "Q";
            DataGridViewTextBoxColumn B = new DataGridViewTextBoxColumn();
            B.Name = "B";
            B.HeaderText = "B";
            DataGridViewTextBoxColumn Vn = new DataGridViewTextBoxColumn();
            Vn.Name = "Vтек";
            Vn.HeaderText = "Vтек";
            DataGridViewTextBoxColumn Vs = new DataGridViewTextBoxColumn();
            Vs.Name = "Vсм";
            Vs.HeaderText = "Vсм";
            DataGridViewTextBoxColumn Vm = new DataGridViewTextBoxColumn();
            Vm.Name = "Vср";
            Vm.HeaderText = "Vср";
            tableAns.Columns.AddRange(Num, P, A, Q, B, Vn, Vs, Vm);
            Controls.Add(tableAns);
        }
        private void GT1(int a, int b)
        {
            solve = new Button();
            solve.Size = new Size(124, 61);
            solve.Location = new Point(252, 320);
            solve.Text = "Вычисления";
            Controls.Add(solve);
            gener = new Button();
            gener.Size = new Size(124, 61);
            gener.Location = new Point(64, 320);
            gener.Text = "Заполнение";
            gener.MouseClick += RndGen;
            solve.MouseClick += NewSolut;
            Controls.Add(gener);
            cleanGraph = new Button();
            cleanGraph.Size = new Size(124, 61);
            cleanGraph.Location = new Point(252, 400);
            cleanGraph.Text = "Очистить график";
            Controls.Add(cleanGraph);
            cleanGraph.MouseClick += ClearGraph;
            onGraph = new Button();
            onGraph.Size = new Size(124, 61);
            onGraph.Location = new Point(64, 400);
            onGraph.Text = "На график";
            Controls.Add(onGraph);
            onGraph.MouseClick += NewSer;
            items = new TextBox[a, b];
            val = new int[a, b];
            Vgr = new List<float>();
            PA = new TextBox[a];
            QB = new TextBox[b];
            Pa = new double[a];
            Qb = new double[b];
            leng = new int[2];
            leng[0] = a;
            leng[1] = b;
            for (int i = 0; i < leng[0]; i++)
            {
                for (int j = 0; j < leng[1]; j++)
                {
                    items[i, j] = new TextBox();
                    items[i, j].Size = new Size(40, 20);
                    items[i, j].BackColor = Color.White;
                    items[i, j].Location = new Point(40 + j * 50, 40 + i * 30);
                    Controls.Add(items[i, j]);
                }
            }

            LA = new Label[leng[0]];
            LB = new Label[leng[1]];
            for (int i = 0; i < LA.Length; i++)
            {
                LA[i] = new Label();
                LA[i].Size = new Size(20, 20);
                LA[i].Location = new Point(20, 40 + i * 30);
                LA[i].Text = Convert.ToString(i + 1);
                Controls.Add(LA[i]);
                PA[i] = new TextBox();
                PA[i].Size = new Size(40, 20);
                PA[i].Location = new Point(440, 40 + i * 30);
                Controls.Add(PA[i]);
            }
            for (int i = 0; i < LB.Length; i++)
            {
                LB[i] = new Label();
                LB[i].Size = new Size(20, 20);
                LB[i].Location = new Point(40 + i * 50, 20);
                LB[i].Text = Convert.ToString(i + 1);
                Controls.Add(LB[i]);
                QB[i] = new TextBox();
                QB[i].Size = new Size(40, 20);
                QB[i].Location = new Point(40 + i * 50, 280);
                Controls.Add(QB[i]);
            }
        }
        private void CreateLine()
        {
            Num0 = new DataGridViewTextBoxCell();
            P0 = new DataGridViewTextBoxCell();
            A0 = new DataGridViewTextBoxCell();
            Q0 = new DataGridViewTextBoxCell();
            B0 = new DataGridViewTextBoxCell();
            Vn0 = new DataGridViewTextBoxCell();
            Vs0 = new DataGridViewTextBoxCell();
            Vm0 = new DataGridViewTextBoxCell();
            Num0.Value = Convert.ToString(answ[0]);
            P0.Value = Convert.ToString(answ[1]);
            A0.Value = Convert.ToString(answ[2]);
            Q0.Value = Convert.ToString(answ[3]);
            B0.Value = Convert.ToString(answ[4]);
            Vn0.Value = Convert.ToString(answ[5]);
            Vs0.Value = Convert.ToString(answ[6]);
            Vm0.Value = Convert.ToString(answ[7]);
            DataGridViewRow row0 = new DataGridViewRow();
            row0.Cells.AddRange(Num0, P0, A0, Q0, B0, Vn0, Vs0, Vm0);
            tableAns.Rows.Add(row0);
            answ[0] += 1;

        }
        private void NFvers()
        {
            Pa[0] = Convert.ToDouble(PA[0].Text);
            for (int i = 1; i < PA.Length; i++)
            {
                Pa[i] = Pa[i - 1] + Convert.ToDouble(PA[i].Text);
            }
            Qb[0] = Convert.ToDouble(QB[0].Text);
            for (int i = 1; i < QB.Length; i++)
            {
                Qb[i] = Qb[i - 1] + Convert.ToDouble(QB[i].Text);
            }
        }
        private void NewSolut(object sender, EventArgs e)
        {
            Vgr.Clear();
            for (int i = 0; i < leng[0]; i++)
            {
                for (int j = 0; j < leng[1]; j++)
                {
                    val[i, j] = Convert.ToInt32(items[i, j].Text);
                }
            }
            tableAns.Rows.Clear();
            BestStrat();
            NFvers();
            double eps = 0.2;
            int it = 0;
            answ[0] = 1;
            for (int i = 0; i < 4; i++)
            {
                answ[1] = Math.Round(rnd.NextDouble(), 2);
                answ[3] = Math.Round(rnd.NextDouble(), 2);
                if (Pa[0] >= answ[1])
                {
                    answ[2] = 1;
                }
                else
                {
                    for (int j = 0; j < Pa.Length - 1; j++)
                    {
                        if (Pa[j] < answ[1] && Pa[j + 1] >= answ[1])
                        {
                            answ[2] = j + 2;
                            break;
                        }
                    }
                    if (Pa.Length == 2)
                    {
                        answ[2] = 2;
                    }
                }
                if (Qb[0] >= answ[3])
                {
                    answ[4] = 1;
                }
                else
                {
                    for (int j = 0; j < Qb.Length - 1; j++)
                    {
                        if (Qb[j] < answ[3] && Qb[j + 1] >= answ[3])
                        {
                            answ[4] = j + 2;
                            break;
                        }
                    }
                    if (Qb.Length == 2)
                    {
                        answ[4] = 2;
                    }
                }
                Vn((int)answ[2] - 1, (int)answ[4] - 1);
                Vsum();
                Vmid();
                CreateLine();
            }
            while (it<40)
            {
                if (Math.Abs(Vgr[Vgr.Count - 1] - Vgr[Vgr.Count - 2]) < eps && (Math.Abs(Vgr[Vgr.Count - 1] - Vgr[Vgr.Count - 3]) < eps) && (Math.Abs(Vgr[Vgr.Count - 1] - Vgr[Vgr.Count - 4]) < eps))
                {
                    break;
                }
                answ[1] = Math.Round(rnd.NextDouble(), 2);
                answ[3] = Math.Round(rnd.NextDouble(), 2);
                if (Pa[0] >= answ[1])
                {
                    answ[2] = 1;
                }
                else
                {
                    for (int i = 0; i < Pa.Length - 1; i++)
                    {
                        if (Pa[i] < answ[1] && Pa[i + 1] >= answ[1])
                        {
                            answ[2] = i + 2;
                            break;
                        }
                    }
                    if (Pa.Length == 2)
                    {
                        answ[2] = 2;
                    }
                }
                if (Qb[0] >= answ[3])
                {
                    answ[4] = 1;
                }
                else
                {
                    for (int i = 0; i < Qb.Length - 1; i++)
                    {
                        if (Qb[i] < answ[3] && Qb[i + 1] >= answ[3])
                        {
                            answ[4] = i + 2;
                        }
                    }
                    if (Qb.Length == 2)
                    {
                        answ[4] = 2;
                    }
                }
                Vn((int)answ[2] - 1, (int)answ[4] - 1);
                Vsum();
                Vmid();
                CreateLine();
                it += 1;
            }
            for (int j = 0; j < 3; j++)
            {
                answ[1] = Math.Round(rnd.NextDouble(), 2);
                answ[3] = Math.Round(rnd.NextDouble(), 2);
                if (Pa[0] >= answ[1])
                {
                    answ[2] = 1;
                }
                else
                {
                    for (int i = 0; i < Pa.Length - 1; i++)
                    {
                        if (Pa[i] < answ[1] && Pa[i + 1] >= answ[1])
                        {
                            answ[2] = i + 2;
                            break;
                        }
                    }
                    if (Pa.Length == 2)
                    {
                        answ[2] = 2;
                    }
                }
                if (Qb[0] >= answ[3])
                {
                    answ[4] = 1;
                }
                else
                {
                    for (int i = 0; i < Qb.Length - 1; i++)
                    {
                        if (Qb[i] < answ[3] && Qb[i + 1] >= answ[3])
                        {
                            answ[4] = i + 2;
                        }
                    }
                    if (Qb.Length == 2)
                    {
                        answ[4] = 2;
                    }
                }
                Vn((int)answ[2] - 1, (int)answ[4] - 1);
                Vsum();
                Vmid();
                CreateLine();

            }
        }

        private void ClearGraph(object sender, EventArgs e)
        {
            ser = 0;
            
            graph1.Series.Clear();
        }
        private void NewSer(object sender, EventArgs e)
        {
            if (ser <= 5)
            {
                s1 = new Series();
                for (int i = 0; i < Vgr.Count; i++)
                {
                    s1.Points.AddXY(i, Vgr[i]);
                }
                s1.ChartType = SeriesChartType.Line;

                s1.IsValueShownAsLabel = true;
                s1.IsValueShownAsLabel = false;
                s1.LabelFormat = "F3";
                s1.MarkerStyle = MarkerStyle.Square;
                s1.MarkerColor = Color.LimeGreen;
                s1.Font = new Font("Microsoft Sans Serif", 8f, FontStyle.Regular);
                s1.BorderWidth = 3;
                s1.BorderColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                graph1.Series.Add(s1);
                ser += 1;
              
            }
        }
        private void Vn(int a, int b)
        {
            answ[5] = val[a, b];
        }
        private void Vsum()
        {
            if (answ[0] == 1)
            {
                answ[6] = answ[5];
            }
            else
            {
                answ[6] += answ[5];
            }
        }
        private void Vmid()
        {
            answ[7] = Math.Round(answ[6] / answ[0], 2);
            Vgr.Add((float)answ[7]);
        }
        private void SaveSedlPoint()
        {
            GT1(4, 4);
            items[0, 0].Text = "5";
            items[0, 1].Text = "2";
            items[0, 2].Text = "-1";
            items[0, 3].Text = "1";
            items[1, 0].Text = "2";
            items[1, 1].Text = "1";
            items[1, 2].Text = "0";
            items[1, 3].Text = "3";
            items[2, 0].Text = "0";
            items[2, 1].Text = "2";
            items[2, 2].Text = "0";
            items[2, 3].Text = "5";
            items[3, 0].Text = "5";
            items[3, 1].Text = "3";
            items[3, 2].Text = "-3";
            items[3, 3].Text = "5";
        }
    }
}
