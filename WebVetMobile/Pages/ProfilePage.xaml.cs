
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
    private Guid imageId;
        private User user = new User();

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
            ImgBtnPerfil.Source = currentUser.ImageFullPath;


        }

        if (currentUser!=null)
        {
            user = currentUser;
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
        if (imageId != Guid.Empty)
        {
            user.ImageId = imageId;
        }

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

    private async void ImgBtnPerfil_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imagemArray = await SelectImageAsync();
            if (imagemArray is null)
            {
                await DisplayAlert("Error", "The image has not sent", "Ok");
                return;
            }
            ImgBtnPerfil.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

            var response = await _apiService.UploadUserImage(imagemArray);
            if (response.Data!=Guid.Empty)
            {
                imageId= response.Data;
                User tempUser= new User
                {
                    ImageId = imageId,
                };

                ImgBtnPerfil.Source=tempUser.ImageFullPath;

               await DisplayAlert("", "The image has sent, to finalize press save ", "Ok");
            }
            else
            {
                await DisplayAlert("Error", response.ErrorMessage ?? "Something went wrong", "Cancel");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "Ok");
        }
    }

    private async Task<byte[]?> SelectImageAsync()
    {
        try
        {
            var archive = await MediaPicker.PickPhotoAsync();

            if (archive is null) return null;

            using (var stream = await archive.OpenReadAsync())
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Error", "The functions is not supported", "Ok");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Error", "Does not have permissions", "Ok");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error Selecting the image: {ex.Message}", "Ok");
        }
        return null;
    }
    
    
}