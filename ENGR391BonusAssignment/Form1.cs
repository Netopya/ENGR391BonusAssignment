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
     * 
     * */
    public partial class Form1 : Form
    {
        const double g = 9.81;
        const double m = 600;
        const double p = 0.25;
        const double diameter = 20;
        const double c = 225;
        double[] C = {0.5555556, 0.8888889, 0.5555556};
        double[] x = { -0.77456667, 0, 0.77459667 };

        readonly double V;
        readonly double A;

        public Form1()
        {
            //calculate volume
            V = (4.0 / 3.0) * Math.PI * Math.Pow(diameter / 2.0, 3);

            A = (p * V / m - 1.0) * g;
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Console.WriteLine("Result: " + SecantCalculation(1, 2, 1));

            int[] intervals = new int[30];
            double[] values = new double[30];

            for (int i = 0; i < 30; i++)
            {
                intervals[i] = i + 1;
                values[i] = SecantCalculation(1, 2, i + 1);
                chart1.Series[0].Points.AddXY(intervals[i], values[i]);
            }

            
        }

        private double SecantCalculation(double guess1, double guess2, int intervals)
        {
            double fcurrent;
            double flast;
            double last = guess1;
            double current = guess2;
            double next;
            flast = ZeroedFunction(last, intervals);
            int iterations = 0;


            while(Math.Abs((current - last)/current) > 0.00001)
            {
                iterations++;
                fcurrent = ZeroedFunction(current, intervals);
                next = current - fcurrent * (current - last) / (fcurrent - flast);
                flast = fcurrent;
                last = current;
                current = next;

                Console.WriteLine("Current guess: " + current);
            }

            Console.WriteLine("Iterations: " + iterations);
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
    }
}
