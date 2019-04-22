using AppFourplaces.Dtos;
using AppFourplaces.Views;
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
	public class MeViewModel : ViewModelBase
    {
        private UserItem _me;
        private string _info;
        public ICommand ModifyMe { get; set; }
        public ICommand ModifyPassword { get; set; }

        public UserItem Me
        {
            get => _me;
            set => SetProperty(ref _me, value);
        }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

		public MeViewModel ()
		{
            this.ModifyMe = new Command(async () => await NavigationService.PushAsync(new ModifyMePage()));
            this.ModifyPassword = new Command(async () => await NavigationService.PushAsync(new ModifyPasswordPage()));
		}

        public override async Task OnResume()
        {
            await base.OnResume();

            HttpClient client = new HttpClient();
            Uri uri = new Uri("https://td-api.julienmialon.com/me");
            var log = (MvvmApplication.Current as App).login;
            if (log.AccessToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
            }
            var reponse = await client.GetAsync(uri);
            if (reponse.IsSuccessStatusCode)
            {
                var content = await reponse.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Response<UserItem>>(content).Data;
                Me = user;
            }
            else
            {
                Info = "Vous n'êtes pas connecté";
            }
        }

    }
}