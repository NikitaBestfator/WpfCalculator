using System.Windows;
using NikBestCalculator.Views;

namespace NikBestCalculator
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // По умолчанию показываем стандартный режим
            ModeContent.Content = new StandardView();
        }

        private void StandardMode_Click(object sender, RoutedEventArgs e)
        {
            ModeContent.Content = new StandardView();
        }

        private void EngineerMode_Click(object sender, RoutedEventArgs e)
        {
            ModeContent.Content = new EngineerView();
        }

        private void ProgrammerMode_Click(object sender, RoutedEventArgs e)
        {
            ModeContent.Content = new ProgrammerView();
        }
    }
}