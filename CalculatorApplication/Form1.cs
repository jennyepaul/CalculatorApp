using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApplication
{
    public partial class Form1 : Form
    {
        //the result of calculations
        private static Int64 result;
        public Form1()
        {
            InitializeComponent();

            //output box should only be readonly, so user cannot change value
            calculatorOutputBox.ReadOnly = true;

            //add the unicode symbols to the needed buttons
            divisionButton.Text = "\x00F7";
            moduloButton.Text = "\u0025";
        }

        //Function -> converts the text that is in the output field to an integer so we can do math 
        private Int64 ConvertOutputToInt()
        {
            //create a stringBuilder since we will be concatenating & strings are immutable in C#
            StringBuilder result = new StringBuilder(calculatorOutputBox.Text);

            //add commas to output as the user adjusts the value
            while (result.ToString().IndexOf(',') != -1)
            {
                result.Remove(result.ToString().IndexOf(','), 1);
            }

            //return result
            return Convert.ToInt64(result.ToString());
        }

    }
}
