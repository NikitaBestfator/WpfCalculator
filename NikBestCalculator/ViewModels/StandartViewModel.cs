using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace NikBestCalculator
{
    public class StandardViewModel : INotifyPropertyChanged
    {
        private string _display = "0";
        private string _currentInput = "";
        private string _operation = "";
        private double _firstNumber = 0;
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

        // Команды для кнопок
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

        public ICommand ClearHistoryCommand => new RelayCommand(p => History.Clear());
        public ICommand RemoveHistoryItemCommand => new RelayCommand(p =>
        {
            if (p is string item)
                History.Remove(item);
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