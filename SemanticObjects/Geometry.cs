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

    }
    public class Position
    {
        public string CRS { get; set; } = "";
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
    }
}
