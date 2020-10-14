using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using CriThink.Server.Web.ActionFilters;

namespace CriThink.Server.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi=true)] // No conflict with Swagger
    public class FrontendController : Controller
    {   
        [Route("/")]
        public IActionResult Index(string jwt)
        {
            return View();
        }

        [Route("/control-panel")]
        public IActionResult Control()
        {
            return View();
        }
    }
}

