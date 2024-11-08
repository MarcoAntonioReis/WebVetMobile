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
}