#pragma checksum "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Account_Details), @"mvc.1.0.view", @"/Views/Account/Details.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Account/Details.cshtml", typeof(AspNetCore.Views_Account_Details))]
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
#line 1 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\_ViewImports.cshtml"
using OnlineMovieDatabase;

#line default
#line hidden
#line 2 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\_ViewImports.cshtml"
using OnlineMovieDatabase.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d", @"/Views/Account/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8dee5b1bdac6a05f67a0edad73efd796c4fe15f9", @"/Views/_ViewImports.cshtml")]
    public class Views_Account_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<OnlineMovieDatabase.ViewModels.AccountDetailsViewModels>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("biguseravatar"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("onerror", new global::Microsoft.AspNetCore.Html.HtmlString("this.src=\'../static/images/UsersAvatars/NoAvatar/NoAvatar200px.png\'"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Разблокировать"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/static/images/InterfaceButtons/Plus.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("height", new global::Microsoft.AspNetCore.Html.HtmlString("30"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Забанить"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/static/images/InterfaceButtons/Delete.png"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(65, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
  
    ViewData["Title"] = Model.User.UserName;

#line default
#line hidden
            BeginContext(120, 14, true);
            WriteLiteral("\r\n<h1>Профиль ");
            EndContext();
            BeginContext(135, 19, false);
#line 7 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
       Write(Model.User.UserName);

#line default
#line hidden
            EndContext();
            BeginContext(154, 35, true);
            WriteLiteral("</h1>\r\n\r\n<div align=\"center\">\r\n    ");
            EndContext();
            BeginContext(189, 163, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d6584", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginAddHtmlAttributeValues(__tagHelperExecutionContext, "src", 3, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            AddHtmlAttributeValue("", 221, "~/static/images/UsersAvatars/", 221, 29, true);
#line 10 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
AddHtmlAttributeValue("", 250, Model.User.Id, 250, 16, false);

#line default
#line hidden
            AddHtmlAttributeValue("", 266, ".jpg", 266, 4, true);
            EndAddHtmlAttributeValues(__tagHelperExecutionContext);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(352, 56, true);
            WriteLiteral("\r\n    <br />\r\n    <label style=\"font-size:20px\">Отзывов:");
            EndContext();
            BeginContext(409, 20, false);
#line 12 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
                                     Write(Model.ReviewsCounter);

#line default
#line hidden
            EndContext();
            BeginContext(429, 15, true);
            WriteLiteral(", комментариев:");
            EndContext();
            BeginContext(445, 21, false);
#line 12 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
                                                                         Write(Model.CommentsCounter);

#line default
#line hidden
            EndContext();
            BeginContext(466, 2238, true);
            WriteLiteral(@"</label>
    <br />
</div>
<script>
    function BanUser(userid, banfor, reason) {
        $.ajax({
            type: 'get',
            async: true,
            url: ""../Users/"" + userid + ""/Ban?bannedFor="" + banfor + ""&reason="" + reason,
            success: function (msg) {
                if (msg == ""ок"") {
                    alert(""Пользователь заблокирован успешно"");
                    var newHtml =  `<details>
                                        <summary>Разблокировать пользователя</summary>
                                        <a onclick=""UnbanUser(` + userid + `)"">
                                            <img title=""Разблокировать"" src=""../static/images/InterfaceButtons/Plus.png"" height=""30"" />
                                        </a>
                                    </details>`
                    $('#banunban').html(newHtml)
                }
                else {
                    alert(""Ошибка: "" + msg);
                }
            }
        });
");
            WriteLiteral(@"
    }
    function UnbanUser(userid) {
        $.ajax({
            type: 'get',
            async: true,
            url: ""../Users/"" + userid + ""/Unban"",
            success: function (msg) {
                if (msg == ""ок"") {
                    alert(""Пользователь разблокирован успешно"");
                    var newHtml = `<details>
                                        <summary>Заблокировать пользователя</summary>
                                        Дата окончания блокировки: <input id=""dateBan"" type=""date"" />
                                        Причина блокировки: <input id=""banReason"" type=""text""/>
                                        <a title=""Заблокировать"" onclick=""BanUser(` + userid + `, $('#dateBan').val(), $('#banReason').val())"">
                                            <img title=""Забанить"" src=""../static/images/InterfaceButtons/Delete.png"" height=""30"" />
                                        </a>
                                    </details>`
              ");
            WriteLiteral("      $(\'#banunban\').html(newHtml)\r\n                }\r\n                else {\r\n                    alert(\"Ошибка: \" + msg);\r\n                }\r\n            }\r\n        });\r\n    }\r\n</script>\r\n");
            EndContext();
#line 64 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
 if (User.IsInRole("admin"))
{

#line default
#line hidden
            BeginContext(2737, 40, true);
            WriteLiteral("    <div align=\"center\" id=\"banunban\">\r\n");
            EndContext();
#line 67 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
         if (Model.User.IsBanned)
        {
            

#line default
#line hidden
#line 69 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
             if (Model.User.BannedFor > (DateTime.Now))
            {

#line default
#line hidden
            BeginContext(2895, 117, true);
            WriteLiteral("                <details>\r\n                    <summary>Разблокировать пользователя</summary>\r\n                    <a");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 3012, "\"", 3047, 3);
            WriteAttributeValue("", 3022, "UnbanUser(", 3022, 10, true);
#line 73 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
WriteAttributeValue("", 3032, Model.User.Id, 3032, 14, false);

#line default
#line hidden
            WriteAttributeValue("", 3046, ")", 3046, 1, true);
            EndWriteAttribute();
            BeginContext(3048, 27, true);
            WriteLiteral(">\r\n                        ");
            EndContext();
            BeginContext(3075, 90, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d13192", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3165, 56, true);
            WriteLiteral("\r\n                    </a>\r\n                </details>\r\n");
            EndContext();
#line 77 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
            }
            else
            {

#line default
#line hidden
            BeginContext(3269, 298, true);
            WriteLiteral(@"                <details>
                    <summary>Заблокировать пользователя</summary>
                    Дата окончания блокировки: <input id=""dateBan"" type=""date"" />
                    Причина блокировки: <input id=""banReason"" type=""text""/>
                    <a title=""Заблокировать""");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 3567, "\"", 3644, 5);
            WriteAttributeValue("", 3577, "BanUser(", 3577, 8, true);
#line 84 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
WriteAttributeValue("", 3585, Model.User.Id, 3585, 14, false);

#line default
#line hidden
            WriteAttributeValue("", 3599, ",", 3599, 1, true);
            WriteAttributeValue(" ", 3600, "$(\'#dateBan\').val(),", 3601, 21, true);
            WriteAttributeValue(" ", 3621, "$(\'#banReason\').val())", 3622, 23, true);
            EndWriteAttribute();
            BeginContext(3645, 27, true);
            WriteLiteral(">\r\n                        ");
            EndContext();
            BeginContext(3672, 86, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d15941", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(3758, 56, true);
            WriteLiteral("\r\n                    </a>\r\n                </details>\r\n");
            EndContext();
#line 88 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
            }

#line default
#line hidden
#line 88 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
             
        }
        else
        {

#line default
#line hidden
            BeginContext(3865, 286, true);
            WriteLiteral(@"            <details>
                <summary>Заблокировать пользователя</summary>
                    Дата окончания блокировки: <input id=""dateBan"" type=""date"" />
                    Причина блокировки: <input id=""banReason"" type=""text""/>
                <a title=""Заблокировать""");
            EndContext();
            BeginWriteAttribute("onclick", " onclick=\"", 4151, "\"", 4228, 5);
            WriteAttributeValue("", 4161, "BanUser(", 4161, 8, true);
#line 96 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
WriteAttributeValue("", 4169, Model.User.Id, 4169, 14, false);

#line default
#line hidden
            WriteAttributeValue("", 4183, ",", 4183, 1, true);
            WriteAttributeValue(" ", 4184, "$(\'#dateBan\').val(),", 4185, 21, true);
            WriteAttributeValue(" ", 4205, "$(\'#banReason\').val())", 4206, 23, true);
            EndWriteAttribute();
            BeginContext(4229, 23, true);
            WriteLiteral(">\r\n                    ");
            EndContext();
            BeginContext(4252, 86, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("img", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "eda9e9e55ab0b4d4e3dc4c67c42bacf430d6c85d18829", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(4338, 48, true);
            WriteLiteral("\r\n                </a>\r\n            </details>\r\n");
            EndContext();
#line 100 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
        }

#line default
#line hidden
            BeginContext(4397, 12, true);
            WriteLiteral("    </div>\r\n");
            EndContext();
#line 102 "C:\MyFiles\Programming\OnlineMoviesDatabase\OnlineMoviesDatabase\Views\Account\Details.cshtml"
}

#line default
#line hidden
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<OnlineMovieDatabase.ViewModels.AccountDetailsViewModels> Html { get; private set; }
    }
}
#pragma warning restore 1591
