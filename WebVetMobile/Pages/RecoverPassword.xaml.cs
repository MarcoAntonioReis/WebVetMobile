
using System.ComponentModel.DataAnnotations;
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;
namespace WebVetMobile.Pages;

public partial class RecoverPassword : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    public RecoverPassword(ApiService apiService, IValidator validator)
    {

        _apiService = apiService;
        _validator = validator;
        InitializeComponent();
    }

    private async void BtnSend_Clicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(EntEmail.Text))
        {
            string email = EntEmail.Text;
            var (result, error) = await _apiService.RecoverPassword(email);

            if (result == true) {
                await DisplayAlert("Sent", "An email has been sent to your email with instructions, if this address was found!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Something went wrong", "OK");
            }
        }
    }
}