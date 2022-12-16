using System.Text;

namespace SharedGeometry
{


        public class Distance
        {
            double Value { get; set; }
        }

        /*
         * 

         * 
         */
        public class GeneratedSphere : Mesh
        {
            Mesh SphereMesh { get; set; }
            // https://en.wikipedia.org/wiki/Tetrahedron#Formulas_for_a_regular_tetrahedron is source of constants
            public GeneratedSphere(double radius, double[] center, double maxEdge)
            {
                // regular tetrahedron inscribed in a radius radius sphere centered at 0.0
                // vertex at 0, 0, radius and base at z = -radius*3
                Node p0 = new Node(radius * Math.Sqrt(8.0 / 9.0), 0.0, -radius / 3.0);
                Node p1 = new Node(-radius * Math.Sqrt(2.0 / 9.0), radius * Math.Sqrt(2.0 / 3.0), -radius / 3.0);
                Node p2 = new Node(-radius * Math.Sqrt(2.0 / 9.0), -radius * Math.Sqrt(2.0 / 3.0), -radius / 3.0);
                Node p3 = new Node(0.0, 0.0, radius);
                // translate to center
                for (int nCoord = 0; nCoord < 3; nCoord++)
                {
                    p0.Coordinates[nCoord] += center[nCoord];
                    p1.Coordinates[nCoord] += center[nCoord];
                    p2.Coordinates[nCoord] += center[nCoord];
                    p3.Coordinates[nCoord] += center[nCoord];
                }
                SphereMesh = new Mesh();
                SphereMesh.nodes.Nodes.Add(p0);
                SphereMesh.nodes.Nodes.Add(p1);
                SphereMesh.nodes.Nodes.Add(p2);
                SphereMesh.nodes.Nodes.Add(p3);
                SphereMesh.edges.Edges.Add(new Edge(0, 1));
                SphereMesh.edges.Edges.Add(new Edge(1, 2));
                SphereMesh.edges.Edges.Add(new Edge(2, 0));
                SphereMesh.edges.Edges.Add(new Edge(0, 3));
                SphereMesh.edges.Edges.Add(new Edge(3, 1));
                SphereMesh.edges.Edges.Add(new Edge(3, 2));
                SphereMesh.triangles.Triangles.Add(new Triangle(2, -6, 5));
                SphereMesh.triangles.Triangles.Add(new Triangle(3, 4, 6));
                SphereMesh.triangles.Triangles.Add(new Triangle(1, -5, -4));
                SphereMesh.triangles.Triangles.Add(new Triangle(1, 2, 3));
            }
            public Mesh GetMesh()
            {
                return SphereMesh;
            }
        }

        public class Node
        {
            public double[] Coordinates { get; set; } = new double[3];
            public Node(double x, double y, double z)
            {
                Coordinates[0] = x;
                Coordinates[1] = y;
                Coordinates[2] = z;
            }
            public Node(double[] coordinates)
            {
                Coordinates = coordinates;
            }
        }
        public class Edge
        {
            public int Start { get; set; }
            public int End { get; set; }
            public Edge(int start, int end)
            {
                Start = start;
                end = End;
            }
        }
        public class Triangle
        {
            public Triangle(int edge0, int edge1, int edge2)
            {
                Edges[0] = edge0;
                Edges[1] = edge1;
                Edges[2] = edge2;
                Neighbors[0] = Neighbors[1] = Neighbors[2] = -1;
            }
            public Triangle(int[] edges)
            {
                Edges = edges;
                Neighbors[0] = Neighbors[1] = Neighbors[2] = -1;
            }
            public Triangle(int[] edges, int[] neighbors)
            {
                Edges = edges;
                Neighbors = neighbors;
            }
            public int[] Edges { get; set; } = new int[3];
            public int[] Neighbors { get; set; } = new int[3];
        }
        public class NodeStore
        {
            public List<Node> Nodes { get; set; } = new List<Node>();
        }
        public class EdgeStore
        {
            public List<Edge> Edges { get; set; } = new List<Edge>();
        }
        public class TriangleStore
        {
            public List<Triangle> Triangles { get; set; } = new List<Triangle>();
        }
        public class Mesh
        {
            public NodeStore nodes { get; set; } = new NodeStore();
            public EdgeStore edges { get; set; } = new EdgeStore();
            public TriangleStore triangles { get; set; } = new TriangleStore();
            // material
            // array of triangles
        }
    }
namespace GeoPose
{
    public class Position
    {
        public double lat { get; set; } = double.NaN;
        public double lon { get; set; } = double.NaN;
        public double h { get; set; } = double.NaN;
    }
    public class YPRAngles
    {
        public double yaw { get; set; } = double.NaN;
        public double pitch { get; set; } = double.NaN;
        public double roll { get; set; } = double.NaN;
    }
    public class Quaternion
    {
        public double x { get; set; } = double.NaN;
        public double y { get; set; } = double.NaN;
        public double z { get; set; } = double.NaN;
        public double w { get; set; } = double.NaN;
    }
    public abstract class GeoPose
    {
        public string Name { get; set; } = "";
    }
    public class BasicYPR : GeoPose
    {
        public BasicYPR(string aName)
        {
            Name = aName;
        }
        public Position Position { get; set; } = new Position();
        public YPRAngles YPRAngles { get; set; } = new YPRAngles();
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


        public string ToJSON(string indent = "")
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\r\n\t" + indent + "{\"position\":{\"lat\":" + Position.lat + ",\"lon\":" + Position.lon + ",\"h\":" + Position.h + "},");
            sb.Append("\r\n\t" + indent + "\"angles\":{\"yaw\":" + YPRAngles.yaw + ",\"pitch\":" + YPRAngles.pitch + ",\"roll\":" + YPRAngles.roll);
            sb.Append("\r\n\t\t" + indent + "}\r\n\t" + indent + "}");
            return sb.ToString();
        }
    }
    public class FrameSpecification
    {
        public string authority { get; set; } = "";
        public string id { get; set; } = "";
        public string parameters { get; set; } = "";
    }
    public class Advanced : GeoPose
    {
        public Advanced(string aName)
        {
            Name = aName;
        }
        public FrameSpecification frameSpecification { get; set; } = new FrameSpecification();
        public Quaternion Quaternion { get; set; } = new Quaternion();
        /*
  {
  "frameSpecification": {
    "authority": "/geopose/1.0",
    "id": "LTP-ENU",
    "parameters": "longitude=-122.0000000&latitude=48.0000000&height=0.000"
  },
  "quaternion": {
    "x": -0.14531824386668632,
    "y": -0.03245644116944548,
    "z": 0.9650744852300841,
    "w": -0.21554680555277317
  },
  "validTime": 1669671566669
}
         * 
         */


        public string ToJSON()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"frameSpecification\":{\"authority\":" + frameSpecification.authority + ",\"id\":" + frameSpecification.id + ",\"parameters\":" + frameSpecification.parameters + "},");
            sb.Append("\"quaternion\":{\"x\":" + Quaternion.x + ",\"y\":" + Quaternion.y + ",\"z\":" + Quaternion.z + ",\"w\":" + Quaternion.w);
            sb.Append("}}");
            return sb.ToString();
        }
    }

}
