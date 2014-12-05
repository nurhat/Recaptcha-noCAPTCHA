using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcGooogleNoCaptcha.Infrastructure.Recaptcha
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RecaptchaValidatorAttribute : ActionFilterAttribute
    {

        public static string PrivateKey
        {
            get { return ConfigurationManager.AppSettings["RecaptchaPrivateKey"]; }
        }
        public static string RecaptchaResponseFieldKey
        {
            get { return ConfigurationManager.AppSettings["RecaptchaResponseFieldKey"]; }
        }

        public static bool SkipRecaptcha
        {
            get
            {
                bool skipRecaptcha = false;
                bool.TryParse(ConfigurationManager.AppSettings["RecaptchaSkipValidation"], out skipRecaptcha);
                return skipRecaptcha;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SkipRecaptcha)
            {
                filterContext.ActionParameters["recaptchaValid"] = true;
            }
            else
            {
                if (string.IsNullOrEmpty(PrivateKey))
                {
                    throw new ApplicationException("reCAPTCHA needs to be configured with a private key.");
                }

                RecaptchaValidator validator = new Recaptcha.RecaptchaValidator();

                var remoteIP = filterContext.HttpContext.Request.UserHostAddress;
                var recaptchaResponseFieldValue = filterContext.HttpContext.Request.Params[RecaptchaResponseFieldKey];

                var recaptchaResponse = validator.Validate(PrivateKey, remoteIP, recaptchaResponseFieldValue);

                // this will push the result values into a parameter in our Action
                filterContext.ActionParameters["recaptchaValid"] = recaptchaResponse.Success;
                filterContext.ActionParameters["recaptchaErrorMessage"] = recaptchaResponse.ErrorMessage;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}