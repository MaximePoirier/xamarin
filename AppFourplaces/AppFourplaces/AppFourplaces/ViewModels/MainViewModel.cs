using AppFourplaces.Dtos;
using AppFourplaces.Models;
using AppFourplaces.Views;
using Newtonsoft.Json;
using Storm.Mvvm;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class MainViewModel : ViewModelBase
	{
        private List<PlaceItemSummary> _places;

        public ICommand AddPlaces { get; set; }

        public List<PlaceItemSummary> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }

        public MainViewModel ()
		{
            this.AddPlaces = new Command(async () => await NavigationService.PushAsync(new AddPlacePage()));
		}

        public override async Task OnResume()
        {
            await base.OnResume();

            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://td-api.julienmialon.com/places");
            var reponse = await client.GetAsync(uri);

            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                var listplaces = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(content).Data;
                Places = new List<PlaceItemSummary>(listplaces);
            }

        }

        
    }
}