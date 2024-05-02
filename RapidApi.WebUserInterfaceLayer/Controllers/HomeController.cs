using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using RapidApi.WebUserInterfaceLayer.Models;
using RapidApi.WebUserInterfaceLayer.Models.MovieViewModels;
using RapidApi.WebUserInterfaceLayer.Models.SonaHotelModels;
using System.Diagnostics;

namespace RapidApi.WebUserInterfaceLayer.Controllers
{
    public class HomeController : Controller
    {  
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<SearchHotelDestinationModel.Datum>> GetHotelDestinationAsync()
        {
            
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=Paris"),
                Headers =
    {
        { "X-RapidAPI-Key", "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<SearchHotelDestinationModel>(body);
                return values.data.ToList();
              
            }
        }

        public async Task<List<SelectListItem>> ListDestinationAsync()
        {
            List<SelectListItem> destinationList = (from i in await GetHotelDestinationAsync() 
                                                    select new SelectListItem
                                                    {
                                                        Value = i.dest_id,
                                                        Text = i.name
                                                    }).ToList();
            return destinationList;
        }
        public async Task<IActionResult> Index()
        { 
            ViewBag.DestinationList = await ListDestinationAsync();
            return View();
        }

        public async Task<IActionResult> HotelList(SearchHotelViewModel searchHotelViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUri = "https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels";     
            searchHotelViewModel.search_type = "CITY";        
            var queryString = $"?dest_id={searchHotelViewModel.dest_id}&search_type={searchHotelViewModel.search_type}&arrival_date={searchHotelViewModel.arrival_date.ToString("yyyy-MM-dd")}&departure_date={searchHotelViewModel.departure_date.ToString("yyyy-MM-dd")}";

            // Combine base URI and query string
            var requestUri = new Uri(baseUri + queryString);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers =
    {
        { "X-RapidAPI-Key", "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8" },
        { "X-RapidAPI-Host", "booking-com15.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<SearchHotelViewModel>>(body);
                return View(values);
            }
        }
    }
}