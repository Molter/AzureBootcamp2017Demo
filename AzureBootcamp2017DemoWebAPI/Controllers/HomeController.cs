using AzureBootcamp2017DemoWebAPI.helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureBootcamp2017DemoWebAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            try
            {
                //string env = Environment.GetEnvironmentVariable("region_setting");
                var config = ConfigurationManager.AppSettings["region_setting"];
                if (config != null)
                {
                    string env = config.ToString();
                    ViewBag.EnvironmentLocation = env;

                    ViewBag.EnvironmentLocationCss = readJsonConfig(env);
                }

            }
            catch(Exception)
            {
                ViewBag.EnvironmentLocation = "Error";
                ViewBag.EnvironmentLocationCss = "default";
            }

            return View();
        }

        private string readJsonConfig(string env)
        {
            try
            {
                var json = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/RegionColorMapping.json"));
                List<ButtonRegion> items = JsonConvert.DeserializeObject<List<ButtonRegion>>(json);

                var region = items.Find(x => x.RegionName == env);

                if(region == null)
                {
                    return "default";
                }

                return region.Color;
                
            }catch (Exception e)
            {
                return "default";
            }
        }
    }
}
