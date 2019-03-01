using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.WebApi;
using Umbraco.Core.Logging;

namespace UmbracoBase4.App_Plugins.Form
{
    [PluginController("MyForm")]
    public class FormController : UmbracoApiController
    {
        public IEnumerable<string> GetAllProducts()
        {
            return new[] { "Table", "Chair", "Desk", "Computer", "Beer fridge" };
        }

        public object SendFormAMP()
        {
            var name = HttpContext.Current.Request.Form["name"];
            var email = HttpContext.Current.Request.Form["email"];
            var message = HttpContext.Current.Request.Form["message"];
            var errorMessage = string.Empty;
            HttpContext.Current.Response.Headers.Add("AMP-Access-Control-Allow-Source-Origin", this.Request.RequestUri.Scheme + "://" + this.Request.RequestUri.Host);

            HttpContext.Current.Response.Headers.Add("Access-Control-Expose-Headers", "AMP-Redirect-To, AMP-Access-Control-Allow-Source-Origin");

            if (IpFilter())
            {
                errorMessage = "Wysłano maksymalną, dzienną ilość wiadomości z twojego adresu IP (" + GetIpAddress() + "). Proszę spróbuj ponownie jutro.";

                //Logger.Error(this.GetType(), ViewBag.ErrorMessage, new Exception());
            }
            else
            {

                if (ValidateForm(name, email, message))
                {
                    try
                    {
                        SendEmail(email, name, message);
                        HttpContext.Current.Response.Headers.Add("AMP-Redirect-To", "//" + HttpContext.Current.Request.Url.Host + "/FormularzWyslany");
                        return new { Success = true };
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "Przepraszamy wystąpił błąd podczas wysyłania wiadomości. Prosimy o&nbsp;kontakt telefoniczny lub za&nbsp;pomocą adresu email: <a href='mailTo:kancelaria@adwokat-szkudlarek.pl' title='mailTo:kancelaria@adwokat-szkudlarek.pl'>mailTo:kancelaria@adwokat-szkudlarek.pl</a>";
                        LogHelper.Error(typeof(FormController), errorMessage, ex);
                        return Json(new { Success = false, ErrorMessage = ex.Message });
                    }
                }
                else
                {
                    errorMessage = "Wypełnij wymagane pola!";
                }
            }
            return new { Success = false, Message = errorMessage };
        }
        
        public string SendForm([FromBody] FormData data)
        {
            var errorMessage = string.Empty;
            if (IpFilter())
            {
                errorMessage = "Wysłano maksymalną, dzienną ilość wiadomości z twojego adresu IP (" + GetIpAddress() + "). Proszę spróbuj ponownie jutro.";

                //Logger.Error(this.GetType(), ViewBag.ErrorMessage, new Exception());
            }
            else
            {
                if (ValidateForm(data.Name, data.Email, data.Message))
                {
                    try
                    {
                        SendEmail(data.Email,data.Name,data.Message);
                        HttpContext.Current.Response.Redirect("//" + HttpContext.Current.Request.Url.Host + "/FormularzWyslany");
                        return null;
                    }
                    catch (Exception ex)
                    {   
                        errorMessage = "Przepraszamy wystąpił błąd podczas wysyłania wiadomości. Prosimy o&nbsp;kontakt telefoniczny lub za&nbsp;pomocą adresu email: <a href='mailTo:kancelaria@adwokat-szkudlarek.pl' title='mailTo:kancelaria@adwokat-szkudlarek.pl'>mailTo:kancelaria@adwokat-szkudlarek.pl</a>";
                        LogHelper.Error(typeof(FormController), errorMessage, ex);
                    }
                }
                else
                {
                    errorMessage = "Wypełnij wymagane pola!";
                }
            }

            HttpContext.Current.Response.Redirect("//" + HttpContext.Current.Request.Url.Host + "/Kontakt?message=" + errorMessage);
            return null;
        }

        private static void SendEmail(string emailTo, string name, string message)
        {
            MailMessage emailMessage = new MailMessage();
            emailMessage.From = new MailAddress(emailTo);
            emailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["ContactEmail"]));
            emailMessage.Bcc.Add(new MailAddress("mssycz@gmail.com"));
            emailMessage.Subject = string.Format("Porada Online - {0}", name);
            emailMessage.Body = message;
            SmtpClient client = new SmtpClient();
            client.Send(emailMessage);
        }

        private bool IpFilter()
        {
            var ip = GetIpAddress();

            var ipValue = HttpContext.Current.Cache[ip];
            if (ipValue != null)
            {
                if ((int)ipValue > 3)
                {
                    return true;
                }
                else
                {
                    ipValue = (int)ipValue + 1;
                    HttpContext.Current.Cache.Remove(ip);
                    HttpContext.Current.Cache.Add(ip, ipValue, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
                }
            }
            else
            {
                HttpContext.Current.Cache.Add(ip, 1, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(24, 0, 0), System.Web.Caching.CacheItemPriority.Normal, null);
            }
            return false;
        }

        private bool ValidateForm(string name, string emailTo, string message)
        {

            //if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            //{
            //    ViewBag.ErrorMessage = "Podaj imię i&nbsp;nazwisko";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(emailTo) || string.IsNullOrWhiteSpace(emailTo))
            //{
            //    ViewBag.ErrorMessage = "Podaj adres email";
            //    return false;
            //}

            //if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            //{
            //    ViewBag.ErrorMessage = "Podaj treść wiadomości";
            //    return false;
            //}
            return true;
        }

        private string GetIpAddress()
        {
            string szRemoteAddr = HttpContext.Current.Request.UserHostAddress;
            string szXForwardedFor = HttpContext.Current.Request.ServerVariables["X_FORWARDED_FOR"];
            string szIP = "";

            if (szXForwardedFor == null)
            {
                szIP = szRemoteAddr;
            }
            else
            {
                szIP = szXForwardedFor;
                if (szIP.IndexOf(",") > 0)
                {
                    string[] arIPs = szIP.Split(',');

                    foreach (string item in arIPs)
                    {
                        if (!IsPrivateIP(item))
                        {
                            return item;
                        }
                    }
                }
            }
            return szIP;
        }

        private bool IsPrivateIP(string item)
        {
            if (item.Trim().Equals("localhost")
                || item.Trim().Equals("127.0.0.1"))
            {
                return true;
            }
            return false;
        }
    }
}