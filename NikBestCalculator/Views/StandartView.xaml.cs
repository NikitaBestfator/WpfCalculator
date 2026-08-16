using System.Windows.Controls;
using System.Windows.Input;

namespace NikBestCalculator.Views
{
    public partial class StandardView : UserControl
    {
        public StandardView()
        {
            InitializeComponent();
            DataContext = new StandardViewModel();

            this.Focusable = true;
            this.Loaded += (s, e) => this.Focus();

            this.KeyDown += (sender, e) =>
            {
                var vm = DataContext as StandardViewModel;
                if (vm == null) return;

                // ============================
                // 1. РАВНО (Enter)
                // ============================
                if (e.Key == Key.Enter || e.Key == Key.Return)
                {
                    vm.EqualsCommand.Execute(null);
                    e.Handled = true;
                    return;
                }

                // ============================
                // 2. ЦИФРЫ (основная + NumPad)
                // ============================
                if (e.Key >= Key.D0 && e.Key <= Key.D9)
                    vm.NumberCommand.Execute((e.Key - Key.D0).ToString());
                else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
                    vm.NumberCommand.Execute((e.Key - Key.NumPad0).ToString());

                // ============================
                // 3. ОПЕРАЦИИ (основная + NumPad)
                // ============================
                else if (e.Key == Key.Add) vm.OperationCommand.Execute("+");
                else if (e.Key == Key.Subtract) vm.OperationCommand.Execute("-");
                else if (e.Key == Key.Multiply) vm.OperationCommand.Execute("*");
                else if (e.Key == Key.Divide) vm.OperationCommand.Execute("/");

                // ============================
                // 4. ОЧИСТКА (Backspace, Escape)
                // ============================
                else if (e.Key == Key.Back || e.Key == Key.Escape)
                    vm.ClearCommand.Execute(null);
            };
        }
        private void HistoryItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Проверяем, что это был двойной клик
            if (e.ClickCount == 2)
            {
                var border = sender as Border;
                if (border?.DataContext is string item)
                {
                    dynamic vm = DataContext;
                    vm?.RemoveHistoryItemCommand.Execute(item);
                }
                e.Handled = true; // Чтобы событие не пошло дальше
            }
        }
    }
}