using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using CriThink.Common.Endpoints;

namespace CriThink.Server.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi=true)] // No conflict with Swagger
    [ApiVersion(EndpointConstants.VersionOne)]
    public class FrontendController : Controller
    {   
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/control-panel")]
        public IActionResult Control()
        {
            return View();
        }

        [Route("/debunking-news")]
        public IActionResult DebunkingNews()
        {
            return View();
        }
    }
}

