using Microsoft.Maui.Controls;
using QuizApplication.Models;

namespace QuizApplication.Views
{
    public partial class QuizPage : ContentPage
    {
        private QuizViewModel ViewModel => (QuizViewModel)BindingContext;

        public QuizPage(QuizViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }


        private void OnOptionSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedOptionIndex = ((ListView)sender).ItemsSource.Cast<string>().ToList().IndexOf((string)e.SelectedItem);
                ViewModel.CheckAnswerCommand.Execute(selectedOptionIndex);

                
                ((ListView)sender).SelectedItem = null;
            }
        }

    }
}