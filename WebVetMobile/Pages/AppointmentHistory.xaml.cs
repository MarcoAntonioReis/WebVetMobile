using System.ComponentModel.DataAnnotations;
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class AppointmentHistory : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;
    public AppointmentHistory(ApiService apiService, IValidator validator)
	{
        _apiService = apiService;
        _validator = validator;
        InitializeComponent();
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetAppointmentHistory();
    }

    private async Task<IEnumerable<AppointmentApi>> GetAppointmentHistory()
    {
        
        try
        {
            var (appointmentsResult, errorMessage) = await _apiService.GetAppoimtments(Preferences.Get("UserName", string.Empty));
            if (errorMessage == "Unauthorized" && _loginPageDisplayed)
            {
                await DisplayLoginPage();
                return Enumerable.Empty<AppointmentApi>();
            }

            if (appointmentsResult is null)
            {
                await DisplayAlert("Error", errorMessage ?? "It was not possible to get history.", "Ok");
                return Enumerable.Empty<AppointmentApi>();
            }
            CvAppointments.ItemsSource = appointmentsResult;
            return appointmentsResult;

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Unexpected error:{ex.Message}", "Ok");
            return Enumerable.Empty<AppointmentApi>();
        }


        
    }
    private async Task DisplayLoginPage()
    {
        _loginPageDisplayed = true;
        await Navigation.PushAsync(new LoginPage(_apiService, _validator));
    }

    private void CvAppointments_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var currentSelection = e.CurrentSelection.FirstOrDefault() as AppointmentApi;

        if (currentSelection == null)
        {
            return;
        }

        Navigation.PushAsync(new DetailsPage(currentSelection, _apiService, _validator));
        ((CollectionView)sender).SelectedItem = null;

    }

  
}