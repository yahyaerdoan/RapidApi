using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RapidApi.WebUserInterfaceLayer.Models;
using RapidApi.WebUserInterfaceLayer.Models.MovieViewModels;
using RapidApi.WebUserInterfaceLayer.Models.SonaHotelModels;
using System.Diagnostics;
using System.Text;

namespace RapidApi.WebUserInterfaceLayer.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetLocation(SearchHotelViewModel searchHotelViewModel)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/searchDestination?query=" + searchHotelViewModel.location_name),
                Headers = {
                  {
                     "X-RapidAPI-Key",
                     "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8"
                  },
                  {
                     "X-RapidAPI-Host",
                     "booking-com15.p.rapidapi.com"
                  },
               },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var locationData = JsonConvert.DeserializeObject<SearchHotelDestinationModel>(body);
                int? destId = Convert.ToInt32(locationData.data[0].dest_id);
                var hotelLocationsData = new SearchHotelViewModel
                {
                    location_name = searchHotelViewModel.location_name,
                    dest_id = destId,
                    arrival_date = searchHotelViewModel.arrival_date,
                    departure_date = searchHotelViewModel.departure_date
                };
                return RedirectToAction("HotelList", "Home", hotelLocationsData);
            }
        }

        public async Task<IActionResult> HotelList(SearchHotelViewModel searchHotelViewModel)
        {
            searchHotelViewModel.search_type = "CITY";
            var client = _httpClientFactory.CreateClient();

            var baseUri = "https://booking-com15.p.rapidapi.com/api/v1/hotels/searchHotels";
            var queryString = $"?dest_id={searchHotelViewModel.dest_id}&search_type={searchHotelViewModel.search_type}&arrival_date={searchHotelViewModel.arrival_date.ToString("yyyy-MM-dd")}&departure_date={searchHotelViewModel.departure_date.ToString("yyyy-MM-dd")}";

            var requestUri = new Uri(baseUri + queryString);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers = {
                  {
                     "X-RapidAPI-Key",
                     "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8"
                  },
                  {
                     "X-RapidAPI-Host",
                     "booking-com15.p.rapidapi.com"
                  },
               },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<HotelListViewModel>(body);
                if (values.data.hotels != null)
                {
                    return View(values.data.hotels.ToList());
                }
                return View(null);
            }
        }

        public async Task<IActionResult> HotelRoomDetail(string hotel_id, string arrival_date, string departure_date)
        {
            var client = _httpClientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://booking-com15.p.rapidapi.com/api/v1/hotels/getHotelDetails?hotel_id=" + hotel_id + "&arrival_date=" + arrival_date + "&departure_date=" + departure_date + "&adults=1&children_age=1%2C17&room_qty=1&languagecode=en-us&currency_code=EUR"),
                Headers = {
                  {
                     "X-RapidAPI-Key",
                     "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8"
                  },
                  {
                     "X-RapidAPI-Host",
                     "booking-com15.p.rapidapi.com"
                  },
               },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var value = JsonConvert.DeserializeObject<HotelListDetailViewModel>(body);
                if (value.data != null)
                {
                    return View(value.data);
                }
            }
            return View();
        }
    }
}