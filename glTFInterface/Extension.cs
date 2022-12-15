using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class OGC_Semantic_Core : Extension
    {
        public OGC_Semantic_Core(string name, double lat, double lon, double h, double radius)
        {
            this.name = name;
            this.lat = lat;
            this.lon = lon;
            this.h = h;
            this.radius = radius;
        }
        public double lat { get; set; } = 0.0;
        public double lon { get; set; } = 0.0;
        public double h { get; set; } = 0.0;
        public double radius { get; set; } = 0.0;   
        public override string ToJSON()
        {
            return string.Empty;
        }

    }
}
