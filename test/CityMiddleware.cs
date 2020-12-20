using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace cinema
{
    public class CityMiddleware
    {
        private readonly RequestDelegate _next;
 
        public CityMiddleware(RequestDelegate next)
        {
            this._next = next;
        }
 
        public async Task InvokeAsync(HttpContext context)
        {
            string city = null;
            if (context.Request.Cookies.ContainsKey("city"))
                city = context.Request.Cookies["city"];

            if(context.Request.HasFormContentType)
                if (context.Request.Form.ContainsKey("city"))
                    city = context.Request.Form["city"];

            city ??= "Казань";
            
            context.Response.Cookies.Append("city", city);
            context.Items["city"] = city;
            
            await _next.Invoke(context);
        }
    }
}