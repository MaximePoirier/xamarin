using AppFourplaces.Dtos;
using Newtonsoft.Json;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TD.Api.Dtos;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class LoginViewModel : ViewModelBase
    {
        private string _email;
        private string _password;
        private string _token;

        public ICommand SendLogin { get; set; }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public LoginViewModel ()
		{
            this.SendLogin = new Command(async () => await Connexion());
		}

        private async Task Connexion()
        {
            if (Email != null && Password != null)
            {
                HttpClient client = new HttpClient();
                LoginRequest req = new LoginRequest();
                req.Email = Email;
                req.Password = Password;
                var json = JsonConvert.SerializeObject(req);
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/auth/login", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var login = JsonConvert.DeserializeObject<Response<LoginResult>>(content).Data;
                    Token = login.AccessToken;
                    var log = (MvvmApplication.Current as App).login;
                    log.AccessToken = login.AccessToken;
                    log.RefreshToken = login.RefreshToken;
                    log.ExpiresIn = login.ExpiresIn;
                    log.TokenType = login.TokenType;
                    await NavigationService.PopAsync();
                }
                else
                {
                    Token = "Identifiant ou Mot de passe Incorrect";
                }
            }
        }
    }
}