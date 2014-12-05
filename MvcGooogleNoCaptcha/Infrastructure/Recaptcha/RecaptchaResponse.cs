using Newtonsoft.Json;
using System;
using System.Collections.Generic;
namespace MvcGooogleNoCaptcha.Infrastructure.Recaptcha
{
    /// <summary>
    /// Encapsulates a response from reCAPTCHA web service.
    /// </summary>
    public class RecaptchaResponse
    {
        public static readonly RecaptchaResponse RecaptchaInvalidServiceResponse = new RecaptchaResponse() { Success = false, ErrorCodes = { "Invalid_reCAPTCHA_Service_response" } };
        public static readonly RecaptchaResponse RecaptchaNotReachable = new RecaptchaResponse() { Success = false, ErrorCodes = { "reCAPTCHA_Server_Is_Unavailable" } };


        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
        public string ErrorMessage
        {
            get
            {
                return string.Join(";", ErrorCodes);
            }
        }
        public RecaptchaResponse()
        {

            ErrorCodes = new List<string>();
        }
    }
}