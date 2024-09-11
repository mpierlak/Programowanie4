using QuizApplication.Models; 
using Microsoft.Maui.Controls;


namespace QuizApplication.Views
{
    public partial class CategoriesPage : ContentPage
    {
        private QuizViewModel ViewModel => (QuizViewModel)BindingContext;

        public CategoriesPage()
        {
            InitializeComponent();
            BindingContext = new QuizViewModel();
        }

        private async void OnCategorySelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedCategory = (Category)e.SelectedItem;
                ViewModel.SelectCategoryCommand.Execute(selectedCategory);

                
                await Navigation.PushAsync(new QuizPage(ViewModel));
            }
        }
    }
    }
