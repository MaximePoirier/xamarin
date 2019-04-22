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
	public class ModifyPasswordViewModel : ViewModelBase
    {
        private string _oldPassword;
        private string _newPassword;
        private string _info;
        public ICommand Edit { get; set; }

        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public string OldPassword
        {
            get => _oldPassword;
            set => SetProperty(ref _oldPassword, value);
        }

        public string NewPassword
        {
            get => _newPassword;
            set => SetProperty(ref _newPassword, value);
        }

		public ModifyPasswordViewModel ()
		{
            this.Edit = new Command(async () => await Enregistrer());
		}

        private async Task Enregistrer()
        {
            if (OldPassword != null && NewPassword != null )
            {
                HttpClient client = new HttpClient();
                UpdatePasswordRequest req = new UpdatePasswordRequest();
                req.OldPassword = OldPassword;
                req.NewPassword = NewPassword;
                var json = JsonConvert.SerializeObject(req);
                HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), "https://td-api.julienmialon.com/me/password");
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
                else
                {
                    Info = "Mauvais Mot de passe ";
                }
            }
        }
    }
}