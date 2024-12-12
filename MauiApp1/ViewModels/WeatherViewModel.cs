using MauiApp1.Models;
using MauiApp1.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Diagnostics;

namespace MauiApp1.ViewModels
{
    public partial class WeatherViewModel : ObservableObject
    {
        [ObservableProperty]
        private string cityInput;

        [ObservableProperty]
        private string escolha;

        [ObservableProperty]
        private string cityName;

        [ObservableProperty]
        private string temp;

        [ObservableProperty]
        private string image;

        [ObservableProperty]
        private string geral;

        [ObservableProperty]
        private string tempextra;

        [ObservableProperty]
        private string sunrise;
        [ObservableProperty]
        private string sunset;

        [ObservableProperty]
        private string humidade;

        [ObservableProperty]
        private string vento;

        [ObservableProperty]
        private string visibilidade;

        [ObservableProperty]
        private string pressao;

        private string lat;
        private string lon;
        private int fuso;

        public ICommand GetCommand { get; }
        private ApiService service;

        public WeatherViewModel()
        {
            service = new ApiService();
            GetCommand = new RelayCommand(async () => await getWeather());
            Task.Run(async () => await GetWeatherByCurrentLocation());
        }

        private async Task getWeather()
        {
            if (string.IsNullOrEmpty(cityInput))
            {
                App.Current.MainPage.DisplayAlert("Erro", "Escolha a cidade ou coordenada que deseja buscar", "ok");
                return;
            }
            else
            {
                if (Escolha == "Cidade")
                {
                    var response = await service.GetWeatherResponse(cityInput);

                    if (response != null)
                    {
                        CityName = response.name;
                        Temp = Math.Round(response.main.Temp - 273.15) + "ºC";
                        Geral = char.ToUpper(response.weather[0].description[0]) + response.weather[0].description.Substring(1).ToLower();
                        Tempextra = Math.Round(response.main.TempMax - 273.15) + "ºC/" + Math.Round(response.main.TempMin - 273.15) + "ºC. Sensação térmica: " + Math.Round(response.main.FeelsLike - 273.15);
                        Humidade = response.main.Humidity + "%";
                        Vento = response.wind.Speed + "km/h";
                        Visibilidade = response.visibility / 1000f + "km";
                        Pressao = response.main.Pressure + "mb";
                        fuso = response.timezone + 10800;
                        Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunrise).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                        Sunset = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunset).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert($"Erro", $"Não foi possível recuperar as informações do clima de {cityInput}.", "Ok");
                    }
                }
                else
                {
                    if (Escolha == "Coordenada")
                    {
                        string[] temp = cityInput.Split(' ');
                        lat = temp[0];
                        lon = temp[1];

                        var response = await service.GetWeatherByCoord(lat, lon);

                        if (response != null)
                        {
                            CityName = cityInput;
                            Temp = Math.Round(response.main.Temp - 273.15) + "ºC";
                            Geral = char.ToUpper(response.weather[0].description[0]) + response.weather[0].description.Substring(1).ToLower();
                            Tempextra = Math.Round(response.main.TempMax - 273.15) + "ºC/" + Math.Round(response.main.TempMin - 273.15) + "ºC. Sensação térmica:" + Math.Round(response.main.FeelsLike - 273.15);
                            Humidade = response.main.Humidity + "%";
                            Vento = response.wind.Speed + "km/h";
                            Visibilidade = response.visibility / 1000f + "km";
                            Pressao = response.main.Pressure + "mb";
                            fuso = response.timezone + 10800;
                            Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunrise).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                            Sunset = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunset).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                        }
                        else
                        {
                            App.Current.MainPage.DisplayAlert($"Erro", $"Não foi possível recuperar as informações do clima de {cityInput}.", "Ok");
                        }
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert($"Erro", "Escolha cidade ou coordenada.", "Ok");
                    }
                }
            }
        }

        private async Task GetWeatherByCurrentLocation()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    lat = location.Latitude.ToString("F6");
                    lon = location.Longitude.ToString("F6");

                    var response = await service.GetWeatherByCoord(lat, lon);
                    if (response != null)
                    {
                        CityName = $"{lat}, {lon}";
                        Temp = Math.Round(response.main.Temp - 273.15) + "ºC";
                        Geral = char.ToUpper(response.weather[0].description[0]) + response.weather[0].description.Substring(1).ToLower();
                        Tempextra = Math.Round(response.main.TempMax - 273.15) + "ºC/" + Math.Round(response.main.TempMin - 273.15) + "ºC. Sensação térmica:" + Math.Round(response.main.FeelsLike - 273.15);
                        Humidade = response.main.Humidity + "%";
                        Vento = response.wind.Speed + "km/h";
                        Visibilidade = response.visibility / 1000f + "km";
                        Pressao = response.main.Pressure + "mb";
                        fuso = response.timezone + 10800;
                        Sunrise = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunrise).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                        Sunset = DateTimeOffset.FromUnixTimeSeconds(response.sys.Sunset).AddSeconds(fuso).ToLocalTime().ToString("HH:mm");
                    }
                    else
                    {
                        App.Current.MainPage.DisplayAlert($"Erro", "Não foi possível recuperar as informações do clima para a localização atual.", "Ok");
                    }
                }
                else
                {
                    App.Current.MainPage.DisplayAlert("Erro", "Não foi possível determinar a localização atual.", "Ok");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erro ao obter localização: {ex.Message}");
                App.Current.MainPage.DisplayAlert("Erro", "Erro ao obter a localização atual.", "Ok");
            }
        }
    }
}