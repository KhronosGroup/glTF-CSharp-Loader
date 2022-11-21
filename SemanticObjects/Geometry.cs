namespace Geometry
{
    public class Vertex
    {
        public double[] Coordinates { get; set; } = new double[3];
    }
    public class edge
    {

    }
    public class triangle
    {

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
