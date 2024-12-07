using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;
namespace WebVetMobile.Pages;

public partial class TeamPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    public TeamPage(ApiService apiService, IValidator validator)
	{
		InitializeComponent();
        _apiService = apiService;
        _validator = validator;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await GetTeamList();
    }

    private async Task<IEnumerable<DoctorDetails>> GetTeamList()
    {
        try
        {
            var (vetsResult, errorMessage) = await _apiService.GetDoctors();

       

            if (vetsResult is null)
            {
                await DisplayAlert("Error", errorMessage ?? "It was not possible to get the vets list.", "Ok");
                return Enumerable.Empty<DoctorDetails>();
            }
            CvVets.ItemsSource = vetsResult;
            return vetsResult;

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", $"Unexpected error:{ex.Message}", "Ok");
            return Enumerable.Empty<DoctorDetails>();
        }
    }


    private void BtnAbout_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new AboutPage());
    }
}