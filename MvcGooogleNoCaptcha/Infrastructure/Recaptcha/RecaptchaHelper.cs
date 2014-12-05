using System;
using System.Net;
using System.Web.Mvc;
using System.Configuration;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Web;
namespace MvcGooogleNoCaptcha.Infrastructure.Recaptcha
{
    public static class RecaptchaHelper
    {
        public static string RecaptchaJsHost
        {
            get { return ConfigurationManager.AppSettings["RecaptchaJsHost"]; }
        }
        public static string PublicKey
        {

            get { return ConfigurationManager.AppSettings["RecaptchaPublicKey"]; }
        }
        public static MvcHtmlString GenerateCaptcha(this HtmlHelper helper, string id, string theme)
        {
            if (string.IsNullOrEmpty(PublicKey))
            {
                throw new ApplicationException("reCAPTCHA needs to be configured with a public key.");
            }
            var captchaBuilder = new TagBuilder("div");

            captchaBuilder.MergeAttribute("async","");
            captchaBuilder.MergeAttribute("defer","");
            captchaBuilder.MergeAttribute("id", id);
            captchaBuilder.MergeAttribute("class", "g-recaptcha");
            captchaBuilder.MergeAttribute("data-sitekey", PublicKey);
            if (!string.IsNullOrEmpty(theme))
            {
                captchaBuilder.MergeAttribute("data-theme", theme);
            }
            

            // Create tag builder
            var scriptBuilder = new TagBuilder("script");

            scriptBuilder.MergeAttribute("src", RecaptchaJsHost);

            // Add attributes
            scriptBuilder.MergeAttribute("type", "text/javascript");

            return MvcHtmlString.Create(captchaBuilder.ToString(TagRenderMode.Normal) + scriptBuilder.ToString(TagRenderMode.Normal));

        }

    }
}