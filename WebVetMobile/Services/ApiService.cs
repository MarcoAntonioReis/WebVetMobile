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
                    Username= username,
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

                Preferences.Set("accesstoken", result!.AccessToken);
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

        public async Task<(List<Appointment>? appointments, string? ErrorMessage)> GetAppoimtments(string userEmail)
        {

            string endpoint = $"api/Owner/GetOwnerAppointmentList?userEmail={userEmail}";
            return await GetAsync<List<Appointment>>(endpoint);

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


    }
}
