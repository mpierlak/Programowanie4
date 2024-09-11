using Microsoft.Maui.Controls;
using QuizApplication.Views;

namespace QuizApplication
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new WelcomePage());
        }
    }
}
