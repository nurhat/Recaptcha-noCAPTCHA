using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcGooogleNoCaptcha.Infrastructure.Recaptcha
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class RecaptchaValidationActionFilterAttribute : ActionFilterAttribute
    {

        public static string RecaptchaPrivateKey
        {
            get { return ConfigurationManager.AppSettings["RecaptchaPrivateKey"]; }
        }
        public static string RecaptchaResponseFieldKey
        {
            get { return ConfigurationManager.AppSettings["RecaptchaResponseFieldKey"]; }
        }

        public static bool RecaptchaSkipValidation
        {
            get
            {
                bool recaptchaSkipValidation = false;
                bool.TryParse(ConfigurationManager.AppSettings["RecaptchaSkipValidation"], out recaptchaSkipValidation);
                return recaptchaSkipValidation;
            }
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (RecaptchaSkipValidation)
            {
                filterContext.ActionParameters["recaptchaValid"] = true;
            }
            else
            {
                if (string.IsNullOrEmpty(RecaptchaPrivateKey))
                {
                    throw new ApplicationException("reCAPTCHA  private key is missing");
                }

                RecaptchaValidator validator = new Recaptcha.RecaptchaValidator();

                var remoteIP = filterContext.HttpContext.Request.UserHostAddress;
                var recaptchaResponseFieldValue = filterContext.HttpContext.Request.Params[RecaptchaResponseFieldKey];

                var recaptchaResponse = validator.Validate(RecaptchaPrivateKey, remoteIP, recaptchaResponseFieldValue);

                // this will push the result values into a parameter in our Action
                filterContext.ActionParameters["recaptchaValid"] = recaptchaResponse.Success;
                filterContext.ActionParameters["recaptchaErrorMessage"] = recaptchaResponse.ErrorMessage;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}