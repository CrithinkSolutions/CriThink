using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace CriThink.Server.Web.Controllers
{
    public class FrontendController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}