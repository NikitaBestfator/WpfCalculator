using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace NikBestCalculator.Views
{
    public class EngineerViewModel : INotifyPropertyChanged
    {
        private string _display = "0";
        private string _currentInput = "";
        private string _operation = "";
        private double _firstNumber = 0;
        private bool _isNewInput = true;
        private bool _isRadian = true;

        public ObservableCollection<string> History { get; } = new ObservableCollection<string>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Display
        {
            get => _display;
            set
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
            }
        }

        public ICommand ClearHistoryCommand => new RelayCommand(p => History.Clear());
        public ICommand RemoveHistoryItemCommand => new RelayCommand(p =>
        {
            if (p is string item)
                History.Remove(item);
        });

        public ICommand NumberCommand => new RelayCommand(parameter =>
        {
            string digit = parameter.ToString();
            if (_isNewInput)
            {
                _currentInput = digit;
                _isNewInput = false;
            }
            else
            {
                _currentInput += digit;
            }
            Display = _currentInput;
        });

        public ICommand DecimalCommand => new RelayCommand(parameter =>
        {
            if (!_currentInput.Contains(","))
            {
                if (_isNewInput)
                {
                    _currentInput = "0,";
                    _isNewInput = false;
                }
                else
                {
                    _currentInput += ",";
                }
                Display = _currentInput;
            }
        });

        public ICommand OperationCommand => new RelayCommand(parameter =>
        {
            string op = parameter.ToString();
            if (!string.IsNullOrEmpty(_operation) && !_isNewInput)
                Calculate();
            if (!string.IsNullOrEmpty(_currentInput))
                _firstNumber = double.Parse(_currentInput);
            _operation = op;
            _isNewInput = true;
        });

        public ICommand EqualsCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_currentInput))
            {
                Display = "0";
                return;
            }
            Calculate();
            _operation = "";
            _isNewInput = true;
        });

        public ICommand ClearCommand => new RelayCommand(parameter =>
        {
            _currentInput = "";
            _operation = "";
            _firstNumber = 0;
            _isNewInput = true;
            Display = "0";
        });

        public ICommand SinCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = _isRadian ? Math.Sin(value) : Math.Sin(value * Math.PI / 180);
            History.Add($"sin({_currentInput}) = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand CosCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = _isRadian ? Math.Cos(value) : Math.Cos(value * Math.PI / 180);
            History.Add($"cos({_currentInput}) = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand TanCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = _isRadian ? Math.Tan(value) : Math.Tan(value * Math.PI / 180);
            History.Add($"tan({_currentInput}) = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand LogCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = Math.Log10(value);
            History.Add($"log10({_currentInput}) = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand LnCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = Math.Log(value);
            History.Add($"ln({_currentInput}) = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand SquareCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = value * value;
            History.Add($"{_currentInput}² = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand CubeCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = value * value * value;
            History.Add($"{_currentInput}³ = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand SqrtCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            double result = Math.Sqrt(value);
            History.Add($"√{_currentInput} = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand InvertCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            double value = double.Parse(_currentInput);
            if (value == 0)
            {
                Display = "Ошибка";
                return;
            }
            double result = 1 / value;
            History.Add($"1/{_currentInput} = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand FactorialCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            int value = int.Parse(_currentInput);
            if (value < 0)
            {
                Display = "Ошибка";
                return;
            }
            long result = 1;
            for (int i = 2; i <= value; i++)
                result *= i;
            History.Add($"{_currentInput}! = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand PiCommand => new RelayCommand(p =>
        {
            _currentInput = Math.PI.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand ECommand => new RelayCommand(p =>
        {
            _currentInput = Math.E.ToString();
            Display = _currentInput;
            _isNewInput = true;
        });

        public ICommand ToggleAngleModeCommand => new RelayCommand(p =>
        {
            _isRadian = !_isRadian;
            Display = _isRadian ? "Rad" : "Deg";
        });

        public ICommand PowerCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_currentInput)) return;
            _firstNumber = double.Parse(_currentInput);
            _operation = "^";
            _isNewInput = true;
        });

        private void Calculate()
        {
            if (string.IsNullOrEmpty(_currentInput))
            {
                Display = "0";
                return;
            }

            double secondNumber = double.Parse(_currentInput);
            double result = 0;

            switch (_operation)
            {
                case "+": result = _firstNumber + secondNumber; break;
                case "-": result = _firstNumber - secondNumber; break;
                case "*": result = _firstNumber * secondNumber; break;
                case "/":
                    if (secondNumber == 0)
                    {
                        Display = "Ошибка";
                        ClearAll();
                        return;
                    }
                    result = _firstNumber / secondNumber;
                    break;
                case "^":
                    result = Math.Pow(_firstNumber, secondNumber);
                    break;
                default:
                    return;
            }

            History.Add($"{_firstNumber} {_operation} {secondNumber} = {result}");
            _currentInput = result.ToString();
            Display = _currentInput;
        }

        private void ClearAll()
        {
            _currentInput = "";
            _operation = "";
            _firstNumber = 0;
            _isNewInput = true;
        }
    }
}