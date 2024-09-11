using System.Windows.Input;
using Microsoft.Maui.Controls;
using QuizApplication.Views;

namespace QuizApplication.ViewModels
{
    public class WelcomeViewModel : BaseViewModel
    {
        public ICommand StartQuizCommand { get; }

        public WelcomeViewModel()
        {
            StartQuizCommand = new Command(StartQuiz);
        }

        private async void StartQuiz()
        {
            
            await Application.Current.MainPage.Navigation.PushAsync(new CategoriesPage());
        }
    }
}
