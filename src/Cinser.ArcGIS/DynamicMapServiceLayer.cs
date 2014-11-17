using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Client;

namespace Cinser.ArcGIS
{
    public class DynamicMapServiceLayer : ArcGISDynamicMapServiceLayer
    {
        Map myMap;

        public Map MyMap
        {
            get { return myMap; }
            set { myMap = value; }
        }

        public DynamicMapServiceLayer(string url, Map myMap)
        {
            this.MyMap = myMap;
            this.Url = url;
            this.Initialized += new EventHandler<EventArgs>(CinserDynamicMapServiceLayer_Initialized);
            this.InitializationFailed += new EventHandler<EventArgs>(CinserDynamicMapServiceLayer_InitializationFailed);
            this.Initialize();        
        }

        void CinserDynamicMapServiceLayer_InitializationFailed(object sender, EventArgs e)
        {

        }

        void CinserDynamicMapServiceLayer_Initialized(object sender, EventArgs e)
        {
            if (this.InitializationFailure == null)
            {
                //设置默认的透明度
                this.Opacity = 0.8;
                this.MyMap.Layers.Add(this);
            }
        }
    }
}
