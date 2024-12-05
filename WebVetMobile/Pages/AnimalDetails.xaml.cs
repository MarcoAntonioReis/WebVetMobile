
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class AnimalDetails : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private Animal _animal;

    public AnimalDetails(Animal animal, ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;

        _animal= animal;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAnimalData();

    }

    private async Task LoadAnimalData()
    {

        if (_animal!=null)
        {
            EntName.Text = _animal.Name;
            EntSpecies.Text = _animal.Species;
        }
        


    }


    private void BtnSave_Clicked(object sender, EventArgs e)
    {
        Animal animalUpdated= new Animal
        {
            Id = _animal.Id,
            Name = _animal.Name,
            Species = _animal.Species
        };




    }
}