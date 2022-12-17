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
        public string uri { get; set; } = string.Empty;
        public long minUnixTime { get; set; } = DateTime.Now.Ticks;
        public long maxUnixTime { get; set; } = DateTime.Now.Ticks;
        public virtual string ToJSON(string indent = "")
        {
            return string.Empty;
        }
    }
    public class OGC_SemanticCore : Extension
    {
        public string extensionName { get; } = "OGC_City_Semantic_Core";
        public string extensionVersion { get; } = "0.5.3";
        public OGC_SemanticCore(string name, string uri, double lat, double lon, double h, double yaw, double pitch, double roll, double radius)
        {
            this.name = name;
            this.uri = uri;
            this.geoPose.Position.lat = lat;
            this.geoPose.Position.lon = lon;
            this.geoPose.Position.h = h;
            this.geoPose.YPRAngles.yaw = yaw;
            this.geoPose.YPRAngles.pitch = pitch;
            this.geoPose.YPRAngles.roll = roll;

            this.radius = radius;
        }
        public GeoPose.BasicYPR geoPose { get; set; } = new GeoPose.BasicYPR("root");
        public double radius { get; set; } = 0.0;
        public override string ToJSON(string indent = "")
        {
            long minUnixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            Thread.Sleep(1000);
            long maxUnixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
            StringBuilder sb = new StringBuilder();
            sb.Append("\"OGC_Semantic_Core\" : {");
            sb.Append("\r\n\t" + indent + "\"gsr_uri\":   " + "\"" + this.uri + "\"");
            sb.Append(",\r\n\t" + indent + "\"gsr_name\":  " + "\"" + this.name + "\"");
            sb.Append(",\r\n\t" + indent + "\"gsr_radius\": " + this.radius.ToString("F1"));
            sb.Append(",\r\n\t" + indent + "\"gsr_minUnixTime\": " + minUnixTime);
            sb.Append(",\r\n\t" + indent + "\"gsr_maxUnixTime\": " + maxUnixTime);
            sb.Append(",\r\n\t" + indent + "\"gsr_geopose\": " + this.geoPose.ToJSON(indent));
            sb.Append("\r\n" + indent + "}");
            return sb.ToString();
        }

    }
    public class OGC_SemanticNode : Extension
    {
        public string extensionName { get; } = "OGC_City_Semantic_Node";
        public string extensionVersion { get; } = "0.5.3";
        public OGC_SemanticNode(string name, double lat, double lon, double h, double radius)
        {
            this.name = name;
            this.geoPose.Position.lat = lat;
            this.geoPose.Position.lon = lon;
            this.geoPose.Position.h = h;
            this.radius = radius;
        }
        public GeoPose.BasicYPR geoPose { get; set; } = new GeoPose.BasicYPR("root");
        public double radius { get; set; } = 0.0;
        public override string ToJSON(string indent = "")
        {
            return string.Empty;
        }

    }
}
