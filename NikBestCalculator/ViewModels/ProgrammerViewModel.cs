using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace NikBestCalculator.Views
{
    public class ProgrammerViewModel : INotifyPropertyChanged
    {
        private string _display = "0";
        private string _currentInput = "";
        private string _operation = "";
        private long _firstNumber = 0;
        private bool _isNewInput = true;
        private int _baseMode = 10;

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

        // === Цифры ===
        public ICommand NumberCommand => new RelayCommand(parameter =>
        {
            string digit = parameter.ToString();

            if (_baseMode == 10 && !char.IsDigit(digit[0]))
            {
                Display = "Ошибка: только цифры";
                return;
            }

            if (_baseMode == 8 && digit[0] >= '8')
            {
                Display = "Ошибка: OCT только 0-7";
                return;
            }

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

        // === HEX буквы ===
        public ICommand HexDigitCommand => new RelayCommand(parameter =>
        {
            string digit = parameter.ToString().ToUpper();

            if (_baseMode != 16)
            {
                Display = "Ошибка: HEX доступен только в режиме HEX";
                return;
            }

            if (!Uri.IsHexDigit(digit[0]))
            {
                Display = "Ошибка: только HEX символы (0-9, A-F)";
                return;
            }

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

        // === Операции (AND, OR, XOR, NOT, сдвиги) — пока TODO ===
        public ICommand OperationCommand => new RelayCommand(parameter =>
        {
            string op = parameter.ToString();
            if (!string.IsNullOrEmpty(_operation) && !_isNewInput)
            {
                // TODO: реализовать вычисление для программистского режима
            }
            _firstNumber = long.Parse(_currentInput);
            _operation = op;
            _isNewInput = true;
        });

        // === Равно ===
        public ICommand EqualsCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_currentInput))
            {
                Display = "0";
                return;
            }

            try
            {
                long value = 0;

                if (_currentInput.StartsWith("0b", StringComparison.OrdinalIgnoreCase))
                {
                    string binary = _currentInput.Substring(2);
                    value = Convert.ToInt64(binary, 2);
                }
                else if (_currentInput.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    string hex = _currentInput.Substring(2);
                    value = Convert.ToInt64(hex, 16);
                }
                else if (_currentInput.StartsWith("0o", StringComparison.OrdinalIgnoreCase))
                {
                    string oct = _currentInput.Substring(2);
                    value = Convert.ToInt64(oct, 8);
                }
                else
                {
                    value = long.Parse(_currentInput);
                }

                History.Add($"{_currentInput} = {value}");
                _currentInput = value.ToString();
                Display = _currentInput;
                _isNewInput = true;
            }
            catch
            {
                Display = "Ошибка";
                _currentInput = "";
            }
        });

        // === Очистка ===
        public ICommand ClearCommand => new RelayCommand(parameter =>
        {
            _currentInput = "";
            _firstNumber = 0;
            _operation = "";
            _isNewInput = true;
            Display = "0";
        });

        // === Системы счисления ===
        public ICommand SetDecimalCommand => new RelayCommand(p =>
        {
            _baseMode = 10;
            ConvertCurrentInput();
        });

        public ICommand SetHexCommand => new RelayCommand(p =>
        {
            _baseMode = 16;
            ConvertCurrentInput();
        });

        public ICommand SetBinaryCommand => new RelayCommand(p =>
        {
            _baseMode = 2;
            ConvertCurrentInput();
        });
        
        public ICommand BackspaceCommand => new RelayCommand(parameter =>
        {
            if (string.IsNullOrEmpty(_currentInput))
                return;

            _currentInput = _currentInput.Length > 1 
                ? _currentInput.Substring(0, _currentInput.Length - 1) 
                : "";

            Display = string.IsNullOrEmpty(_currentInput) ? "0" : _currentInput;
        });

        public ICommand SetOctalCommand => new RelayCommand(p =>
        {
            _baseMode = 8;
            ConvertCurrentInput();
        });

        private void ConvertCurrentInput()
        {
            if (string.IsNullOrEmpty(_currentInput))
            {
                Display = "0";
                return;
            }

            try
            {
                long value = long.Parse(_currentInput);

                _currentInput = _baseMode switch
                {
                    10 => value.ToString(),
                    16 => "0x" + value.ToString("X"),
                    2 => "0b" + Convert.ToString(value, 2),
                    8 => "0o" + Convert.ToString(value, 8),
                    _ => value.ToString()
                };
                Display = _currentInput;
            }
            catch
            {
                Display = "Ошибка";
                _currentInput = "";
            }
        }

        // === Побитовые операции (пока заглушки) ===
        // === Операции (AND, OR, XOR, NOT, сдвиги) — пока заглушки ===
        public ICommand AndCommand => new RelayCommand(p => { Display = "AND (TODO)"; });
        public ICommand OrCommand => new RelayCommand(p => { Display = "OR (TODO)"; });
        public ICommand XorCommand => new RelayCommand(p => { Display = "XOR (TODO)"; });
        public ICommand NotCommand => new RelayCommand(p => { Display = "NOT (TODO)"; });
        public ICommand LeftShiftCommand => new RelayCommand(p => { Display = "<< (TODO)"; });
        public ICommand RightShiftCommand => new RelayCommand(p => { Display = ">> (TODO)"; });

        // === История ===
        public ICommand ClearHistoryCommand => new RelayCommand(p => History.Clear());
        public ICommand RemoveHistoryItemCommand => new RelayCommand(p =>
        {
            if (p is string item)
                History.Remove(item);
        });
    }
}