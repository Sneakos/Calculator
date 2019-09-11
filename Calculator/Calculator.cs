using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class Calculator : Form
    {
        enum Operator
        {
            Add, Subtract, Multiply, Divide, None, Equals
        }

        private bool resetText, recordHistory;
        private double cachedResult;
        private Operator lastOperator;

        public Calculator()
        {
            InitializeComponent();

            Init();
        }

        private void Init()
        {
            SetTags();

            resetText = true;
            recordHistory = true;
            cachedResult = 0;
            lastOperator = Operator.None;

            Focus();
        }

        private void SetTags()
        {
            btnAdd.Tag = Operator.Add;
            btnSubtract.Tag = Operator.Subtract;
            btnMultiply.Tag = Operator.Multiply;
            btnDivide.Tag = Operator.Divide;
            btnEquals.Tag = Operator.Equals;
        }

        private void NumberButtonClick(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;

            SetResultText(btnSender.Text);

            btnEquals.Focus();
        }

        private void SetResultText(string text)
        {
            if (resetText)
            {
                txtResult.Text = text;
                resetText = false;
            }
            else
            {
                txtResult.Text += text;
            }
        }

        private void OperatorButtonClick(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;
            PerformOperation(btnSender.Text.ToLower(), (Operator)btnSender.Tag);
            btnEquals.Focus();
        }

        private void PerformOperation(string @operatorText, Operator @operator)
        {
            switch (lastOperator)
            {
                case Operator.Add: cachedResult += double.Parse(txtResult.Text); break;
                case Operator.Subtract: cachedResult -= double.Parse(txtResult.Text); break;
                case Operator.Multiply: cachedResult *= double.Parse(txtResult.Text); break;
                case Operator.Divide: cachedResult /= double.Parse(txtResult.Text); break;
                case Operator.None:
                case Operator.Equals: cachedResult = double.Parse(txtResult.Text); break;
                default: return;
            }

            resetText = true;
            lastOperator = (Operator)@operator;

            if (@operator == Operator.Equals)
            {
                txtHistory.Clear();
            }
            else if (recordHistory)
            {
                txtHistory.Text += $"{txtResult.Text} {@operatorText} ";
            }
            else
            {
                txtHistory.Text += $" {@operatorText} ";
            }
            recordHistory = true;
            txtResult.Text = cachedResult.ToString();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtHistory.Clear();
            txtResult.Text = "0";
            cachedResult = 0;
            resetText = true;
            lastOperator = Operator.None;
            btnEquals.Focus();
        }

        private void TxtResult_TextChanged(object sender, EventArgs e)
        {
            if(txtResult.Text.Length > 12)
            {
                txtResult.Text = txtResult.Text.Substring(0, 12);
            }
        }

        private void Calculator_KeyPress(object sender, KeyPressEventArgs e)
        {
            int numPressed = -1;
            string keyPressed = e.KeyChar.ToString();
            bool parseSuccessful = int.TryParse(keyPressed, out numPressed);

            if(parseSuccessful)
            {
                SetResultText(keyPressed);
                btnEquals.Focus();
                return;
            }

            string[] validOperations = new string[] { "+", "-", "*", "/", "=" };

            if(validOperations.Contains(keyPressed))
            {
                PerformOperation(keyPressed, GetOperator(keyPressed));
            }

            btnEquals.Focus();
        }

        private Operator GetOperator(string text)
        {
            switch (text)
            {
                case "+": return Operator.Add;
                case "-": return Operator.Subtract;
                case "/": return Operator.Divide;
                case "*": return Operator.Multiply;
                case "=": return Operator.Equals;
                default : return Operator.None;
            }
        }

        private void BtnDecimal_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Contains(".") && !resetText)
                return;

            if (resetText)
            {
                txtResult.Text = "0.";
            }
            else
            {
                txtResult.Text += ".";
            }
            resetText = false;
        }

        private void BtnClearEnd_Click(object sender, EventArgs e)
        {
            txtResult.Text = "0";
            resetText = true;
        }

        private void BtnDel_Click(object sender, EventArgs e)
        {
            Delete();
            btnEquals.Focus();
        }

        private void Delete()
        {
            string value = txtResult.Text;
            if (resetText)
            {
                return;
            }
            if (value.Length > 1)
            {
                if(value.Length == 2 && double.Parse(value) < 0)
                {
                    txtResult.Text = "0";
                    resetText = true;
                }
                else
                {
                    txtResult.Text = value.Substring(0, value.Length - 1);
                }
            }
            else if (value.Length == 1)
            {
                txtResult.Text = "0";
                resetText = true;
            }
        }

        private void Calculator_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                Delete();
                btnEquals.Focus();
            }
        }

        private void BtnSign_Click(object sender, EventArgs e)
        {
            double value = double.Parse(txtResult.Text) * -1;

            txtResult.Text = value.ToString();
        }

        private void BtnFraction_Click(object sender, EventArgs e)
        {
            if (!recordHistory)
                return;

            double value = 1 / double.Parse(txtResult.Text);

            txtHistory.Text += $"1/{txtResult.Text}";
            txtResult.Text = value.ToString();
            recordHistory = false;
            btnEquals.Focus();
        }
    }
}
