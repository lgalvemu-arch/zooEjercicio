using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class VeterinarioController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
    }
}
