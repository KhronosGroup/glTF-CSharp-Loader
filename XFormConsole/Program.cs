using System.Numerics;

using GSR00;
using OpenTopo;
namespace XFormConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
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
            GSR00.TopocentricFrame.EPSG4327ToEPSG4978(ellipsoid, lat, lon, height, out x, out y, out z);
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
            double deltaX, deltaY, deltaZ;
            double expectedX, expectedY, expectedZ;
            GSR00.TopocentricFrame.EPSG4327ToEPSG4978(ellipsoid, lat + deltaLat, lon + deltaLon, height + deltaH, out xt, out yt, out zt);
            double dist = Math.Sqrt((x - xt) * (x - xt) + (y - yt) * (y - yt) + (z - zt) * (z - zt));
            // convert test point back to EPSG4327
            GSR00.TopocentricFrame.EPSG4978ToEPSG4327(ellipsoid, xt, yt, zt, out tLat, out tLon, out tH);

            // convert test point to enu
            GSR00.TopocentricFrame.EPSG4978ToEPSG4979(topoFrame, xt, yt, zt, out double xENU, out double yENU, out double zENU);

            // convert test point back to ecef
            GSR00.TopocentricFrame.EPSG4979ToECEF(topoFrame, xENU, yENU, zENU, out xt, out yt, out zt);
            // Convert test point back to EPSG4327
            GSR00.TopocentricFrame.EPSG4978ToEPSG4327(ellipsoid, xt, yt, zt, out tLat, out tLon, out tH);
            expectedZ = deltaH;
            double xScale = 111556.58;
            double yScale = 111242.71;
            double bestXScale = 111556.58;
            double bestYScale = 111242.71;
            double aveDeltaX = 100.0;
            double aveDeltaY = 100.0;
            double aveDeltaH = 100.0;
            double totalDeltaX = 0.0;
            double totalDeltaY = 0.0;
            double totalDeltaH = 0.0;
            int nSamples = 0;
            int nIterations = 0;
            int nDeviations = 100000;
            int bestNDeviations = 100000;
            while(nIterations < 100 && nDeviations > 0)
            {
                nSamples = 0;
                totalDeltaX = 0.0;
                totalDeltaY = 0.0;
                totalDeltaH = 0.0;
                nDeviations = 0;
                for (int nRow = -50; nRow < 50; nRow++)
                {
                    deltaY = nRow * 10.0;
                    expectedY = deltaY;
                    deltaLat = deltaY / yScale;
                    for (int nCol = -50; nCol < 50; nCol++)
                    {
                        nSamples++;
                        deltaX = nCol * 10.0;
                        expectedX = deltaX;
                        deltaLon = (deltaX / Math.Cos(lat * TopocentricFrame.DegToRadians)) / xScale;
                        // convert test point to ECEF
                        GSR00.TopocentricFrame.EPSG4327ToEPSG4978(ellipsoid, lat + deltaLat, lon + deltaLon, height + deltaH, out xt, out yt, out zt);
                        // convert ECEF to ENU
                        GSR00.TopocentricFrame.EPSG4978ToEPSG4979(topoFrame, xt, yt, zt, out xENU, out yENU, out zENU);
                        totalDeltaX += (xENU - expectedX);
                        totalDeltaY += (yENU - expectedY);
                        totalDeltaH += (zENU - expectedZ);
                        double difX = Math.Sqrt((xENU - expectedX) * (xENU - expectedX));
                        double difY = Math.Sqrt((yENU - expectedY) * (yENU - expectedY));
                        double difH = Math.Sqrt((zENU - expectedZ) * (zENU - expectedZ));
                        double dif = Math.Sqrt((xENU - expectedX) * (xENU - expectedX) + (yENU - expectedY) * (yENU - expectedY) + (zENU - expectedZ) * (zENU - expectedZ));
                        if(difH < 0.0001)
                        {
                            int hh = 0;
                        }
                        if (difX > 0.1 || difY > 0.1 || difH > 0.1 || dif > 0.1)
                        {
                            nDeviations++;
                            //Console.WriteLine((lat + deltaLat).ToString("f6") + " " + (lon + deltaLon).ToString("f6") + " " + xENU.ToString("f6"));
                        }
                    }
                }
                aveDeltaX = totalDeltaX / (double)nSamples;
                aveDeltaY = totalDeltaY / (double)nSamples;
                aveDeltaH = totalDeltaH / (double)nSamples;

                if (nDeviations < bestNDeviations)
                {
                    bestNDeviations = nDeviations;  
                    bestXScale = xScale;
                    bestYScale = yScale;
                }
                else
                {
                    break;
                }

                xScale += aveDeltaX * 5.0;
                yScale += aveDeltaY * 5.0;
            }
            //eudem25m?locations=50.936764404085416,-1.4702295790037962
            Elevation[] path = new Elevation[2];
            await OpenTopo.ElevationRequests.RequestElevationOnPath("eudem25m", path);
        }
    }
}