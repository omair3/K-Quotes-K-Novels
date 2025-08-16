using Microsoft.AspNetCore.Mvc;
namespace KQuotesNovels.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}

