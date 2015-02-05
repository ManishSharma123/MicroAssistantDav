using System.Web;
using System.Web.Mvc;

namespace MicroAssistant.Web.Utility
{
    public class UrlManager
    {
        private static string GetCurrentUrl(string action, string controller, System.Web.Routing.RouteValueDictionary dictionary)
        {
            var urlHelper = new UrlHelper(((MvcHandler)HttpContext.Current.CurrentHandler).RequestContext);
            var url = urlHelper.Action(action, controller, dictionary);

            var baseURI = HttpContext.Current.Request.Url.Scheme + System.Uri.SchemeDelimiter +
                         HttpContext.Current.Request.Url.Host +
                         (HttpContext.Current.Request.Url.Port != 80 ? ":" + HttpContext.Current.Request.Url.Port : "");

            return baseURI + url;
        }

        #region Common Urls

        //Get account activation Url
        public static string GetAccountActivatoinUrl(string userName, string accountCreateDate)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            code = accountCreateDate,
                            name = userName
                        });

            return GetCurrentUrl("VerifyAccount", "Account", urlParams);
        }

        //Get terms of use url
        public static string GetTermsUrl()
        {
            return GetCurrentUrl("terms", "help", null);
        }

        //Get privacy url
        public static string GetPrivacyUrl()
        {
            return GetCurrentUrl("privacy", "help", null);
        }

        //Get copyright url
        public static string GetCopyrightUrl()
        {
            return GetCurrentUrl("copyright", "help", null);
        }

        //Get copyright url
        public static string GetContactusUrl()
        {
            return GetCurrentUrl("contactus", "home", null);
        }

        //Get forgot password Url
        public static string GetPasswordResetUrl(string userName, string accountCreateDate)
        {
            var urlParams =
                    new System.Web.Routing.RouteValueDictionary(
                        new
                        {
                            uname = userName,
                            codedt = accountCreateDate
                            
                        });

            return GetCurrentUrl("resetpassword", "account", urlParams);
        }

        #endregion
        
        public static string GetHomePageUrl()
        {
            return GetCurrentUrl("index", "home",null);
        }
    }
}
