using MvcGooogleNoCaptcha.Infrastructure.Recaptcha;
using MvcGooogleNoCaptcha.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcGooogleNoCaptcha.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [RecaptchaValidationActionFilter]
        [HttpPost]
        public ActionResult Index(IndexModel model, bool recaptchaValid, string recaptchaErrorMessage)
        {
            if (!ViewData.ModelState.IsValid)
            {
                return View("Index", model);
            }
            else if (!recaptchaValid)
            {
                ViewData.ModelState.AddModelError("_recaptchaError", recaptchaErrorMessage);
                return View("Index", model);
            }
            return View("Welcome", model);
        }
        

    }
}