using AppFourplaces.Dtos;
using AppFourplaces.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Storm.Mvvm;
using Storm.Mvvm.Forms;
using Storm.Mvvm.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace AppFourplaces.ViewModels
{
	public class PlaceViewModel : ViewModelBase
    {
        private PlaceItem _place;
        private int _placeId;
        private Position _myPosition;
        private ObservableCollection<Pin> _pinCollection = new ObservableCollection<Pin>();
        private string _info;
        private string _author;
        private string _comment;
        public ICommand SendComment { get; set; }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        public string Comment
        {
            get => _comment;
            set => SetProperty(ref _comment, value);
        }

        public ObservableCollection<Pin> PinCollection { get { return _pinCollection; } set { _pinCollection = value; OnPropertyChanged(); } }

        public Position MyPosition
        {
            get => _myPosition;
            set => SetProperty(ref _myPosition, value);
        }

        public PlaceItem Place
        {
            get => _place;
            set => SetProperty(ref _place, value);
        }

        [NavigationParameter]
        public int PlaceId
        {
            get => _placeId;
            set => SetProperty(ref _placeId, value);
        }

		public PlaceViewModel()
        {
            this.SendComment = new Command(async () => await Commenter());
        }

        private async Task Commenter()
        { 
           if (Author != null && Comment != null)
            {
                HttpClient client = new HttpClient();
                Commentaire com = new Commentaire();
                com.text = Comment;
                var json = JsonConvert.SerializeObject(com);
                var log = (MvvmApplication.Current as App).login;
                if(log.AccessToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
                }
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/places/" + _placeId + "/comments", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    await OnResume();
                    Author = "";
                    Comment = "";
                }
                else
                {
                    Info = "Vous n'êtes pas Connecté";
                    Author = "";
                    Comment = "";
                }
            }
        }

        public override void Initialize(Dictionary<string, object> navigationParameters)
        {
            base.Initialize(navigationParameters);
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://td-api.julienmialon.com/places/"+ _placeId);
            var reponse = await client.GetAsync(uri);

            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                var listplaces = JsonConvert.DeserializeObject<Response<PlaceItem>>(content).Data;
                Place =  (PlaceItem)listplaces;
                MyPosition = new Position(Place.Latitude, Place.Longitude);
                PinCollection.Add(new Pin() { Position = MyPosition, Type = PinType.Generic, Label = Place.Title });

            }

        }

    }
}