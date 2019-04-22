using AppFourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace AppFourplaces
{
	public partial class PlacePage : BaseContentPage
    {
		public PlacePage ()
		{
			InitializeComponent ();
            BindingContext = new PlaceViewModel();
        }

        
	}
}