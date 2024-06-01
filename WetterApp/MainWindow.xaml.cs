using System.Net.Http;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.Marshalling.IIUnknownCacheStrategy;
using System.Buffers.Text;
using Newtonsoft.Json.Converters;
using System.Xaml.Permissions;
using WetterApp;

namespace WetterApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "f1abc6e07b7a446c01d4ee9a3228676d";
        private string requestUrl = "https://api.openweathermap.org/data/2.5/weather";
        public MainWindow()
        {
            InitializeComponent();
            UbdateData("Dortmund");
          
        }
        public void UbdateData(string city)
        {
            WeatherMapResponse result = GetWeatherData(city);
            float temperatur = result.main.temp;
            string finalImage = "Sonne.png";
            string currentWeather = result.weather[0].main.ToLower();
            if (currentWeather.Contains("cloud"))
            {
                finalImage = "Wolken.png";
                Console.WriteLine(".");
                if (currentWeather.Contains("cloud") &&temperatur>=7) { Console.WriteLine("Empfehlung:Schal, Jacke und langer Spaziergan sind gut für die Selle!");
                    finalImage = "Wandern.jpg";
                }
            }
            else if (currentWeather.Contains("windy") && temperatur<=7)
            {
                finalImage = "Wind.png";
                Console.WriteLine(" Empfehlung: Mütze, Handschue und Winterjacke");
            }
            else if (currentWeather.Contains("rain"))
            {
                finalImage = "Regen.png";
            }
            else if (currentWeather.Contains("snow"))
            {
                finalImage = "SchneeTag.png";
            }
            else if (currentWeather.Contains("sun"))
            {
                finalImage = "Sonne.png";
            }

            backgroundImage.ImageSource = new BitmapImage(new Uri("Bilder/" + finalImage, UriKind.Relative));
            labelTemperature.Content = result.main.temp.ToString("F1") + "°C";
            labelinfo.Content = result.weather[0].main;

        }
        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient();
            //Hier werden die Temperatur und app daten abgerufen.
            var finalUri = requestUrl + "?q=" + city + "&appid=" + apiKey + "&units=metric";
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result;
            string response = httpResponse.Content.ReadAsStringAsync().Result;
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);
            //json convertiert den string in ein Weathermap string. 
            return weatherMapResponse;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string query = textBoxQuery.Text;
            UbdateData(query);
        }  
    }
}