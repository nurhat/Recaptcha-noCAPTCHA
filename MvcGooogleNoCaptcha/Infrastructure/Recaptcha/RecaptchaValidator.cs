using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;

namespace MvcGooogleNoCaptcha.Infrastructure.Recaptcha
{
    /// <summary>
    /// Calls the reCAPTCHA server to validate the answer.
    /// </summary>
    public class RecaptchaValidator
    { 
        private void CheckNotNull(object obj, string name)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        public RecaptchaResponse Validate(string privateKey, string remoteIP, string recaptchaResponseFieldValue)
        {
            this.CheckNotNull(privateKey, "privateKey");
            this.CheckNotNull(recaptchaResponseFieldValue, "recaptchaResponseFieldValue");

            if (string.IsNullOrEmpty(recaptchaResponseFieldValue))
            {
                return new RecaptchaResponse() { Success = false, ErrorCodes = { "missing-input-response" } };
            }
            string serviceUri = String.Format("{0}?secret={1}&remoteip={2}&response={3}",
                                     ConfigurationManager.AppSettings["RecaptchaVerifyUrl"],
                                    HttpUtility.UrlEncode(privateKey),
                                    HttpUtility.UrlEncode(remoteIP),
                                    HttpUtility.UrlEncode(recaptchaResponseFieldValue));

            IWebProxy proxy = WebRequest.GetSystemWebProxy();
            proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(serviceUri);
            request.Proxy = proxy;
            request.Timeout = 30 * 1000;
            request.Method = "GET"; 
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = 0;

            RecaptchaResponse response = null;
            try
            {
                using (WebResponse httpResponse = request.GetResponse())
                {
                    string result;
                    using (TextReader readStream = new StreamReader(httpResponse.GetResponseStream(), Encoding.UTF8))
                    {
                        result = readStream.ReadToEnd();
                    }
                    response = JsonConvert.DeserializeObject<RecaptchaResponse>(result);
                }
            }
            catch (WebException ex)
            {
                response.ErrorCodes.Add(ex.ToString());
                response = RecaptchaResponse.RecaptchaNotReachable;
            }
            catch (Exception ex)
            {
                response.ErrorCodes.Add(ex.ToString());
                response = RecaptchaResponse.RecaptchaInvalidServiceResponse;
            }

            return response;
        }
    }
}