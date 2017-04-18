using AzureBootcamp2017DemoWebAPI.helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AzureBootcamp2017DemoWebAPI.Controllers
{
    public class HomeController : Controller
    {

        static List<ButtonRegion> _buttomColorItems;
        static PerformanceCounter cpuCounter;
        static PerformanceCounter ramCounter;

        static bool keepRunnig = false;


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
            catch (Exception)
            {
                ViewBag.EnvironmentLocation = "Error";
                ViewBag.EnvironmentLocationCss = "default";
            }

            ViewBag.usedCPU = '0';

            return View();
        }

        private string readJsonConfig(string env)
        {
            try
            {
                if (_buttomColorItems == null)
                {
                    var json = System.IO.File.ReadAllText(Server.MapPath(@"~/App_Data/RegionColorMapping.json"));
                    _buttomColorItems = JsonConvert.DeserializeObject<List<ButtonRegion>>(json);
                }

                var region = _buttomColorItems.Find(x => x.RegionName == env);

                if (region == null)
                {
                    return "default";
                }

                return region.Color;

            }
            catch (Exception e)
            {
                return "default";
            }
        }

        public float getCurrentCpuUsage()
        {
            if(cpuCounter == null)
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time");
            }
            return cpuCounter.NextValue();
        }

        public float getAvailableRAM()
        {
            if(ramCounter == null)
            {
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            }

            return ramCounter.NextValue();
        }

        public JsonResult getCPU()
        {
            PerformanceItems pi = new PerformanceItems();
            float cpu = getCurrentCpuUsage();
            float ram = getAvailableRAM();
            int cpuInt = (int)cpu;
            int ramInt = (int)ram;

            pi.CPU = cpuInt.ToString();
            pi.RAM = ramInt.ToString();

            return Json(pi);

            // return JsonConvert.SerializeObject(pi);
        }

        public JsonResult increaseCPU()
        {
            keepRunnig = true;
            int threads = Environment.ProcessorCount * Environment.ProcessorCount;
            if (threads <= 4)
            {
                threads = 8;
            }
            for (int i = 0; i < threads; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc));
            }

            return Json("sucess");
        }

        public JsonResult resetCPU()
        {
            keepRunnig = false;

            return Json("sucess");
        }



        static void ThreadProc(Object stateInfo)
        {
            List<int> list = new List<int>();
            while (keepRunnig)
            {
                for (int i = 0; i < 99999; i++)
                {
                    if (!keepRunnig)
                    {
                        break;
                    }

                    int fb = Fibonacci(i) + Fibonacci(i);
                    int fb2 = Fibonacci(i) + Fibonacci(i);
                    list.Add(fb);
                    list.Add(fb2);

                }
            }
        }

        public static int Fibonacci(int n)
        {
            if (!keepRunnig)
            {
                return 0;
            }

            int a = 0;
            int b = 1;
            // In N steps compute Fibonacci sequence iteratively.
            for (int i = 0; i < n; i++)
            {
                int temp = a;
                a = b;
                b = temp + b;
            }
            return a;
        }


    }
}