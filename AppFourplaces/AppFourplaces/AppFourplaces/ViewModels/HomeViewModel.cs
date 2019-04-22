using AppFourplaces.Views;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class HomeViewModel : ViewModelBase
	{
        public ICommand GoToPlaces { get; set; }
        public ICommand GoToRegister { get; set; }
        public ICommand Connexion { get; set; }
        public ICommand GoToProfile { get; set; }

        public HomeViewModel ()
		{
            this.GoToPlaces = new Command(async () => await NavigationService.PushAsync(new MainPage()));
            this.GoToRegister = new Command(async () => await NavigationService.PushAsync(new RegisterPage()));
            this.Connexion = new Command(async () => await NavigationService.PushAsync(new LoginPage()));
            this.GoToProfile = new Command(async () => await NavigationService.PushAsync(new MePage()));
		}
	}
}