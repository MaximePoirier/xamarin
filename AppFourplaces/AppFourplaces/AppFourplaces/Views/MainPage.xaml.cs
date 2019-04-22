using AppFourplaces.Dtos;
using AppFourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Storm.Mvvm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppFourplaces
{
    public partial class MainPage : BaseContentPage
    {
        private MainViewModel vm;

        public MainPage()
        {
            InitializeComponent();
            this.vm = new MainViewModel();
            BindingContext = this.vm;
        }

        public async void GoToThePlace(object sender, ItemTappedEventArgs e)
        {
            var myListView = (ListView)sender;
            var myItem = (PlaceItemSummary)myListView.SelectedItem;
            await this.vm.NavigationService.PushAsync(new PlacePage(), new Dictionary<string, object>()
            {
                { "PlaceId", myItem.Id }
            });
        }
    }
}
