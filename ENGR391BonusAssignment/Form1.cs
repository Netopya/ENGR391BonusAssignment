using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ENGR391BonusAssignment
{
    /* TODO:
     *  Add comments
     *  Add table of values
     *  Add chart title and legend 
     *  CREATE GUI
     * */
    public partial class Form1 : Form
    {
        const double g = 9.81; //gravity
        const double m = 600; //mass
        const double p = 0.25; //rho
        const double diameter = 20; //diameter
        const double c = 225; //drag coefficient
        double[] C = {0.5555556, 0.8888889, 0.5555556}; //Coefficient Ci (weights)
        double[] x = { -0.77456667, 0, 0.77459667 }; //Gauss points xi
        double distainceValue; //solve for a d

        readonly double V; //Volume
        readonly double A; //A values

        public Form1()
        {
            //calculate volume
            V = (4.0 / 3.0) * Math.PI * Math.Pow(diameter / 2.0, 3);

            //calculate A
            A = (p * V / m - 1.0) * g;

            InitializeComponent();

            //increase column width to display title
            listView1.Columns[1].Width = 160;

            splitContainer1.Panel2Collapsed = true;

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int[] intervals = new int[30];
            double[] values = new double[30];

            for (int i = 0; i < 30; i++)
            {
                intervals[i] = i + 1;
                values[i] = SecantCalculation(1, 2, i + 1);
                chart1.Series[0].Points.AddXY(intervals[i], values[i]);
                listView1.Items.Add(new ListViewItem(new string[] { intervals[i].ToString(), values[i].ToString() }));
            }            
        }

        private double SecantCalculation(double guess1, double guess2, int intervals)
        {
            double fcurrent, flast, next;
            double last = guess1;
            double current = guess2;
            flast = ZeroedFunction(last, intervals);

            while(Math.Abs((current - last)/current) > 0.00001)
            {
                fcurrent = ZeroedFunction(current, intervals);
                next = current - fcurrent * (current - last) / (fcurrent - flast);
                flast = fcurrent;
                last = current;
                current = next;
            }

            return current;
        }

        private double ZeroedFunction(double t, int intervals)
        {
            return 1000.0 - EstimateGaussFunction(t, intervals);
        }

        private double EstimateGaussFunction(double t, int intervals)
        {
            double result = 0;
            for (int j = 0; j < intervals; j++)
            {
                double a = j * (t / (double)intervals);
                double b = a + (t / (double)intervals);

                for (int i = 0; i < 3; i++)
                {
                    result += C[i] * EstimatedGaussFunctioni(x[i], a, b);
                }
            }
            return (A * m / c) * result;

        }

        private double EstimatedGaussFunctioni(double xi, double a, double b)
        {
            return (1 - Math.Exp(-(c / m) * ((b - a) * xi + a + b) / 2.0)) * (b - a) / 2.0;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.Panel1Collapsed = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel1Collapsed = false;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
