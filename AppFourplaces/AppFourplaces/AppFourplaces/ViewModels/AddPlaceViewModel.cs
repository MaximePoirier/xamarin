using AppFourplaces.Dtos;
using Newtonsoft.Json;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class AddPlaceViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private double _latitude;
        private double _longitude;
        private string _info;
        public ICommand SendPlace { get; set; }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        public AddPlaceViewModel ()
		{
            this.SendPlace = new Command(async () => await SavePlace());
		}

        private async Task SavePlace()
        {
            if(Title != null && Description != null && Latitude != 0 && Longitude != 0)
            {
                HttpClient client = new HttpClient();
                CreatePlaceRequest req = new CreatePlaceRequest();
                req.Title = Title;
                req.Description = Description;
                req.Latitude = Latitude;
                req.Longitude = Longitude;
                req.ImageId = 1;
                var json = JsonConvert.SerializeObject(req);
                var log = (MvvmApplication.Current as App).login;
                if (log.AccessToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
                }
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/places", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    await NavigationService.PopAsync();
                }
                else
                {
                    Info = "Vous n'êtes pas connecté";
                }
            }
        }
    }
}