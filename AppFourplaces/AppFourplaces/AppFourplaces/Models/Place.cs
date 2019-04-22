using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace AppFourplaces.Models
{
	public class Place
	{
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int ImageId { get; set; }

        public int Latitude { get; set; }

        public int Longitude { get; set; }

		public Place ()
		{	
		}
	}
}