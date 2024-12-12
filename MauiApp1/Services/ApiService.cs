using MauiApp1.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;


namespace MauiApp1.Services
{
    public class ApiService
    {
        private HttpClient httpClient;
        private WeatherResponse response;
        private JsonSerializerOptions jsonSerializerOptions;

        public ApiService()
        {
            httpClient = new HttpClient();
            jsonSerializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<WeatherResponse> GetWeatherResponse(string cityInput)
        {
            string apiKey = "e3562ca8760dc6b2df351c7f955b4439";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cityInput}&appid={apiKey}&lang=pt_br";

            try
            {
                var resp = await httpClient.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro: {resp.StatusCode}");
                }

                string jsonResponse = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine($"Resposta: {jsonResponse}");
                return JsonSerializer.Deserialize<WeatherResponse>(jsonResponse, jsonSerializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<WeatherResponse> GetWeatherByCoord(string lat, string lon)
        {
            string apiKey = "e3562ca8760dc6b2df351c7f955b4439";
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}&lang=pt_br";

            try
            {
                var resp = await httpClient.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                {
                    throw new Exception($"Erro: {resp.StatusCode}");
                }

                string jsonResponse = await resp.Content.ReadAsStringAsync();
                Debug.WriteLine($"Resposta: {jsonResponse}");
                return JsonSerializer.Deserialize<WeatherResponse>(jsonResponse, jsonSerializerOptions);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
