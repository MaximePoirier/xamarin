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
    public class RegisterViewModel : ViewModelBase
    {
        private string _email;
        private string _prenom;
        private string _nom;
        private string _password;
        private string _token;
        public ICommand SendRegister {get; set;}

        public string Token
        {
            get => _token;
            set => SetProperty(ref _token, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

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

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }


		public RegisterViewModel ()
		{
            this.SendRegister = new Command(async () => await Register());
        }

        private async Task Register()
        {
            if( Email != null && Prenom != null && Nom != null && Password != null)
            {
                HttpClient client = new HttpClient();
                RegisterRequest req = new RegisterRequest();
                req.Email = Email;
                req.FirstName = Prenom;
                req.LastName = Nom;
                req.Password = Password;
                var json = JsonConvert.SerializeObject(req);
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/auth/register", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var login = JsonConvert.DeserializeObject<Response<LoginResult>>(content).Data;
                    await NavigationService.PopAsync();
                }
                else
                {
                    Token = "Compte déjà associé à cet email";
                }
            }
        }
    }
}