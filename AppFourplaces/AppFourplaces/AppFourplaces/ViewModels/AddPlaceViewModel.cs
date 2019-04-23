using AppFourplaces.Dtos;
using Newtonsoft.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppFourplaces.ViewModels
{
	public class AddPlaceViewModel : ViewModelBase
    {
        private string _title;
        private string _description;
        private double _latitude;
        private double _longitude;
        private string _info;
        private ImageSource _image1;
        private byte[] _imageData;
        public ICommand SendPlace { get; set; }
        public ICommand TakePicture { get; set; }
        public ICommand PickPicture { get; set; }
        public ICommand Pos { get; set; }


        public byte[] ImageData
        {
            get => _imageData;
            set => SetProperty(ref _imageData, value);
        }
        public ImageSource Image1
        {
            get => _image1;
            set => SetProperty(ref _image1, value);
        }
        public string Info
        {
            get => _info;
            set => SetProperty(ref _info, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }
        public AddPlaceViewModel ()
		{
            this.SendPlace = new Command(async () => await SavePlace());
            this.TakePicture = new Command(async () => await TakePic());
            this.PickPicture = new Command(async () => await PickPic());
            this.Pos = new Command(async () => await GetPos());
		}

        private async Task GetPos()
        {
            var locator = CrossGeolocator.Current;
            Position position = null;
            position = await locator.GetLastKnownLocationAsync();

            if (position != null)
            {

                Latitude = position.Latitude;
                Longitude = position.Longitude;
            }
            if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
            {
                Info = "Activer la position sur votre téléphone";
            }
            position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            if (position != null)
            {

                Latitude = position.Latitude;
                Longitude = position.Longitude;
            }
        }

        private async Task PickPic()
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
            {
                Info = "Pas de camera disponible";
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
            {
                return;
            }
            Image1 = ImageSource.FromStream(() => file.GetStream());
            MemoryStream ms = new MemoryStream();
            file.GetStream().CopyTo(ms);
            ImageData = ms.ToArray();
        }

        private async Task TakePic()
        {
            await CrossMedia.Current.Initialize();
            if(!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                Info = "Pas de camera disponible";
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                SaveToAlbum = false,
                Name = Title +".jpg"
            }); 

            if(file == null)
            {
                return;
            }
            Image1 = ImageSource.FromStream(() => file.GetStream());
        }

        private async Task SavePlace()
        {
            if(Title != null && Description != null && Latitude != 0 && Longitude != 0)
            {
                var log = (MvvmApplication.Current as App).login;
                HttpClient client = new HttpClient();
                CreatePlaceRequest req = new CreatePlaceRequest();
                req.Title = Title;
                req.Description = Description;
                req.Latitude = Latitude;
                req.Longitude = Longitude;
                if (ImageData != null)
                {
                    HttpClient clientImg = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://td-api.julienmialon.com/images");
                    if(log.AccessToken != null)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
                    }
                    

                    MultipartFormDataContent requestContent = new MultipartFormDataContent();

                    var imageContent = new ByteArrayContent(ImageData);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                    requestContent.Add(imageContent, "file", "file.jpg");

                    request.Content = requestContent;

                    HttpResponseMessage responseImg = await clientImg.SendAsync(request);

                    if (responseImg.IsSuccessStatusCode)
                    {
                        var content = await responseImg.Content.ReadAsStringAsync();
                        var imgItem = JsonConvert.DeserializeObject<Response<ImageItem>>(content).Data;
                        req.ImageId = imgItem.Id;
                    }
                    else
                    {
                        req.ImageId = 1;
                    }
                }
                else
                {
                    req.ImageId = 1;
                }
                var json = JsonConvert.SerializeObject(req);
                
                if (log.AccessToken != null)
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(log.TokenType, log.AccessToken);
                }
                HttpResponseMessage response = await client.PostAsync("https://td-api.julienmialon.com/places", new StringContent(json, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    await NavigationService.PopAsync();
                }
                else
                {
                    Info = "Vous n'êtes pas connecté";
                }
            }
        }
    }
}