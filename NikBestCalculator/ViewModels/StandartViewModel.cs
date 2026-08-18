using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data;

namespace NikBestCalculator
{
    public class StandardViewModel : INotifyPropertyChanged
    {
        private string _display = "0";
        private string _expression = "";
        private bool _isNewInput = true;

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

        // === Вспомогательный метод: проверка, что последний символ — число или закрывающая скобка ===
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

        // === Операции (+, -, *, /) ===
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
                // Заменяем запятые на точки для DataTable
                string expr = _expression.Replace(",", ".");
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
        
        public ICommand BackspaceCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_expression))
                return;

            _expression = _expression.Length > 1 
                ? _expression.Substring(0, _expression.Length - 1) 
                : "";

            Display = string.IsNullOrEmpty(_expression) ? "0" : _expression;
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