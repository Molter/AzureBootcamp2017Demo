using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBootcamp2017DemoWebAPI.helpers
{
    public class ButtonRegion
    {
        public string RegionName;
        public string Color;

        public ButtonRegion()
        {

        }
        public ButtonRegion(string regionName, string color)
        {
            RegionName = regionName;
            Color = color;
        }
    }
}