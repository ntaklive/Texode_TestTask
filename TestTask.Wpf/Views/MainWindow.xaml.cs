using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TestTask.Shared.Extensions;
using TestTask.Wpf.Extensions;
using TestTask.Wpf.ViewModels;

namespace TestTask.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMainWindowViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        private void AddNewButton_OnClick(object sender, RoutedEventArgs e)
        {
            UncheckAllCardRadioButtons();
        }

        private void SortTypeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UncheckAllCardRadioButtons();
        }

        private void UncheckAllCardRadioButtons()
        {
            IEnumerable<RadioButton> cardRadioButtons = CardRadioButtonsItemsControl.GetChildrenOfType<RadioButton>();
            cardRadioButtons.ForEach(button => button.IsChecked = false);
        }
    }
}