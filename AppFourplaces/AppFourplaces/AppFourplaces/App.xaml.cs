using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Storm.Mvvm;
using AppFourplaces.Views;
using TD.Api.Dtos;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AppFourplaces
{
    public partial class App : MvvmApplication
    {
        public LoginResult login = new LoginResult();
        public App():base(()=>new HomePage())
        {
            InitializeComponent();

        }
    }
}
