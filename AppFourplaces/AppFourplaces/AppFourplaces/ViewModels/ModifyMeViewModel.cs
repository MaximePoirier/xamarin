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
using TD.Api.Dtos;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class ModifyMeViewModel : ViewModelBase
    {
        private string _prenom;
        private string _nom;
        private int _imageId;
        public ICommand Edit { get; set; }

        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value);
        }

        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        public int ImageId
        {
            get => _imageId;
            set => SetProperty(ref _imageId, value);
        }
        public ModifyMeViewModel ()
		{
            this.Edit = new Command(async () => await Envoyer());
		}

        private async Task Envoyer()
        {
            if (Prenom != null && Nom != null && ImageId != 0)
            {
                HttpClient client = new HttpClient();
                UpdateProfileRequest req = new UpdateProfileRequest();
                req.FirstName = Prenom;
                req.LastName = Nom;
                req.ImageId = ImageId;
                var json = JsonConvert.SerializeObject(req);
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/me");
                var log = (MvvmApplication.Current as App).login;
                if (log.AccessToken != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
                }
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    await NavigationService.PopAsync();
                }
            }
        }
    }
}