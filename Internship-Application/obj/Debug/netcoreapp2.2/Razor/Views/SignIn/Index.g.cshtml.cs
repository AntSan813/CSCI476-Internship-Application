#pragma checksum "/Users/antoniosantos/Projects/CSCI476-Internship-Application/Internship-Application/Views/SignIn/Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fa7423bc96774a52159d8d433c730f8828d52739"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_SignIn_Index), @"mvc.1.0.view", @"/Views/SignIn/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/SignIn/Index.cshtml", typeof(AspNetCore.Views_SignIn_Index))]
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
#line 1 "/Users/antoniosantos/Projects/CSCI476-Internship-Application/Internship-Application/Views/_ViewImports.cshtml"
using Internship_Application;

#line default
#line hidden
#line 2 "/Users/antoniosantos/Projects/CSCI476-Internship-Application/Internship-Application/Views/_ViewImports.cshtml"
using Internship_Application.Models;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fa7423bc96774a52159d8d433c730f8828d52739", @"/Views/SignIn/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0c60719c61f3a507d7f6f44e27aab3497e1c6f0f", @"/Views/_ViewImports.cshtml")]
    public class Views_SignIn_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(0, 1575, true);
            WriteLiteral(@"<h1>Sign in page</h1>
<fieldset>
    <legend><h1>Student E-mail</h1></legend>



    <div id=""loginUI"">
        <table>
            <tbody>
                <tr>
                    <td>Username:</td>
                    <td><input name=""txtUsername"" type=""text"" value=""santosa4"" id=""txtUsername"" tabindex=""1"" autocomplete=""off"" style=""background-image: url(&quot;data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR4nGP6zwAAAgcBApocMXEAAAAASUVORK5CYII=&quot;); cursor: auto;""><a title=""Username Lookup"" href=""https://asap.winthrop.edu/studentaccount/lookupusername.aspx"" tabindex=""6"">&nbsp;Lookup Username</a></td>
                </tr>
                <tr>
                    <td>Password:</td>
                    <td><input name=""txtPassword"" type=""password"" id=""txtPassword"" tabindex=""2"" autocomplete=""off"" style=""background-image: url(&quot;data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR4nGP6zwAAAgcBApocMXEAAAAASUVORK5CYII=&quot;); cursor: auto;"">");
            WriteLiteral(@"<a title=""Forget Password?"" href=""https://asap.winthrop.edu/studentaccount/changepassword.aspx"" tabindex=""7"">&nbsp;Forgot Password?</a></td>
                </tr>
            </tbody>
        </table>
        <div>

            <input id=""chkRememberMe"" type=""checkbox"" name=""chkRememberMe"" checked=""checked"" tabindex=""4""><label for=""chkRememberMe"">Remember me on this computer</label>
            <input type=""submit"" name=""btnLogin"" value=""Login"" id=""btnLogin"" tabindex=""5"" title=""Click to login... "">
        </div>
    </div>
</fieldset>");
            EndContext();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
