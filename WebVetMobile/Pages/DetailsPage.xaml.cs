
using WebVetMobile.Models;
using WebVetMobile.Services;
using WebVetMobile.Validations;

namespace WebVetMobile.Pages;

public partial class DetailsPage : ContentPage
{
    private readonly ApiService _apiService;
    private readonly IValidator _validator;
    private AppointmentApi _appointment;

    public DetailsPage(AppointmentApi appointment, ApiService apiService, IValidator validator)
    {
        InitializeComponent();
        _apiService = apiService;
        _validator = validator;
        _appointment = appointment;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        GetAppointmentDetails();

    }

    private void GetAppointmentDetails()
    {
        if (_appointment != null)
        {
            LblSubject.Text = _appointment.Subject;
            LblDescription.Text = _appointment.Description;
            LblDate.Text = _appointment.ScheduleDate;
            LblTime.Text = _appointment.ScheduleTime;
            LblState.Text = _appointment.State;
        }
        else
        {
            DisplayAlert("Error", "Something went wrong", "OK");
        }

    }

    private async void BtnCancel_Clicked(object sender, EventArgs e)
    {
        var result = await _apiService.CancelAppointment(_appointment);


        if (result.Data == false)
        {
            await DisplayAlert("Canceled", result.ErrorMessage ?? "It was not possible to cancel the appointment .", "Ok");
        }
        else
        {
            await DisplayAlert("Canceled", "The appointment was canceled.", "Ok");
        }








    }
}