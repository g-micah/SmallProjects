/**
 * CIS 317
 * 
 * G. Micah Garrison
 */
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;

namespace CalculatorApp
{
    public partial class CalculatorForm : Form
    {
        private const UInt32 WM_LBUTTONDOWN = 0x201;
        private const UInt32 WM_LBUTTONUP = 0x202;
        private const string saveFile = "calculatorSaveData.txt";
        private const string saveFileFirstLine = "G. Micah Calculator Save File -- Do Not Edit";
        private bool hidden;              //Used to ensure caret stay hidden when main textbox is selected
        private bool memoryExists;        //Used to know if anything is in memory
        private bool memoryJustRecalled;  //Used to check if last button hit was MR
        private string output;            //Main textbox at top
        private string outputSmall;       //Small textbox above main one
        private char sign;                //What operation is being done
        private double memory;            //Stores memory value
        private double num1;              //Left number in operation
        private double num2;              //Right number in operation
        private int outputMaxLength;      //Max amount of chars allowed in output textbox
        private int mode;             
        /** mode 0: start
         *  mode 1: just clicked operation
         *  mode 2: just started inputting num after clicking operation
         *  mode 3: right after hitting equals
         */  

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr handle, UInt32 message, int wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "ShowCaret")]
        public static extern long ShowCaret(IntPtr hwnd);

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        public static extern long HideCaret(IntPtr hwnd);

        public CalculatorForm()
        {
            InitializeComponent();
            hidden = false;
        }

        //Used to hide Caret
        private void timer1_Tick_1(object sender, EventArgs e)
        {
            SendMessage(textOutput.Handle, WM_LBUTTONDOWN, 0, 0);
            SendMessage(textOutput.Handle, WM_LBUTTONUP, 0, 0);
            timer1.Stop();
        }

        //Setup constructor
        private void Form1_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            memoryJustRecalled = false;
            Reset();
            GetSaveData();
        }

        //Clear output and set mode to starting mdoe
        private void Reset()
        {
            sign = ' ';
            Output = "";
            OutputSmall = "";
            mode = 0;
        }

        //Retrieve data from saveData.txt
        private void GetSaveData()
        {
            StreamReader sr = default;
            try
            {
                sr = new StreamReader(saveFile);

                //If file not empty, save data to class attributes
                if ((sr.ReadLine()) == saveFileFirstLine)
                {
                    Output = sr.ReadLine();
                    Num1 = Convert.ToDouble(sr.ReadLine());
                    OutputSmall = sr.ReadLine();
                    sign = Convert.ToChar(sr.ReadLine());
                    mode = Convert.ToInt32(sr.ReadLine());
                    if (MemoryExists = Convert.ToBoolean(sr.ReadLine()))
                    {
                        memory = Convert.ToDouble(sr.ReadLine());
                    }
                }
                else
                {
                    MemoryExists = false;
                }
            }
            catch (IOException ex) //Throws excpetion if saveFile is not found
            {
                if(ex != null)
                {
                    MessageBox.Show("Save file not found.\nSave file will be created on exit.");
                    MemoryExists = false;
                }
            }
            finally  //Dispose to free memory
            {
                if (sr != default(StreamReader))
                {
                    sr.Dispose();
                    sr.Close();
                }
            }
        }

        //Send data to saveData.txt
        private void SetSaveData()
        {
            string[] data = new string[] { saveFileFirstLine, Output, Num1.ToString(), OutputSmall, sign.ToString(), mode.ToString(), memoryExists.ToString(), memory.ToString()};

            using (StreamWriter sw = new StreamWriter(saveFile))
            {
                foreach (string str in data)
                {
                    sw.WriteLine(str);
                }

                sw.Close();
            }
        }

        
        
        //Output property; sets textbox to output with correct format
        private string Output
        {
            get
            {
                return output;
            }
            set
            {
                //Format checks
                if (value == "")
                {
                    output = "0";
                    textOutput.Text = "0";
                }
                else if (value == "-")
                {
                    output = "-0";
                    textOutput.Text = "-0";
                }
                else if (value == ".")
                {
                    output = "0.";
                    textOutput.Text = "0.";
                }
                else
                {
                    outputMaxLength = 10;
                    //Set maxlength depending on if chars - and . are included
                    if (value.Contains("-"))
                    {
                        outputMaxLength++;
                    }
                    else if (value.Contains(".") && !value.EndsWith("."))
                    {
                        outputMaxLength++;
                    }
                    //Round off decimals if possible to let value fit
                    if(value.Length > outputMaxLength && value.Contains(".") && !value.EndsWith("."))
                    {
                        string[] temp = value.Split('.');
                        int decimalPlacesAllowed = outputMaxLength - temp[0].Length - 1;

                        if (decimalPlacesAllowed == 0)
                        {
                            value = Math.Round(Convert.ToDouble(value), 0).ToString();
                        }
                        else if (decimalPlacesAllowed == 1)
                        {
                            value = value.Substring(0, temp[0].Length + 2);
                        }
                        else
                        {
                            value = Math.Round(Convert.ToDouble(value), decimalPlacesAllowed).ToString();
                        }
                    }
                    //Only update if new value is within allowed length
                    if (value.Length <= outputMaxLength)
                    {
                        //Multiple 0s at start are not allowed
                        if (value.StartsWith("0") && value.Length > 1 && !value.StartsWith("0."))
                        {
                            output = value.Remove(0, 1);
                            textOutput.Text = output;
                        }
                        else if (value.StartsWith("-0") && value.Length > 2 && !value.StartsWith("-0."))
                        {
                            output = value.Remove(1, 1);
                            textOutput.Text = output;
                        }
                        else
                        {
                            output = value;
                            textOutput.Text = output;
                        }
                    }
                }
            }
        }
        
        //Num1 property; updates OutputSmall as well
        private double Num1
        {
            get
            {
                return num1;
            }
            set
            {
                num1 = value;
                OutputSmall = num1 + " " + sign;
            }
        }

        //Num2 property; updates OutputSmall as well
        private double Num2
        {
            get
            {
                return num2;
            }
            set
            {
                num2 = value;
                OutputSmall = num1 + " " + sign + " " + num2 + " =";
            }
        }

        //OutputSmall property; sets text as well
        private string OutputSmall
        {
            get
            {
                return outputSmall;
            }
            set
            {
                outputSmall = value;
                textOutputSmall.Text = value;
            }
        }

        //MemoryExists property; disables recall and clear if false
        private bool MemoryExists
        {
            get
            {
                return memoryExists;
            }
            set
            {
                if (value)
                {
                    btnMemoryRecall.Enabled = true;
                    btnMemoryClear.Enabled = true;
                }
                else
                {
                    memory = 0;
                    btnMemoryRecall.Enabled = false;
                    btnMemoryClear.Enabled = false;
                }
                memoryExists = value;
            }
        }

        //Handles all numerical input button clicks
        private void InputButtonClick(char btn)
        {
           pictureLogo.Focus(); //This is to get rid of the weird button selected after a button push.
                                //There is probaly a better way to do this but I do not have the time
                                //to try to figure it out and this works fine.

            if (!(btn == '.' && Output.Contains(".") && mode != 1)) //Make sure there are not two '.'
            {
                if(mode == 0) 
                {
                    Output += btn;
                }
                else if(mode == 1 || memoryJustRecalled)
                {
                    //Clear input before new input if mode button was just hit
                    Output = "" + btn;
                    mode = 2;
                    memoryJustRecalled = false;
                }
                else if (mode == 2)
                {
                    Output += btn;
                }
                else if (mode == 3)
                {
                    OutputSmall = "";
                    Output = "" + btn;
                    mode = 0;
                }
            } 
        }

        //Handles all 4 operation button clicks
        private void OperationClick(char sign)
        {
            pictureLogo.Focus();

            if (this.sign == '÷' && Convert.ToDouble(Output) == 0) // Handle Divide by 0
            {
                MessageBox.Show("Cannot divide by 0.");
            }
            else if (mode <= 1 || mode == 3)
            {
                this.sign = sign;
                Num1 = Convert.ToDouble(Output);
            }
            else if (mode == 2) // if user hits 2 operations without hitting =
            {
                Equals();
                this.sign = sign;
                Num1 = Convert.ToDouble(Output);
            }
            mode = 1;
        }

        //Hides caret after clicking into main textbox (allowing user to still copy number from textbox)
        private void textOutput_Click(object sender, EventArgs e)
        {
            if (!hidden)
            {
                HideCaret(textOutput.Handle);
                hidden = true;
            }
        }
        private void textOutput_Leave(object sender, EventArgs e)
        {
            hidden = false;
        }

        //Clear Button
        private void btnClearEntry_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            Output = "";
        }

        //Backspace Button
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            //Different action depending on the current mode
            if (mode == 1)
            {
                Output = "0";
                mode = 2;
            }
            else if (mode == 3)
            {
                Reset();
            }
            else
            {
                Output = Output.Remove(Output.Length - 1, 1);
            }
        }

        //+/- button click
        private void btnSignChange_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            //Different action depending on the current mode
            if (mode == 1)
            {
                Output = "-0";
                mode = 2;
            }
            else if (mode == 3)
            {
                Reset();
                Output = "-0";
            }
            else if (Output.StartsWith("-"))
            {
                Output = Output.Remove(0, 1);
            }
            else
            {
                Output = "-" + output;
            }
        }

        //Divide button
        private void btnDivide_Click(object sender, EventArgs e)
        {
            OperationClick('÷');
        }

        //Multiply Button
        private void btnMultiply_Click(object sender, EventArgs e)
        {
            OperationClick('×');
        }

        //Subtract Button
        private void btnSubtract_Click(object sender, EventArgs e)
        {
            OperationClick('-');
        }

        //Addition Button
        private void btnAdd_Click(object sender, EventArgs e)
        {
            OperationClick('+');
        }

        //Equals Button
        private void btnEquals_Click(object sender, EventArgs e)
        {
            if (sign == '÷' && Convert.ToDouble(Output) == 0) // Handle Divide by 0
            {
                MessageBox.Show("Cannot divide by 0.");
            }
            else
            {
                Equals();
            }
        }

        //Applys the operation entered
        private void Equals()
        {
            if (mode == 1 || mode == 2) //If doing multiple operations in a row
            {
                mode = 3;
                switch (sign)
                {
                    case '+':
                        Num2 = Convert.ToDouble(Output);
                        Output = (num1 + num2).ToString();
                        break;
                    case '÷':
                        Num2 = Convert.ToDouble(Output);
                        Output = (num1 / num2).ToString();
                        break;
                    case '×':
                        Num2 = Convert.ToDouble(Output);
                        Output = (num1 * num2).ToString();
                        break;
                    case '-':
                        Num2 = Convert.ToDouble(Output);
                        Output = (num1 - num2).ToString();
                        break;
                }
            }
            else if (mode == 3) //If hitting = more than once in a row
            {
                switch (sign)
                {
                    case '+':
                        Num1 = Convert.ToDouble(Output);
                        Num2 = Num2;
                        Output = (num1 + num2).ToString();
                        break;
                    case '÷':
                        Num1 = Convert.ToDouble(Output);
                        Num2 = Num2;
                        Output = (num1 / num2).ToString();
                        break;
                    case '×':
                        Num1 = Convert.ToDouble(Output);
                        Num2 = Num2;
                        Output = (num1 * num2).ToString();
                        break;
                    case '-':
                        Num1 = Convert.ToDouble(Output);
                        Num2 = Num2;
                        Output = (num1 - num2).ToString();
                        break;
                }
            }
        }

        //x^2 button
        private void btnSquared_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            Output = Math.Pow(Convert.ToDouble(Output),2).ToString();
            mode = 1;
        }

        //√ button
        private void btnSqrRoot_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            Output = Math.Sqrt(Convert.ToDouble(Output)).ToString();
            mode = 1;
        }

        //All numerical buttons:
        private void btn0_Click(object sender, EventArgs e)
        {
            InputButtonClick('0');
        }
        private void btn1_Click(object sender, EventArgs e)
        {
            InputButtonClick('1');
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            InputButtonClick('2');
        }
        private void btn3_Click(object sender, EventArgs e)
        {
            InputButtonClick('3');
        }
        private void btn4_Click(object sender, EventArgs e)
        {
            InputButtonClick('4');
        }
        private void btn5_Click(object sender, EventArgs e)
        {
            InputButtonClick('5');
        }
        private void btn6_Click(object sender, EventArgs e)
        {
            InputButtonClick('6');
        }
        private void btn7_Click(object sender, EventArgs e)
        {
            InputButtonClick('7');
        }
        private void btn8_Click(object sender, EventArgs e)
        {
            InputButtonClick('8');
        }
        private void btn9_Click(object sender, EventArgs e)
        {
            InputButtonClick('9');
        }
        private void btnDecimal_Click(object sender, EventArgs e)
        {
            InputButtonClick('.');
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();
            Reset();
        }

        //MR button
        private void btnMemoryRecall_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            if (mode <= 2)
            {
                Output = memory.ToString();
                mode = 2;
                memoryJustRecalled = true;
            }
            else if (mode == 3)
            {
                OutputSmall = "";
                Output = memory.ToString();
            }
        }

        //MC button
        private void btnMemoryClear_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            MemoryExists = false;
        }

        //M+ button
        private void btnMemoryAdd_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            MemoryExists = true;
            memory += Convert.ToDouble(Output);
        }

        //M- button
        private void btnMemorySubtract_Click(object sender, EventArgs e)
        {
            pictureLogo.Focus();

            MemoryExists = true;
            memory -= Convert.ToDouble(Output);
        }

        //When form is closing
        private void CalculatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save data to .txt file
            SetSaveData();
        }

        //Allow keyboard input
        private void CalculatorForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 48)
            {
                btn0.PerformClick();
            }
            else if (e.KeyChar == 49)
            {
                btn1.PerformClick();
            }
            else if (e.KeyChar == 50)
            {
                btn2.PerformClick();
            }
            else if (e.KeyChar == 51)
            {
                btn3.PerformClick();
            }
            else if (e.KeyChar == 52)
            {
                btn4.PerformClick();
            }
            else if (e.KeyChar == 53)
            {
                btn5.PerformClick();
            }
            else if (e.KeyChar == 54)
            {
                btn6.PerformClick();
            }
            else if (e.KeyChar == 55)
            {
                btn7.PerformClick();
            }
            else if (e.KeyChar == 56)
            {
                btn8.PerformClick();
            }
            else if (e.KeyChar == 57)
            {
                btn9.PerformClick();
            }
            else if (e.KeyChar == 08)
            {
                btnBackspace.PerformClick();
            }
            else if (e.KeyChar == 46)
            {
                btnDecimal.PerformClick();
            }
            else if (e.KeyChar == 99 || e.KeyChar == 67)
            {
                btnClear.PerformClick();
            }
            else if (e.KeyChar == 61 || e.KeyChar == 13)
            {
                btnEquals.PerformClick();
            }
            else if (e.KeyChar == 47)
            {
                btnDivide.PerformClick();
            }
            else if (e.KeyChar == 45)
            {
                btnSubtract.PerformClick();
            }
            else if (e.KeyChar == 42)
            {
                btnMultiply.PerformClick();
            }
            else if (e.KeyChar == 43)
            {
                btnAdd.PerformClick();
            }
        }
    }
}

