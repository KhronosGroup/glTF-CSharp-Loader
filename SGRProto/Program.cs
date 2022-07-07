using System;
using System.IO;
using System.Numerics;

using GSR00;
using OpenTopo;
using NetTopologySuite.Geometries;

namespace SGRProto
{
    internal class Program
    {
        public static void Main(string[] args)
        {
               public double lat { get; set; } = 50.936735;
        public static double lon { get; set; } = -1.470217;
        public static double height { get; set; } = 17.0;
        public static GSR00.Ellipsoid ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
        public static GSR00.EPSG4327 tangentPoint = new EPSG4327(lat, lon, height);
        public static GSR00.TopocentricFrame topoFrame = new TopocentricFrame(ellipsoid, tangentPoint);
        public static Coordinate[] coordinates = new Coordinate[20 / 2];
        public static LinearRing ring = new LinearRing(coordinates);
        public static Polygon boundary = new Polygon(ring);
        public static GSR gsr = new GSR("", "", topoFrame, boundary);
        Console.WriteLine("Hello, World!");
      
        //{
        /*          
          public static double[]? boundaryDoubles = null;//{ -1.472798723195476, 50.93925335906807, -1.468116750304808, 50.93922333038582, -1.468092923470759, 50.93886298468631, -1.468557546734719, 50.93885547745454, -1.468593286985793, 50.938269909642269, -1.468319278394227, 50.937879527004017, -1.469582100598834, 50.9360777185106, -1.472655762191181, 50.936085226190829, -1.472655762191181, 50.936085226190829, -1.472798723195476, 50.93925335906807 };

              public static double lat = 50.936735;
    public static double lon = -1.470217;
    public static double height = 17.0;
    public static GSR00.Ellipsoid ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
    public static GSR00.EPSG4327 tangentPoint = new EPSG4327(lat, lon, height);
    public static GSR00.TopocentricFrame topoFrame = new TopocentricFrame(ellipsoid, tangentPoint);
    public static Coordinate[] coordinates = new Coordinate[20 / 2];
    public static LinearRing ring = new LinearRing(coordinates);
    public static Polygon boundary = new Polygon(ring);
    public static GSR gsr = new GSR("", "", topoFrame, boundary);
    Console.WriteLine("Hello, World!");
    }
                              */

    }
}

