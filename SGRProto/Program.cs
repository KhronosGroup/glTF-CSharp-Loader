using System;
using System.IO;
using System.Numerics;

using SGR00;
using OpenTopo;
using NetTopologySuite.Geometries;

namespace SGRProto
{
    public class Program
    {
        /// <summary>
        ///  
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // 50.9381388226438, -1.4707338064233055
            double lat = 50.938138822;
            double lon = -1.470733806;
            double height = 17.0;
            SGR00.Ellipsoid ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
            SGR00.EPSG4327 tangentPoint = new EPSG4327(lat, lon, height);
            SGR00.TopocentricFrame topoFrame = new TopocentricFrame(ellipsoid, tangentPoint);
            double[]? boundaryDoubles = { -1.472798723195476, 50.93925335906807, -1.468116750304808, 50.93922333038582, -1.468092923470759, 50.93886298468631, -1.468557546734719, 50.93885547745454, -1.468593286985793, 50.938269909642269, -1.468319278394227, 50.937879527004017, -1.469582100598834, 50.9360777185106, -1.472655762191181, 50.936085226190829, -1.472655762191181, 50.936085226190829, -1.472798723195476, 50.93925335906807 };
            int nCoordinates = boundaryDoubles.Length / 2;
            Coordinate[] coordinates = new Coordinate[nCoordinates];
            for (int nCoord = 0; nCoord < nCoordinates; nCoord++)
            {
                EPSG4327 geoPosition = new EPSG4327(boundaryDoubles[nCoord * 2 + 1], boundaryDoubles[nCoord * 2], height);
                EPSG4978 ecefPosition = TopocentricFrame.EPSG4327ToEPSG4978(ellipsoid, geoPosition);
                EPSG4979 enuPosition = TopocentricFrame.EPSG4978ToEPSG4979(topoFrame, ecefPosition);
                Coordinate coordinate = new Coordinate(enuPosition.east, enuPosition.north);
                coordinates[nCoord] = coordinate;
            }
            LinearRing ring = new LinearRing(coordinates);
            Polygon boundary = new Polygon(ring);
            GSR gsr = new GSR("", "", topoFrame);
            gsr.MinEast = -140.0;
            gsr.MaxEast = 140.0;
            gsr.MinNorth = -160.0;
            gsr.MaxEast = 160.0;
            gsr.MinValidUnixTime = "1657344100000";
            gsr.MaxValidUnixTime = "1657345100000";
            gsr.SerializeAsJSON("metadata.gltf");
            // build terrain using bounds and opentopo on a given number of random points
            gsr.BuildTerrain(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            // convert to mesh and save mesh in buffer
            // add terrain
            gsr.SerializeAsJSON("terrain.gltf");
            // add water
            gsr.BuildWater(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            gsr.SerializeAsJSON("water.gltf");
            // add transportation
            gsr.BuildTransportation(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            gsr.SerializeAsJSON("transportation.gltf");
            // add furniture
            gsr.BuildFurniture(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            gsr.SerializeAsJSON("furniture.gltf");
            // add vegetation
            gsr.BuildVegetation(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            gsr.SerializeAsJSON("vegetation.gltf");
            // add buildings
            gsr.BuildBuildingsAndConstruction(gsr.MinEast, gsr.MaxEast, gsr.MinNorth, gsr.MaxNorth, 4096);
            gsr.SerializeAsJSON("buildings.gltf");
        }
    }
}

