
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;
namespace WebVetMobile.Pages;

public partial class Animals : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;
    public Animals(ApiService apiService, IValidator validator)
	{
        _apiService = apiService;
        _validator = validator;
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetAnimalList();
    }

    private async Task<IEnumerable<Animal>> GetAnimalList()
    {
        try
        {
            var (animalResult, errorMessage) = await _apiService.GetUserAnimals();

            if (errorMessage == "Unauthorized" && _loginPageDisplayed)
            {
                await DisplayLoginPage();
                return Enumerable.Empty<Animal>();
            }

            if (animalResult is null)
            {
                await DisplayAlert("Error", errorMessage ?? "It was not possible to the animal list.", "Ok");
                return Enumerable.Empty<Animal>();
            }
            CvAnimals.ItemsSource = animalResult;
            return animalResult;

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Unexpected error:{ex.Message}", "Ok");
            return Enumerable.Empty<Animal>();
        }
    }

    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private void CvAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }
}