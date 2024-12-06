using System.ComponentModel.DataAnnotations;
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;
namespace WebVetMobile.Pages;

public partial class ChangePassword : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    public ChangePassword(ApiService apiService, IValidator validator)
    {
        _apiService = apiService;
        _validator = validator;
        InitializeComponent();
    }

    private async void BtnChange_Clicked(object sender, EventArgs e)
    {

        string oldPassword = EntOldPassword.Text;
        string newPassword = EntNewPassword.Text;
        string confPassword = EntConfPassword.Text;


        if (!string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(newPassword) && !string.IsNullOrEmpty(confPassword))
        {
            if (oldPassword.Length >= 8 && newPassword.Length >= 8 && confPassword.Length >= 8)
            {
                ChangePasswordApi changePassword = new ChangePasswordApi
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    Confirm = confPassword
                };

                var (result, error) = await _apiService.ChangePassword(changePassword);

                if (result==true)
                {
                    await DisplayAlert("Success", "The password was changed", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", "The password was not changed, check if it has at least 8 characters and both letters and numbers  ", "Ok");
                }


            }
            else
            {
                await DisplayAlert("Error", "Must have at least 8 character", "Cancel");
            }
        }
        else
        {
            await DisplayAlert("Error", "Insert all fields", "Ok");
        }
    }
}