using Microsoft.AspNetCore.Mvc;
using RapidApi.WebUserInterfaceLayer.Models;
using System.Diagnostics;

namespace RapidApi.WebUserInterfaceLayer.Controllers
{
    public class HomeController : Controller
    {  
        public async Task<IActionResult> Index()
        { 
            return View();
        }        
    }
}