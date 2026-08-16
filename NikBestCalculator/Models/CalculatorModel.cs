namespace NikBestCalculator
{
    public class CalculatorModel
    {
        public string CurrentInput { get; private set; } = "0";
        public double FirstNumber { get; private set; }
        public string Operation { get; private set; } = "";
        public bool IsNewInput { get; private set; } = true;
        public bool IsError { get; private set; } = false;

        public void AddDigit(string digit)
        {
            if (IsError) Reset();

            if (IsNewInput)
            {
                CurrentInput = digit;
                IsNewInput = false;
            }
            else
            {
                CurrentInput += digit;
            }
        }

        public void AddDecimal()
        {
            if (IsError) Reset();

            if (!CurrentInput.Contains(","))
            {
                if (IsNewInput)
                {
                    CurrentInput = "0,";
                    IsNewInput = false;
                }
                else
                {
                    CurrentInput += ",";
                }
            }
        }

        public void SetOperation(string op)
        {
            if (IsError) Reset();

            if (!string.IsNullOrEmpty(Operation) && !IsNewInput)
            {
                Calculate();
            }

            FirstNumber = Convert.ToDouble(CurrentInput);
            Operation = op;
            IsNewInput = true;
        }

        public void Calculate()
        {
            if (IsError) return;

            double secondNumber = Convert.ToDouble(CurrentInput);
            double result = 0;

            switch (Operation)
            {
                case "+": result = FirstNumber + secondNumber; break;
                case "-": result = FirstNumber - secondNumber; break;
                case "*": result = FirstNumber * secondNumber; break;
                case "/":
                    if (secondNumber == 0)
                    {
                        CurrentInput = "Деление на ноль";
                        IsError = true;
                        return;
                    }
                    result = FirstNumber / secondNumber;
                    break;
                default:
                    return;
            }

            CurrentInput = result.ToString();
            Operation = "";
            IsNewInput = true;
        }

        public void Reset()
        {
            CurrentInput = "0";
            FirstNumber = 0;
            Operation = "";
            IsNewInput = true;
            IsError = false;
        }

        public void Clear()
        {
            Reset();
        }
    }
}