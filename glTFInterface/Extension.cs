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
    public class OGC_Semantic_Overlay : Extension
    {
        public OGC_Semantic_Overlay(string name, double lat, double lon, double h, double radius)
        {
            this.name = name;
        }
        public override string ToJSON()
        {
            return string.Empty;
        }

    }
}
