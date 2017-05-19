using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Crossover.app.CrossoverSvc;

namespace Crossover.app.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            CrossoverSvc.StockSvcSoapClient svcClient=new StockSvcSoapClient("StockSvcSoap");

            SecuredWebServiceHeader header = new SecuredWebServiceHeader()
            {
                Username = "",
                Password = ""
            };
            var token = svcClient.AuthenticateUser(header);
            var result = svcClient.GetStockPrice(header, "", "");
            return Content(result.ToString());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}