using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class ProfilePage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private bool _loginPageDisplayed = false;

    private const string UserId = "UserId";


    public ProfilePage(ApiService apiService, IValidator validator)
    {
        InitializeComponent();

        _apiService = apiService;
        _validator = validator;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadUserData();

    }


    private async Task LoadUserData()
    {
        User currentUser = await _apiService.GetCurrentUserInfo();
        if (currentUser != null)
        {
            EntFirstName.Text = currentUser.FirstName;
            EntLastName.Text = currentUser.LastName;
            EntEmail.Text = currentUser.Email;
            EntContact.Text = currentUser.PhoneNumber.ToString();
            EntAddress.Text = currentUser.Address;

        }


    }


    private async void BtnSave_Clicked(object sender, EventArgs e)
    {
        User user = new User
        {
            Id = Preferences.Get(UserId, string.Empty),
            FirstName = EntFirstName.Text,
            LastName = EntLastName.Text,
            Address = EntAddress.Text,
            PhoneNumber = EntContact.Text
        };

        var response = await _apiService.UpdateUser(user);

        if (response.Data==true)
        {
            await DisplayAlert("Success", "Update has successful!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "The update failed", "OK");
        }


    }

    private async void BtnChangePassword_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ChangePassword(_apiService, _validator));
    }
}