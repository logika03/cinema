using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using test.Models;

namespace test
{
    public class ScheduleBadgeViewComponent : ViewComponent
    {
        private readonly AuthService _authService;
        public ScheduleBadgeViewComponent(AuthService authService)
        {
            _authService = authService;
        }
        
        public IViewComponentResult Invoke(ScheduleViewModel scheduleViewModel,
            string outerTagClasses = "mb-1",
            string innerTagClasses = "mr-2 fs-1", 
            string outerTag = "h4"
        )
        {
            var outerTagBuilder = new TagBuilder(outerTag);
            outerTagBuilder.AddCssClass("badge-parent");
            foreach(var outerClass in outerTagClasses.Split(' '))
                outerTagBuilder.AddCssClass(outerClass);
            
            var badgeTagBuilder = new TagBuilder("a");
            foreach(var innerClass in innerTagClasses.Split(' '))
                badgeTagBuilder.AddCssClass(innerClass);
            badgeTagBuilder.AddCssClass("badge");
            badgeTagBuilder.AddCssClass("badge-outline-white");
            
            if(_authService.IsAuthenticated)
                badgeTagBuilder.Attributes.Add("href", Url.Content(string.Format("~/booking/{0}", scheduleViewModel.Id)));
            else
            {
                badgeTagBuilder.Attributes.Add("data-toggle", "modal");
                badgeTagBuilder.Attributes.Add("data-target", "#loginModal");
            }
            badgeTagBuilder.InnerHtml.Append(scheduleViewModel.Time.ToString("HH:mm"));

            var priceTag = new TagBuilder("small");
            priceTag.InnerHtml.Append($"{scheduleViewModel.PricePerSeat} руб.");
            outerTagBuilder.InnerHtml.AppendHtml(badgeTagBuilder);
            outerTagBuilder.InnerHtml.AppendHtml(priceTag);

            return new HtmlContentViewComponentResult(outerTagBuilder);
        }
    }
}