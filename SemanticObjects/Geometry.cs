using System.Text;

namespace Geometry
{
    public class Vertex
    {
        public double[] Coordinates { get; set; } = new double[3];
    }
    public class Edge
    {
        public Vertex[] Vertices { get; set; } = new Vertex[2];
        public Triangle[] Areas { get; set; } = new Triangle[2];
    }
    public class Triangle
    {
        public Vertex[] Vertices { get; set; } = new Vertex[3];
        public Edge[] Sides { get; set; } = new Edge[3];
        public Triangle[] Neighbors { get; set; } = new Triangle[3];
    }
    public class Mesh
    {
        // material
        // array of triangles
    }
    public class Position
    {
        public double[] Coordinates { get; set; } = new double[3];
    }
    public class Quaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;
    }
    public class GeoPose
    {
        public Position Position { get; set; } = new Position();
        public Quaternion Quaternion { get; set; } = new Quaternion();
        /*
  {
  "position": {
    "lat": 48,
    "lon": -122,
    "h": 0
  },
  "quaternion": {
    "x": 0.2054016057332686,
    "y": 0.2602252793489189,
    "z": 0.5845327055343223,
    "w": -0.7405501336916342
  }
}
         * 
         */


        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"position\":{\"lat\":" + Position.Coordinates[0] + ",\"lon\":" + Position.Coordinates[1] + ",\"h\":" + Position.Coordinates[2] + "},");
            sb.Append("\"quaternion\":{\"x\":" + Quaternion.x + ",\"y\":" + Quaternion.y + ",\"z\":" + Quaternion.z + ",\"w\":" + Quaternion.w);
            sb.Append("}}");
            return sb.ToString();
        }
    }
}
