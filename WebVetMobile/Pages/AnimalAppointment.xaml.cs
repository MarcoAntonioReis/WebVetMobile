
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class AnimalAppointment : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private Animal _animal;

    public AnimalAppointment(Animal animal, ApiService apiService, IValidator validator)
    {
        _apiService = apiService;
        _validator = validator;
        _animal = animal;
        InitializeComponent();
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetAnimalAppointmentHistory();
    }
    private async Task<IEnumerable<AppointmentApi>> GetAnimalAppointmentHistory()
    {
        try
        {
            var (appointmentsResult, errorMessage) = await _apiService.GetAnimalAppointments(_animal.Id);


            if (appointmentsResult is null)
            {
                await DisplayAlert("Error", errorMessage ?? "It was not possible to get history.", "Ok");
                return Enumerable.Empty<AppointmentApi>();
            }

            CvAnimalApp.ItemsSource = appointmentsResult;
            return appointmentsResult;

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Unexpected error:{ex.Message}", "Ok");
            return Enumerable.Empty<AppointmentApi>();
        }
    }

    private void CvAnimalApp_SelectionChanged(object sender, SelectionChangedEventArgs e)
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