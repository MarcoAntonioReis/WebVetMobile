using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class LoginPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;

    public LoginPage(ApiService apiService, IValidator validator)
	{
        _apiService = apiService;
        _validator = validator;
        InitializeComponent();
	}

    private async void BtnSignIn_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(EntEmail.Text))
        {
            await DisplayAlert("Error", "Insert email", "Cancel");
            return;
        }

        if (string.IsNullOrEmpty(EntPassword.Text))
        {
            await DisplayAlert("Error", "Insert password", "Cancel");
            return;
        }

        var response = await _apiService.Login(EntEmail.Text, EntPassword.Text);

        if (!response.HasError)
        {
            Application.Current!.MainPage = new AppShell(_apiService, _validator);
        }
        else
        {
            await DisplayAlert("Error", "something went wrong", "Cancel");
        }
    }

    private void BtnTheTime_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new TeamPage(_apiService, _validator));
    }

    private void BtnRecover_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new RecoverPassword(_apiService, _validator));
    }
}