using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureBootcamp2017DemoWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.EnvironmentLocation = "Error";

            try
            {
                //string env = Environment.GetEnvironmentVariable("region_setting");
                var config = ConfigurationManager.AppSettings["region_setting"];
                if (config != null)
                {
                    string env = config.ToString();
                     ViewBag.EnvironmentLocation = env;
                }

            }
            catch(Exception)
            {

            }

            return View();
        }
    }
}
