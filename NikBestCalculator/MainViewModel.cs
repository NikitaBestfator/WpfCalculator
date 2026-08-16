using System.Windows.Input;
// связывает модель с кнопками
namespace NikBestCalculator
{
    public class MainViewModel
    {
        private readonly CalculatorModel _model = new CalculatorModel();

        public string Display => _model.CurrentInput;

        public ICommand NumberCommand => new RelayCommand(parameter =>
        {
            _model.AddDigit(parameter.ToString());
            UpdateDisplay();
        });

        public ICommand DecimalCommand => new RelayCommand(parameter =>
        {
            _model.AddDecimal();
            UpdateDisplay();
        });

        public ICommand OperationCommand => new RelayCommand(parameter =>
        {
            _model.SetOperation(parameter.ToString());
            UpdateDisplay();
        });

        public ICommand EqualsCommand => new RelayCommand(parameter =>
        {
            _model.Calculate();
            UpdateDisplay();
        });

        public ICommand ClearCommand => new RelayCommand(parameter =>
        {
            _model.Clear();
            UpdateDisplay();
        });

        private void UpdateDisplay()
        {
            // Здесь вызываем событие обновления интерфейса
        }
    }
}