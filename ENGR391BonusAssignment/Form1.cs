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
    /// <summary>
    /// Program to estimate the time that a hot-air
    /// balloon will take to traverse a specified distance
    /// using three-point Gauss Quadrature and the Secant
    /// Method
    /// 
    /// ENGR 391 Bonus Assignment
    /// by: Michael Bilinsky 26992358
    /// </summary>
    public partial class Form1 : Form
    {
        const double g = 9.81; //gravity
        const double m = 600; //mass
        const double p = 0.25; //rho
        const double diameter = 20; //diameter
        const double c = 225; //drag coefficient
        double[] C = {0.5555556, 0.8888889, 0.5555556}; //Coefficient Ci (weights)
        double[] x = { -0.77456667, 0, 0.77459667 }; //Gauss points xi
        double distanceValue; //solve for a d

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

            //colapse view with graph
            splitContainer1.Panel2Collapsed = true;

            
        }

        private void Form1_Load(object sender, EventArgs e)
        {  
        }

        /// <summary>
        /// Performs the Secant Method on Zeroed Function
        /// </summary>
        /// <param name="guess1">Initial guess 1</param>
        /// <param name="guess2">Initial guess 2</param>
        /// <param name="intervals">Number of intervals for the composite Gauss Quadrature</param>
        /// <returns>Estimated result</returns>
        private double SecantCalculation(double guess1, double guess2, int intervals)
        {
            double fcurrent, flast, next;
            double last = guess1;
            double current = guess2;
            //calculate the first f(x_i-1)
            flast = ZeroedFunction(last, intervals);

            //evalutate until the relative approximate error is less than 0.001%
            while(Math.Abs((current - last)/current) > 0.00001)
            {
                //calculate f(x_i)
                fcurrent = ZeroedFunction(current, intervals);
                //calculate f(x_i+1)
                next = current - fcurrent * (current - last) / (fcurrent - flast);
                //store f(x_i-1)
                flast = fcurrent;
                //store x_i-1
                last = current;
                //save the new x_i
                current = next;
            }

            return current;
        }

        /// <summary>
        /// Evaluate f(t) about the distance value
        /// </summary>
        /// <param name="t">Guess t</param>
        /// <param name="intervals">Number of intervals for the composite Gauss Quadrature</param>
        /// <returns>Result of the Gauss Quadrature estimation</returns>
        private double ZeroedFunction(double t, int intervals)
        {
            return distanceValue - EstimateGaussFunction(t, intervals);
        }

        /// <summary>
        /// Calculate the Composite Gauss Quadrature
        /// </summary>
        /// <param name="t">Guess for t</param>
        /// <param name="intervals">Number of composite intervals</param>
        /// <returns>Gauss Quadrature Estimate</returns>
        private double EstimateGaussFunction(double t, int intervals)
        {
            double result = 0;
            //iterate through the composite intervals
            for (int j = 0; j < intervals; j++)
            {
                //calculate the a and b bounds for the interval
                double a = j * (t / (double)intervals);
                double b = a + (t / (double)intervals);

                //calculate the three Ci*f(xi) values of three-point quadrature
                for (int i = 0; i < 3; i++)
                {
                    result += C[i] * EstimatedGaussFunctioni(x[i], a, b);
                }
            }
            //multiply the constants before returning the result
            return (A * m / c) * result;

        }

        /// <summary>
        /// Estimate the f(xi) of integral f(t)
        /// </summary>
        /// <param name="xi">Gauss point to evaluate</param>
        /// <param name="a">Lower bound</param>
        /// <param name="b">Upper bound</param>
        /// <returns>Result of f(xi)</returns>
        private double EstimatedGaussFunctioni(double xi, double a, double b)
        {
            return (1.0 - Math.Exp(-(c / m) * ((b - a) * xi + a + b) / 2.0)) * (b - a) / 2.0;
        }

        /// <summary>
        /// The "Calculate Time" button. Executes the calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            int low, high; //upper and lower interval values
            double guess1, guess2; //guesses for the Secant Method
            try
            {
                //Retrieve the values from the input boxes
                low = int.Parse(textBox1.Text) - 1;
                high = int.Parse(textBox2.Text);
                guess1 = double.Parse(textBox3.Text);
                guess2 = double.Parse(textBox4.Text);
                distanceValue = double.Parse(textBox5.Text);
            } 
            catch(Exception ex)
            {
                //Catch and Parsing error
                MessageBox.Show("An error has occured: " + ex.Message);
                return;
            }

            //Clear the chart and the table
            chart1.Series[0].Points.Clear();
            listView1.Items.Clear();

            int[] intervals = new int[high];
            double[] values = new double[high];

            for (int i = low; i < high; i++)
            {
                //store the number of intervals
                intervals[i] = i + 1;
                //Perform the calculation and store the result
                values[i] = SecantCalculation(guess1, guess2, i + 1);
                //Plot the result on the chart
                chart1.Series[0].Points.AddXY(intervals[i], values[i]);
                //Add the result to the table
                listView1.Items.Add(new ListViewItem(new string[] { intervals[i].ToString(), values[i].ToString() }));
                //Print to console for debugging purposes
                Console.WriteLine(intervals[i].ToString() + "\t" + values[i].ToString());
            } 
                        
            //toggle the split container to show the results page
            splitContainer1.Panel2Collapsed = false;
            splitContainer1.Panel1Collapsed = true;
        }

        /// <summary>
        /// The "Reset" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //toggle the split container to go back to the parameters page
            splitContainer1.Panel2Collapsed = true;
            splitContainer1.Panel1Collapsed = false;
        }
    }
}
