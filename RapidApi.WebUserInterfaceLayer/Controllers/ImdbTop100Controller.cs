using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RapidApi.WebUserInterfaceLayer.Models.MovieViewModels;

namespace RapidApi.WebUserInterfaceLayer.Controllers
{
    public class ImdbTop100Controller : Controller
    {
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://imdb-top-100-movies.p.rapidapi.com/"),
                Headers = {
                  {
                     "X-RapidAPI-Key",
                     "f2dad87302msh8d1a2670dfba065p12ca1ajsnd427718e24d8"
                  },
                  {
                     "X-RapidAPI-Host",
                     "imdb-top-100-movies.p.rapidapi.com"
                  },
               },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<MovieViewModel>>(body);
                return View(values);
            }
        }
    }
}