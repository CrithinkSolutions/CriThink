using Microsoft.AspNetCore.Mvc.Filters;

namespace CriThink.Server.Web.ActionFilters
{
    public class ScraperAuthenticationFilter : BaseCrossServiceAuthenticationFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ValidateCredentials(context, "CrossService:Header", "CrossService:Scraper:Value");
        }
    }
}