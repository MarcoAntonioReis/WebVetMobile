using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebVetMobile.Models;

namespace WebVetMobile.Services
{

    public class ApiService
    {

        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;

        JsonSerializerOptions _serializerOptions;

        public ApiService(HttpClient httpClient,
                  ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,

            };
        }


        public async Task<ApiResponse<bool>> Login(string username, string password)
        {
            try
            {
                var login = new LoginViewModel()
                {
                    Username = username,
                    Password = password
                };

                var json = JsonSerializer.Serialize(login, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Account/ApiLogin", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error in the request HTTP : {response.StatusCode}");
                    return new ApiResponse<bool>
                    {
                        ErrorMessage = $"Error in the request HTTP : {response.StatusCode}"
                    };
                }

                var jsonResult = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(jsonResult, _serializerOptions);

                Preferences.Set("accesstoken", result.AccessToken);
                Preferences.Set("UserId", result.UserId!);
                Preferences.Set("UserName", result.UserName);

                return new ApiResponse<bool> { Data = true };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro no login : {ex.Message}");
                return new ApiResponse<bool> { ErrorMessage = ex.Message };
            }
        }


        public async Task<User> GetCurrentUserInfo()
        {

            string endpoint = "api/Account/GetUserInfo";
            var (response, errorMessage) = await GetAsync<User>(endpoint);
            if (response != null)
            {
                return response;
            }
            else
            {
                _logger.LogError($"Error on get details : {errorMessage}");
                return null;
            }



        }

        public async Task<(List<Animal>? animals, string? ErrorMessage)> GetUserAnimals()
        {
            string endpoint = $"api/Animal/GetAnimalsByClientApi";
            return await GetAsync<List<Animal>>(endpoint);
        }

        public async Task<ApiResponse<bool>> UpdateUser(User user)
        {
            if (user!=null)
            {
                try
                {
                    var json = JsonSerializer.Serialize(user, _serializerOptions);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await PostRequest("api/Account/ApiUpdateUser", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Error in the request HTTP : {response.StatusCode}");
                        return new ApiResponse<bool>
                        {
                            ErrorMessage = $"Error in the request HTTP : {response.StatusCode}"
                        };
                    }
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<User>(jsonResult, _serializerOptions);


                    return new ApiResponse<bool> { Data = true };
                }
                catch(Exception ex)
                {
                    _logger.LogError($"Error on login : {ex.Message}");
                    return new ApiResponse<bool> { ErrorMessage = ex.Message };
                }
            }
            else
            {
                return new ApiResponse<bool> { ErrorMessage = "Invalid User" };
            }
        }




        public async Task<ApiResponse<bool>> UpdateAnimal(Animal animal)
        {
            if (animal != null)
            {
                try
                {
                    var json = JsonSerializer.Serialize(animal, _serializerOptions);

                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await PostRequest("api/Animal/ApiUpdateAnimal", content);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"Error in the request HTTP : {response.StatusCode}");
                        return new ApiResponse<bool>
                        {
                            ErrorMessage = $"Error in the request HTTP : {response.StatusCode}"
                        };
                    }
                    var jsonResult = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<Animal>(jsonResult, _serializerOptions);


                    return new ApiResponse<bool> { Data = true };
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error on login : {ex.Message}");
                    return new ApiResponse<bool> { ErrorMessage = ex.Message };
                }
            }
            else
            {
                return new ApiResponse<bool> { ErrorMessage = "Invalid Animal" };
            }
        }




        private async Task<HttpResponseMessage> PostRequest(string uri, HttpContent content)
        {
            var enderecoUrl = AppConfig.BaseUrl + uri;
            try
            {
                var result = await _httpClient.PostAsync(enderecoUrl, content);
                return result;
            }
            catch (Exception ex)
            {

                _logger.LogError($"Error in the request POST for {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        public async Task<(List<AppointmentApi>? appointments, string? ErrorMessage)> GetAppoimtments(string userEmail)
        {

            string endpoint = $"api/Owner/GetOwnerAppointmentList?userEmail={userEmail}";
            return await GetAsync<List<AppointmentApi>>(endpoint);

        }

        public async Task<(List<AppointmentApi>? appointments, string? ErrorMessage)> GetAnimalAppointments(int animalId)
        {

            string endpoint = $"api/Animal/ApiGetAnimalAppointments?id={animalId}";
            return await GetAsync<List<AppointmentApi>>(endpoint);

        }

        public async Task<(List<DoctorDetails>? doctors, string? ErrorMessage)> GetDoctors()
        {

            string endpoint = $"api/Doctor/GetDoctorsApi";
            return await GetAsync<List<DoctorDetails>>(endpoint);

        }

        public async Task<(bool Data, string? ErrorMessage)> RecoverPassword(string email)
        {

            string endpoint = $"api/Account/ApiRecoverPassword?email={email}";
            return await GetAsync<bool>(endpoint);

        }


        public async Task<(bool Data, string? ErrorMessage)> CancelAppointment(AppointmentApi appointment)
        {
            try
            {
                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
                var response = await PutRequest($"api/Appointment/CancelAppointments?id={appointment.Id}", content);
                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (false, errorMessage);
                    }
                    string generalErrorMessage = $"Erro na requisição: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (false, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"Erro de requisição HTTP: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Erro inesperado: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (false, errorMessage);
            }
        }



        public async Task<(bool Data, string? ErrorMessage)> ChangePassword(ChangePasswordApi changePassword)
        {
            try
            {
                var json = JsonSerializer.Serialize(changePassword, _serializerOptions);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await PostRequest("api/Account/ApiChangePassword", content);
                if (response.IsSuccessStatusCode)
                {
                    return (true, null);
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (false, errorMessage);
                    }
                    string generalErrorMessage = $"Error on task: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (false, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"Error on HTTP request: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (false, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Unexpected error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (false, errorMessage);
            }
        }

        private async Task<HttpResponseMessage> PutRequest(string uri, HttpContent content)
        {
            var enderecoUrl = AppConfig.BaseUrl + uri;
            try
            {
                AddAuthorizationHeader();
                var result = await _httpClient.PutAsync(enderecoUrl, content);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in the put PUT para {uri}: {ex.Message}");
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        private async Task<(T? Data, string? ErrorMessage)> GetAsync<T>(string endpoint)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync(AppConfig.BaseUrl + endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var data = JsonSerializer.Deserialize<T>(responseString, _serializerOptions);



                    return (data ?? Activator.CreateInstance<T>(), null);
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        string errorMessage = "Unauthorized";
                        _logger.LogWarning(errorMessage);
                        return (default, errorMessage);
                    }

                    string generalErrorMessage = $"Error in requisition: {response.ReasonPhrase}";
                    _logger.LogError(generalErrorMessage);
                    return (default, generalErrorMessage);
                }
            }
            catch (HttpRequestException ex)
            {
                string errorMessage = $"Error in HTTP requisition : {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);
            }
            catch (JsonException ex)
            {
                string errorMessage = $"Error in JSON requisition JSON: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"Unexpected Error: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                return (default, errorMessage);
            }
        }

        private void AddAuthorizationHeader()
        {
            var token = Preferences.Get("accesstoken", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        internal async Task<ApiResponse<Guid>> UploadUserImage(byte[] imageArray)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new ByteArrayContent(imageArray), "image", "image.jpg");
                var response = await PostRequest("api/Account/UploadImage", content);

                if (!response.IsSuccessStatusCode)
                {
                    string errorMessage = response.StatusCode == HttpStatusCode.Unauthorized
                      ? "Unauthorized"
                      : $"Erro ao enviar requisição HTTP: {response.StatusCode}";

                    _logger.LogError($"Erro ao enviar requisição HTTP: {response.StatusCode}");
                    return new ApiResponse<Guid> { ErrorMessage = errorMessage };
                }

                var responseString = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<Guid>(responseString, _serializerOptions);
            

                return new ApiResponse<Guid> { Data = data };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao fazer upload da imagem do usuário: {ex.Message}");
                return new ApiResponse<Guid> { ErrorMessage = ex.Message };
            }
        }
    }
}
