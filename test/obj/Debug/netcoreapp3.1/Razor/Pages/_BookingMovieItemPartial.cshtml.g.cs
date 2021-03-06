#pragma checksum "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "e4194d5dfec1c68238f0b53a227261d8b1cf8281"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Pages__BookingMovieItemPartial), @"mvc.1.0.view", @"/Pages/_BookingMovieItemPartial.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "D:\project\semester_work\movies\test\Pages\_ViewImports.cshtml"
using cinema;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\project\semester_work\movies\test\Pages\_ViewImports.cshtml"
using cinema.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"e4194d5dfec1c68238f0b53a227261d8b1cf8281", @"/Pages/_BookingMovieItemPartial.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"1333ae1c593edc9ae49443037dec8c0483975219", @"/Pages/_ViewImports.cshtml")]
    public class Pages__BookingMovieItemPartial : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<BookingViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n<div");
            BeginWriteAttribute("href", " href=\"", 31, "\"", 86, 1);
#nullable restore
#line 3 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
WriteAttributeValue("", 38, Url.Content($"~/film/{Model.Schedule.Film.Id}"), 38, 48, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"list-group-item d-flex flex-row align-items-start li-dark-transparent\">\r\n    <div class=\"row py-1 w-100 pr-4 pl-lg-4 pl-4 justify-content-lg-center align-items-center\">\r\n        <div class=\"col-auto p-0 movie-list-item-content\">\r\n            <img");
            BeginWriteAttribute("src", " src=\"", 341, "\"", 390, 1);
#nullable restore
#line 6 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
WriteAttributeValue("", 347, Url.Content(Model.Schedule.Film.ImagePath), 347, 43, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" class=""bg-white d-flex align-items-center justify-content-center h-100 w-100""/>
        </div>
        <div class=""col-4 ml-3 movie-list-item-info d-flex flex-column align-items-start justify-content-around"">
            <div>
                <div class=""d-flex justify-content-center align-items-center"">
                    <h4>");
#nullable restore
#line 11 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                   Write(Model.Schedule.Film.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h4>\r\n                    <h5 class=\"badge badge-secondary ml-2\" style=\"font-size: 1rem; margin-bottom:3px\">New</h5>\r\n                </div>\r\n                <small>");
#nullable restore
#line 14 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                  Write(Model.Schedule.Film.Year);

#line default
#line hidden
#nullable disable
            WriteLiteral(" г.</small>\r\n            </div>\r\n            <small>");
#nullable restore
#line 16 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
              Write(Model.Schedule.Film.Genres.Aggregate((a, b) => $"{a}, {b}"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</small>\r\n            <small class=\"text-small\">Продолжительность: ");
#nullable restore
#line 17 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                                                    Write(Model.Schedule.Film.DurationInMinutes);

#line default
#line hidden
#nullable disable
            WriteLiteral(" мин.</small>\r\n        </div>\r\n\r\n        <div class=\"col-lg-6\">\r\n            <div class=\"row w-100\">\r\n                <div class=\"col-lg-4 d-none d-860-flex d-lg-flex d-xl-flex flex-column fs-12\">\r\n");
            WriteLiteral("                    <h5>Зал: ");
#nullable restore
#line 24 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                        Write(Model.Schedule.Hall.HallNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                    <div>Ряд: ");
#nullable restore
#line 25 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                          Write(Model.Row + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                    <div>Место: ");
#nullable restore
#line 26 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                            Write(Model.Seat + 1);

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                </div>\r\n\r\n                <div class=\"col-lg-auto d-none d-860-flex d-lg-flex d-xl-flex flex-column fs-12\">\r\n                    <h5>Дата: ");
#nullable restore
#line 30 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                         Write(Model.Schedule.Time.ToString("dd.MM.yyyy"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</h5>\r\n                    <div>Время: ");
#nullable restore
#line 31 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                           Write(Model.Schedule.Time.ToString("hh:mm"));

#line default
#line hidden
#nullable disable
            WriteLiteral("</div>\r\n                    <div>Цена: ");
#nullable restore
#line 32 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                          Write(Model.Schedule.PricePerSeat);

#line default
#line hidden
#nullable disable
            WriteLiteral(" руб.</div>\r\n                </div>\r\n            </div>\r\n            <b class=\"row mt-3 ml-4\">Код брони: ");
#nullable restore
#line 35 "D:\project\semester_work\movies\test\Pages\_BookingMovieItemPartial.cshtml"
                                           Write(Model.BookingCode);

#line default
#line hidden
#nullable disable
            WriteLiteral("</b>\r\n        </div>\r\n    </div>\r\n</div>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<BookingViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
