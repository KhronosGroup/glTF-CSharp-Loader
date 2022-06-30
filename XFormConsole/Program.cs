using GSR00;
namespace XFormConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double x, y, z;
            double xt, yt, zt;
            double tLat, tLon, tH;
            double lat = 50.936735;
            double lon = -1.470217;
            double height = 17.0;
            GSR00.Ellipsoid ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
            GSR00.EPSG4327 tangentPoint = new EPSG4327(lat, lon, height);
            GSR00.TopocentricFrame topoFrame = new TopocentricFrame(ellipsoid, tangentPoint);

            // tangent point in ecef
            GSR00.TopocentricFrame.EPSG4327ToECEF(ellipsoid, lat, lon, height, out x, out y, out z);
            /*
             * 
             * X : 4026183  4026182.68 m 
             * Y : -103335  -103335.04 m
             * Z : 4929126  4929125.59 m
             * 
             */
            // test point in ecef
            double deltaLat = 0.00234 / 1.11111;
            double deltaLon = (0.00125 / Math.Cos(lat * TopocentricFrame.DegToRadians))/1.11111;
            double deltaH = 43.21;
            GSR00.TopocentricFrame.EPSG4327ToECEF(ellipsoid, lat + deltaLat, lon + deltaLon, height + deltaH, out xt, out yt, out zt);
            double dist = Math.Sqrt((x - xt) * (x - xt) + (y - yt) * (y - yt) + (z - zt) * (z - zt));
            // convert test point back to EPSG4327
            GSR00.TopocentricFrame.ECEFToEPSG4327(ellipsoid, xt, yt, zt, out tLat, out tLon, out tH);

            // convert test point to enu
            GSR00.TopocentricFrame.ECEFToEPSG4979(topoFrame, xt, yt, zt, out double xENU, out double yENU, out double zENU);

            // convert test point back to ecef
            GSR00.TopocentricFrame.EPSG4979ToECEF(topoFrame, xENU, yENU, zENU, out xt, out yt, out zt);
            // Convert test point back to EPSG4327
            GSR00.TopocentricFrame.ECEFToEPSG4327(ellipsoid, xt, yt, zt, out tLat, out tLon, out tH);
            for(int nRow = -50; nRow < 50; nRow++)
            {
                deltaLat = nRow * 10.0;
                for (int nCol = -50; nCol < 50; nCol++)
                {
                    deltaLon = nCol * 10.0;
                    // convert test point to ECEF
                    GSR00.TopocentricFrame.EPSG4327ToECEF(ellipsoid, lat + deltaLat, lon + deltaLon, height + deltaH, out xt, out yt, out zt);
                    // convert ECEF to ENU
                    GSR00.TopocentricFrame.ECEFToEPSG4979(topoFrame, xt, yt, zt, out xENU, out yENU, out zENU);
                    Console.WriteLine((lat + deltaLat).ToString("f6") + " " + (lon + deltaLon).ToString("f6") + " " + xENU.ToString("f6"));
                }
            }
        }
    }
}