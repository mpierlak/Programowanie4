using Microsoft.Maui.Controls;
using QuizApplication.ViewModels;

namespace QuizApplication.Views
{
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
            BindingContext = new WelcomeViewModel();
        }
    }
}
