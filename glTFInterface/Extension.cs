using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharedGeometry;

namespace glTFInterface
{
    public class Extension
    {
        public string name { get; set; } = string.Empty;
        public virtual string ToJSON()
        {
            return string.Empty;
        }
    }
    public class OGC_CitySemanticCore : Extension
    {
        public string extensionName { get; } = "OGC_City_Semantic_Core";
        public string extensionVersion { get; } = "0.5.3";
        public OGC_CitySemanticCore(string name, double lat, double lon, double h, double radius)
        {
            this.name = name;
            this.geoPose.Position.lat = lat;
            this.geoPose.Position.lon = lon;
            this.geoPose.Position.h = h;
            this.radius = radius;
        }
        public GeoPose.Basic geoPose { get; set; } = new GeoPose.Basic("root");
        public double radius { get; set; } = 0.0;   
        public override string ToJSON()
        {
            return string.Empty;
        }

    }
}
