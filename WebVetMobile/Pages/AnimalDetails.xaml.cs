
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class AnimalDetails : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private Animal _animal;
    private Guid imageId;

    public AnimalDetails(Animal animal, ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;

        _animal = animal;
    }

    protected override  void OnAppearing()
    {
        base.OnAppearing();
         LoadAnimalData();

    }

    private void LoadAnimalData()
    {

        if (_animal != null)
        {
            EntName.Text = _animal.Name;
            EntSpecies.Text = _animal.Species;
            ImgBtnAnimal.Source = _animal.ImageFullPath;
        }



    }


    private async void BtnSave_Clicked(object sender, EventArgs e)
    {
        Animal animalUpdated = new Animal
        {
            Id = _animal.Id,
            Name = EntName.Text,
            Species = EntSpecies.Text
        };

        if (imageId != Guid.Empty)
        {
            animalUpdated.ImageId = imageId;
        }

        var response = await _apiService.UpdateAnimal(animalUpdated);

        if (response.Data == true)
        {
            await DisplayAlert("Success", "Update has successful!", "OK");
        }
        else
        {
            await DisplayAlert("Error", "The update failed", "OK");
        }



    }

    private async void BtnAnimalHistory_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AnimalAppointment(_animal, _apiService, _validator));
    }



    private async void ImgBtnAnimal_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imagemArray = await SelectImageAsync();
            if (imagemArray is null)
            {
                await DisplayAlert("Error", "The image has not sent", "Ok");
                return;
            }
            ImgBtnAnimal.Source = ImageSource.FromStream(() => new MemoryStream(imagemArray));

            var response = await _apiService.UploadAnimalImage(imagemArray);
            if (response.Data != Guid.Empty)
            {
                imageId = response.Data;


                _animal.ImageId = imageId;

                ImgBtnAnimal.Source = _animal.ImageFullPath;

                await DisplayAlert("", "The image has sent, to finalize press save ", "Ok");
            }
            else
            {
                await DisplayAlert("Error", response.ErrorMessage ?? "Something went wrong", "Cancel");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrongo: {ex.Message}", "Ok");
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