using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data;

namespace NikBestCalculator.Views
{
    public class EngineerViewModel : INotifyPropertyChanged
    {
        private string _display = "0";
        private string _expression = "";
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

        // === Проверка последнего символа ===
        private bool IsLastCharValidForOperation()
        {
            if (string.IsNullOrEmpty(_expression)) return false;
            char last = _expression[_expression.Length - 1];
            return char.IsDigit(last) || last == ')' || last == 'π' || last == 'e';
        }

        // === Цифры ===
        public ICommand NumberCommand => new RelayCommand(parameter =>
        {
            string digit = parameter.ToString();
            if (_isNewInput)
            {
                _expression = digit;
                _isNewInput = false;
            }
            else
            {
                _expression += digit;
            }
            Display = _expression;
        });

        // === Десятичная запятая ===
        public ICommand DecimalCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_expression))
            {
                _expression = "0,";
                _isNewInput = false;
            }
            else if (!_expression.EndsWith(","))
            {
                _expression += ",";
            }
            Display = _expression;
        });

        // === Операции ===
        public ICommand OperationCommand => new RelayCommand(parameter =>
        {
            if (!IsLastCharValidForOperation()) return;
            string op = parameter.ToString();
            _expression += " " + op + " ";
            _isNewInput = false;
            Display = _expression;
        });

        // === Скобки ===
        public ICommand LeftBracketCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0")
            {
                _expression = "(";
            }
            else
            {
                _expression += "(";
            }
            _isNewInput = false;
            Display = _expression;
        });

        public ICommand RightBracketCommand => new RelayCommand(parameter =>
        {
            _expression += ")";
            _isNewInput = false;
            Display = _expression;
        });

        // === Тригонометрия ===
        public ICommand SinCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = _isRadian ? Math.Sin(value) : Math.Sin(value * Math.PI / 180);
            History.Add($"sin({_expression}) = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand CosCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = _isRadian ? Math.Cos(value) : Math.Cos(value * Math.PI / 180);
            History.Add($"cos({_expression}) = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand TanCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = _isRadian ? Math.Tan(value) : Math.Tan(value * Math.PI / 180);
            History.Add($"tan({_expression}) = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        // === Логарифмы ===
        public ICommand LogCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = Math.Log10(value);
            History.Add($"log10({_expression}) = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand LnCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = Math.Log(value);
            History.Add($"ln({_expression}) = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        // === Степени ===
        public ICommand SquareCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = value * value;
            History.Add($"{_expression}² = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand CubeCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = value * value * value;
            History.Add($"{_expression}³ = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand SqrtCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            double value = double.Parse(_expression);
            double result = Math.Sqrt(value);
            History.Add($"√{_expression} = {result}");
            _expression = result.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        // === Обратное число (1/x) ===
        public ICommand InvertCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "Ошибка";
                return;
            }

            try
            {
                double value = double.Parse(_expression.Replace(",", "."));
                if (value == 0)
                {
                    Display = "Ошибка";
                    return;
                }
                double result = 1 / value;
                History.Add($"1/({_expression}) = {result}");
                _expression = result.ToString().Replace(".", ",");
                Display = _expression;
                _isNewInput = true;
            }
            catch
            {
                Display = "Ошибка";
                _expression = "";
            }
        });
        
        public ICommand BackspaceCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_expression))
                return;

            _expression = _expression.Length > 1 
                ? _expression.Substring(0, _expression.Length - 1) 
                : "";

            Display = string.IsNullOrEmpty(_expression) ? "0" : _expression;
        });
        
        public ICommand ZeroCommand => new RelayCommand(parameter =>
        {
            if (_isNewInput)
            {
                _expression = "0";
                _isNewInput = false;
            }
            else
            {
                _expression += "0";
            }
            Display = _expression;
        });

        public ICommand FactorialCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression) || _expression == "0" || _expression == "0,")
            {
                Display = "0";
                return;
            }
            if (string.IsNullOrEmpty(_expression)) return;
            int value = int.Parse(_expression);
            if (value < 0)
            {
                Display = "Ошибка";
                return;
            }
            long result = 1;
            for (int i = 2; i <= value; i++)
                result *= i;
            History.Add($"{_expression}! = {result}");
            _expression = result.ToString();
            Display = _expression;
            _isNewInput = true;
        });

        // === Константы ===
        public ICommand PiCommand => new RelayCommand(p =>
        {
            _expression = Math.PI.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        public ICommand ECommand => new RelayCommand(p =>
        {
            _expression = Math.E.ToString().Replace(".", ",");
            Display = _expression;
            _isNewInput = true;
        });

        // === Переключение режима углов ===
        public ICommand ToggleAngleModeCommand => new RelayCommand(p =>
        {
            _isRadian = !_isRadian;
            Display = _isRadian ? "Rad" : "Deg";
        });

        // === Возведение в степень (x^y) ===
        public ICommand PowerCommand => new RelayCommand(p =>
        {
            if (string.IsNullOrEmpty(_expression)) return;
            _expression += " ^ ";
            _isNewInput = false;
            Display = _expression;
        });

        // === Равно ===
        public ICommand EqualsCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_expression))
            {
                Display = "0";
                return;
            }

            try
            {
                string expr = _expression.Replace(",", ".").Replace("^", "**");
                var result = new DataTable().Compute(expr, null);
                string resultStr = result.ToString().Replace(".", ",");
                History.Add($"{_expression} = {resultStr}");
                _expression = resultStr;
                Display = _expression;
                _isNewInput = true;
            }
            catch
            {
                Display = "Ошибка";
                _expression = "";
            }
        });

        // === Очистка ===
        public ICommand ClearCommand => new RelayCommand(parameter =>
        {
            _expression = "";
            _isNewInput = true;
            Display = "0";
        });

        // === История ===
        public ICommand ClearHistoryCommand => new RelayCommand(p => History.Clear());
        public ICommand RemoveHistoryItemCommand => new RelayCommand(p =>
        {
            if (p is string item)
                History.Remove(item);
        });
    }
}