using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Web;
using System.Web.Services;

namespace Crossover.Services
{

    /// <summary>
    /// Soap Header for the Secured Web Service.
    /// Username and Password are required for AuthenticateUser(),
    ///   and AuthenticatedToken is required for everything else.
    /// </summary>
    public class SecuredWebServiceHeader : System.Web.Services.Protocols.SoapHeader
    {
        public string Username;
        public string Password;
        public string AuthenticatedToken;
    }


    /// <summary>
    /// Summary description for StockSvc
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StockSvc : System.Web.Services.WebService
    {
        private readonly int minValue = 1;
        private readonly int maxValue = 1000;


        public SecuredWebServiceHeader SoapHeader;
        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public string AuthenticateUser()
        {
            if (SoapHeader == null)
                return "Please provide a Username and Password";
            if (string.IsNullOrEmpty(SoapHeader.Username) || string.IsNullOrEmpty(SoapHeader.Password))
                return "Please provide a Username and Password";
            // Are the credentials valid?
            if (!IsUserValid(SoapHeader.Username, SoapHeader.Password))
                return "Invalid Username or Password";
            // Create and store the AuthenticatedToken before returning it
            string token = Guid.NewGuid().ToString();
            HttpRuntime.Cache.Add(
                token,
                SoapHeader.Username,
                null,
                System.Web.Caching.Cache.NoAbsoluteExpiration,
                TimeSpan.FromMinutes(60),
                System.Web.Caching.CacheItemPriority.NotRemovable,
                null);
            return token;
        }
        private bool IsUserValid(string Username, string Password)
        {
            // TODO: Implement Authentication
            return true;
        }
        private bool IsUserValid(SecuredWebServiceHeader SoapHeader)
        {
            if (SoapHeader == null)
                return false;
            // Does the token exists in our Cache?
            if (!string.IsNullOrEmpty(SoapHeader.AuthenticatedToken))
                return (HttpRuntime.Cache[SoapHeader.AuthenticatedToken] != null);
            return false;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
        public double GetStockPrice(string exchange, string symbol)
        {
            if (!IsUserValid(SoapHeader))
                throw new AuthenticationException("Please call AuthenitcateUser() method first.");

            Random random=new Random();
            var next = random.NextDouble();
            var roundVal = Math.Round(minValue + (next * (maxValue - minValue)), 2);
            return roundVal;
        }
    }
}
