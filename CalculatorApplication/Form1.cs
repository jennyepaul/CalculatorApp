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

        //left operand vlaue of expression
        private static Nullable<Int64> leftValue = null;
        private static bool isNegative = false;
        private static bool clearOutput = false;

        //char var to hold value of arithmetic operation 
        private static char code;

        public Form1()
        {
            InitializeComponent();

            //output box should only be readonly, so user cannot change value
            calculatorOutputBox.ReadOnly = true;

            //add the unicode symbols to the needed buttons
            divisionButton.Text = "\x00F7";
            moduloButton.Text = "\u0025";
            plusMinusButton.Text = "\x00B1";

            outputLabel.Focus();

        }
        //Event -> when user clicks on any of the number buttons, this event will fire and append value of button to output
        public void NumberClick(Object sender, EventArgs e)
        {
            //if sender is null then return 
            if(sender == null)
            {
                return;
            }

            Button buttonValue = (Button)sender; 

            //if typecast to a Button failed then return!
            if(buttonValue == null)
            {
                return;
            }
            //if the flag is set, then we want to jsut see the next value clicked, not append it, so clear output box
            if(clearOutput)
            {
                calculatorOutputBox.Text = "0";
                clearOutput = false;
            }
            //temporary integer representation of our output string
            Int64 tempVariable = ConvertOutputToInt();
            tempVariable *= 10; //multiply by 10 to append value to end
            tempVariable += Convert.ToInt64(buttonValue.Text); //add value

            if(tempVariable.ToString().Length >= 18)
            {
                MessageBox.Show("Value is too big!");
                return;
            }
            //format output box
            calculatorOutputBox.Text = String.Format("{0: #,0}", tempVariable);
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

        //Event -> when clicked, we will clear the output field
        private void clearButton_Click(object sender, EventArgs e)
        {
            //if we clear the calculator output to 0, then click "clear" again we will clear out the expression box
            if(calculatorOutputBox.Text == "0")
            {
                currentExpressionBox.Text = "";
            }
           
            calculatorOutputBox.Text = "0";
            isNegative = false;
        }

        //Event -> change output value to negative or positive
        private void plusMinusButton_Click(object sender, EventArgs e)
        {
            //flip between negative and positive as user clicks this button
            isNegative = !isNegative;

            //if flag is negative, add a "-" sign, else the output field won't have a neg sign (so start at substring 1, since "-" is at 0)
            if(isNegative)
            {
                calculatorOutputBox.Text = calculatorOutputBox.Text + "-";
            }
            else
            {
                calculatorOutputBox.Text = calculatorOutputBox.Text.Substring(0, calculatorOutputBox.Text.Length - 1);
            }
        }

        //Function -> appends the numbers and symbols to the current expression output box
        private void appendCurrentExpression(string symbol)
        {
            //append current expression to expression box
            currentExpressionBox.Text = symbol + calculatorOutputBox.Text + currentExpressionBox.Text;
            clearOutput = true;
        }

        private void MathCalculations()
        {
            //convert the current result to int to do math
            result = ConvertOutputToInt();

            //based on the arithmetic button clicked, depends what math operation we do
            switch (code)
            {
                case '+':
                    leftValue = leftValue + result;
                    break;
                case '-':
                    leftValue = leftValue - result;
                    break;
                case '*':
                    leftValue = leftValue * result;
                    break;
                case '%':
                    leftValue = leftValue % result;
                    break;
                default:
                    leftValue = leftValue / result;
                    break;
            }


        }
        private void addButton_Click(object sender, EventArgs e)
        {
            appendCurrentExpression("+");
        }

        private void subtractButton_Click(object sender, EventArgs e)
        {
            appendCurrentExpression("-");
        }

        private void multiplyButton_Click(object sender, EventArgs e)
        {
            appendCurrentExpression("*");
        }

        private void divisionButton_Click(object sender, EventArgs e)
        {
            appendCurrentExpression("\x00F7");
        }

        private void moduloButton_Click(object sender, EventArgs e)
        {
            appendCurrentExpression("%");
        }

        private void equalButton_Click(object sender, EventArgs e)
        {
            //calculate the value
            MathCalculations();

            //reset code value
            code = ' ';

            //clear out current expression since we want to see final value
            currentExpressionBox.Text = "";

            //format the total value and set the flag
            calculatorOutputBox.Text = String.Format("{0:#,0}", leftValue);
            clearOutput = true;
        }

        private void ArithmeticButtonClick(object sender, EventArgs e)
        {
            if (sender == null)
                return;

            //convert the sender to button
            Button arithmeticOperation = (Button)sender;

            ///if typecast fail then return
            if (arithmeticOperation == null)
                return;

            //if first button/operand captured
            if(leftValue == null)
            {
                leftValue = ConvertOutputToInt();

                //set code to whatever the text of the button is
                code = arithmeticOperation.Text[0];
            }
            //else it is not the first operand captured so see what code it is and do the math 
            else
            {
                if (code != ' ')
                {
                    MathCalculations();
                    code = arithmeticOperation.Text[0];
                }
                else
                {
                    code = arithmeticOperation.Text[0];
                }
            }
            //output the value
            currentExpressionBox.Text = " " + arithmeticOperation.Text + " " + calculatorOutputBox.Text + currentExpressionBox.Text;
            calculatorOutputBox.Text = String.Format("{0:#,0}", leftValue);
            clearOutput = true;


        }
    }
}
