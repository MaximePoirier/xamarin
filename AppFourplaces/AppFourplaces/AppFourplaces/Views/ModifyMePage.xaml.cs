using AppFourplaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppFourplaces.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModifyMePage : BaseContentPage
    {
		public ModifyMePage ()
		{
			InitializeComponent ();
            BindingContext = new ModifyMeViewModel();
		}
	}
}