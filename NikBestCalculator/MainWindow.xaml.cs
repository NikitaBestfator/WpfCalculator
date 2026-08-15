using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NikBestCalculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private string currentInput = "";
        private string operation = "";
        private double firstNumber = 0;
        private bool isNewInput = true;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string digit = btn.Content.ToString();

            if (isNewInput)
            {
                currentInput = digit;
                isNewInput = false;
            }
            else
            {
                currentInput += digit;
            }

            UpdateDisplay();
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            if (!currentInput.Contains(","))
            {
                if (isNewInput)
                {
                    currentInput = "0,";
                    isNewInput = false;
                }
                else
                {
                    currentInput += ",";
                }

                UpdateDisplay();
            }
        }

        private void OperationButton_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            string op = btn.Content.ToString();

            if (!string.IsNullOrEmpty(operation) && !isNewInput)
                CalculateResult();

            firstNumber = Convert.ToDouble(currentInput);
            operation = op;
            isNewInput = true;
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            CalculateResult();
            operation = "";
            isNewInput = true;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            currentInput = "";
            operation = "";
            firstNumber = 0;
            isNewInput = true;
            DisplayTextBox.Text = "0";
        }

        private void CalculateResult()
        {
            double secondNumber = Convert.ToDouble(currentInput);
            double result = 0;

            switch (operation)
            {
                case "+": result = firstNumber + secondNumber; break;
                case "-": result = firstNumber - secondNumber; break;
                case "*": result = firstNumber * secondNumber; break;
                case "/":
                    if (secondNumber == 0)
                    {
                        DisplayTextBox.Text = "Ошибка";
                        ClearAll();
                        return;
                    }

                    result = firstNumber / secondNumber;
                    break;
                default:
                    return;
            }

            currentInput = result.ToString();
            UpdateDisplay();
        }

        private void UpdateDisplay() => DisplayTextBox.Text = currentInput;

        private void ClearAll()
        {
            currentInput = "";
            operation = "";
            firstNumber = 0;
            isNewInput = true;
        }
}