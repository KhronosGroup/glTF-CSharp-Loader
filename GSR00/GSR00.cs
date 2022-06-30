
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GSR00
{
    public struct EPSG4327
    {
        public EPSG4327(double aLat, double aLon, double aH)
        {
            lat = aLat;
            lon = aLon;
            h = aH;
        }
        public double lat { get; set; }
        public double lon { get; set; }
        public double h { get; set; }
        public void EPSG4327ToECEF(double lat, double lon, double h, out double x, out double y, out double z)
        {
            x = 0;
            y = 0;
            z = 0;
            double sin_lat = Math.Sin(lat * TopocentricFrame.DegToRadians);
        }
    }
    public struct EPSG4979
    {
        public EPSG4979(double anEast, double aNorth, double anUp)
        {
            east = anEast;
            north = aNorth;
            up = anUp;
        }
        public double east { get; set; }
        public double north { get; set; }
        public double up { get; set; }
    }
    public class WGS84ECEF
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public WGS84ECEF(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    public class TopocentricFrame
    {
        static public readonly double DegToRadians = Math.PI / 180.0;
        static public readonly double RadiansToDeg = 180.0 / Math.PI;
        public EPSG4327 tangentPoint { get; set; }
        public WGS84ECEF ecefTangentPoint { get; set; }

        public double radius { get; set; } // z value at origin of topocentric system
        public double sin_lat { get; set; } // sine of angle between normal and equatorial plane
        public double cos_lat { get; set; } // cosine of angle between normal and equatorial plane
        public double sin_lon { get; set; } // sine of angle between normal and plane of prime meridian
        public double cos_lon { get; set; } // cosine of angle between normal and plane of prime meridian
        public double sin_cos { get; set; } // sin(lat) * cos(lon)
        public double sin_sin { get; set; } // sin(lat)°sin(on))
        public double cos_cos { get; set; } // cos(lat)'cos(lon) 
        public double cos_sin { get; set; } // cos(lat)'sin(lon) 
        public static void ECEFToEPSG4327(Ellipsoid ellipsoid, double x, double y, double z, out double aLat, out double aLon, out double aH)
        {
            double zp = Math.Abs(z);
            double w2 = x * x + y * y;
            double w = Math.Sqrt(w2);
            double r2 = w2 + z * z;
            double r = Math.Sqrt(r2);
            aLon = Math.Atan2(y, x);       
            double s2 = z * z / r2;
            double c2 = w2 / r2;
            double u = ellipsoid.a2 / r;
            double v = ellipsoid.a3 - ellipsoid.a4 / r;
            double c, s, ss;
            if (c2 > 0.3)
            {
                s = (zp / r) * (1.0 + c2 * (ellipsoid.a1 + u + s2 * v) / r);
                aLat = Math.Asin(s);      
                ss = s * s;
                c = Math.Sqrt(1.0 - ss);
            }
            else
            {
                c = (w / r) * (1.0 - s2 * (ellipsoid.a5 - u - c2 * v) / r);
                aLat = Math.Acos(c);      
                ss = 1.0 - c * c;
                s = Math.Sqrt(ss);
            }
            double g = 1.0 - ellipsoid.e2 * ss;
            double rg = ellipsoid.a / Math.Sqrt(g);
            double rf = ellipsoid.a6 * rg;
            u = w - rg * c;
            v = zp - rf * s;
            double f = c * u + s * v;
            double m = c * v - s * u;
            double p = m / (rf / g + f);
            aLat = aLat + p;      
            aH = f + m * p / 2.0;     
            if (z < 0.0)
            {
                aLat *= -1.0;     //Lat
            }
            aLat *= RadiansToDeg;
            aLon *= RadiansToDeg;
        }
        public static void EPSG4327ToECEF(Ellipsoid ellipsoid, double lat, double lon, double height, out double x, out double y, out double z)
        {
            double N = ellipsoid.a / Math.Sqrt(1.0 - ellipsoid.e2 * Math.Sin(lat * DegToRadians) * Math.Sin(lat * DegToRadians));
            x = (N + height) * Math.Cos(lat * DegToRadians) * Math.Cos(lon * DegToRadians);    //ECEF x
            y = (N + height) * Math.Cos(lat * DegToRadians) * Math.Sin(lon * DegToRadians);    //ECEF y
            z = (N * (1.0 - ellipsoid.e2) + height) * Math.Sin(lat * DegToRadians);          //ECEF z
        }
        public static void ECEFToEPSG4979(TopocentricFrame topoFrame, double x, double y, double z, out double xENU, out double yENU, out double zENU)
        {
            x = x -topoFrame.ecefTangentPoint.x;
            y = y -topoFrame.ecefTangentPoint.y;
            z = z -topoFrame.ecefTangentPoint.z;

            xENU = x * (-topoFrame.sin_lon) + y * ( topoFrame.cos_lon) + z * ( 0.0 ) ;
            yENU = x * (-topoFrame.cos_sin) + y * (-topoFrame.sin_sin) + z * ( topoFrame.cos_lat);
            zENU = x * ( topoFrame.cos_cos) + y * ( topoFrame.sin_cos) + z * ( topoFrame.sin_lat) ; // translate to the topocentric origin)
        }
        public static void EPSG4979ToECEF(TopocentricFrame topoFrame, double xENU, double yENU, double zENU, out double x, out double y, out double z)
        {
            x = xENU * (-topoFrame.sin_lon) + yENU * (-topoFrame.cos_sin) + zENU * ( topoFrame.cos_cos);
            y = xENU * ( topoFrame.cos_lon) + yENU * (-topoFrame.sin_sin) + zENU * ( topoFrame.sin_cos);
            z = xENU * ( 0.0 )              + yENU * ( topoFrame.cos_lat) + zENU * ( topoFrame.sin_lat);

            x = x + topoFrame.ecefTangentPoint.x;
            y = y + topoFrame.ecefTangentPoint.y;
            z = z + topoFrame.ecefTangentPoint.z;
        }
        // -sin L             cos L            0
        // -cos L * sin P     -sin L * sin P    sin L * cos P
        // 0                  cos P            sin P




        public TopocentricFrame(Ellipsoid ellipsoid, EPSG4327 tangentPoint)
        {
            if (ellipsoid == null)
            {
                ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
            }
            this.tangentPoint = tangentPoint;   
            double x, y, z;
            EPSG4327ToECEF(ellipsoid, tangentPoint.lat, tangentPoint.lon, tangentPoint.h, out x, out y, out z);
            WGS84ECEF ecefTangentPoint = new WGS84ECEF(x, y, z);
            this.ecefTangentPoint = ecefTangentPoint;   
            double wsq = x * x + y * y;
            double aLat = tangentPoint.lat;
            double aLon = tangentPoint.lon;
            double aH = tangentPoint.h;
            this.radius = Math.Sqrt(wsq + z * z);
            double w = Math.Sqrt(wsq);
            this.cos_lat = Math.Cos(aLat * DegToRadians);
            this.sin_lat = Math.Sqrt(1.0 - this.cos_lat * this.cos_lat);
            double xcos_lon = x / w;
            double xsin_lon = y / w;
            this.cos_lon = Math.Cos(tangentPoint.lon * DegToRadians);
            this.sin_lon = Math.Sin(tangentPoint.lon * DegToRadians);
            this.sin_sin = this.sin_lon * this.sin_lat;
            this.sin_cos = this.sin_lon * this.cos_lat;
            this.cos_sin = this.cos_lon * this.sin_lat;
            this.cos_cos = this.cos_lon * this.cos_lat;
        }

        /// <summary>
        /// Create and initialize a TopocentricFrame with tangent point in ECEF coordinates
        /// </summary>
        /// <param name="ellipsoid"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public TopocentricFrame(Ellipsoid ellipsoid, double x, double y, double z)
        {
            if (ellipsoid == null)
            {
                ellipsoid = new Ellipsoid(6378137.0, 1.0 / 298.257223563);
            }
            double wsq = x * x + y * y;
            double aLat, aLon, aH;
            this.radius = Math.Sqrt(wsq + z * z);
            ECEFToEPSG4327(ellipsoid, x, y, z, out aLat, out aLon, out aH);
            EPSG4327 tangentPoint = new EPSG4327(aLat, aLon, aH);   
            this.tangentPoint = tangentPoint;   
            double w = Math.Sqrt(wsq);
            this.cos_lat = Math.Cos(aLat * DegToRadians);
            this.sin_lat = Math.Sqrt(1.0 - this.cos_lat * this.cos_lat);
            this.cos_lon = x / w;
            this.sin_lon = y / w;
            this.sin_sin = this.sin_lat * this.sin_lon;
            this.sin_cos = this.sin_lat * this.cos_lon;
            this.cos_sin = this.cos_lat * this.sin_lon;
            this.cos_cos = this.cos_lat * this.cos_lon;
        }
    }
    public class Ellipsoid
    {
        const double DegToRadians = Math.PI / 180.0;
        const double RadiansToDeg = 180.0 / Math.PI;

        //private static double a = 6378137.0;              //WGS-84 semi-major axis
        //private static double e2 = 6.6943799901377997e-3;  //WGS-84 first eccentricity squared 0.00669437999014132950.0066943799901413295
        //private static double a1 = 4.2697672707157535e+4;  //a1 = a*e2
        //private static double a2 = 1.8230912546075455e+9;  //a2 = a1*a1
        //private static double a3 = 1.4291722289812413e+2;  //a3 = a1*e2/2
        //private static double a4 = 4.5577281365188637e+9;  //a4 = 2.5*a2
        //private static double a5 = 4.2840589930055659e+4;  //a5 = a1+a3
        //private static double a6 = 9.9330562000986220e-1;  //a6 = 1-e2


        public double a { get; set; } //WGS-84 semi-major axis
        public double a_sq { get; set; }
        public double f { get; set; }
        public double b { get; set; }
        public double b_sq { get; set; }
        public double a_sq_over_b { get; set; }
        public double e { get; set; }
        public double e2 { get; set; }
        public double a1 { get; set; }
        public double a2 { get; set; }
        public double a3 { get; set; }
        public double a4 { get; set; }
        public double a5 { get; set; }
        public double a6 { get; set; }

        public double c1 { get; set; }
        /*
        * 
        *  double ellipse_a = a;
           double ellipse_a_sq = ellipse_a*ellipse_a;
           double ellipse_f = f;
           double ellipse_b = ellipse_a * (1.0 - ellipse_f);
           double ellipse_b_sq = ellipse_b + ellipse_b;
           double ellipse_a_sq_over_b = ellipse_a_sq / ellipse_b;
           double ellipse_c1 = (1.0 - ellipse_f) * (1.0 - ellipse_f);
        * 
        * 
        */
        /// <summary>
        /// Private empty constructor to force initialization.
        /// </summary>
        private Ellipsoid()
        {

        }
        private void FinishInitialization()
        {
            this.a_sq = this.a * this.a;
            this.b_sq = this.b * this.b;
            this.a_sq_over_b = this.a_sq / this.b;
            this.e = Math.Sqrt(1.0 - (this.b * this.b) / (this.a * this.a));// this.f * (2.0 + this.f); // (1 - b ^ 2 / a ^ 2) ^ (1 / 2)
            this.e2 = e * e;
            this.a1 = this.a * this.e2;
            this.a2 = this.a1 * this.a1;
            this.a3 = this.a1 * this.e2 / 2.0;
            this.a4 = 2.5 * this.a2;
            this.a5 = this.a1 + this.a3;
            this.a6 = 1.0 - this.e2;
         }
        public Ellipsoid(double a, double f)
        {
            this.a = a; // 6378137;
            this.f = f;
            this.b = this.a - this.a * this.f;
            FinishInitialization();
        }
        public Ellipsoid(double a, double borf, bool isFlattening)
        {
            this.a = a; // 6378137;
            if (isFlattening)
            {
                this.f = f;
                this.b = this.a - this.a * this.f;
            }
            else
            {
                this.b = b; // 6356752.3142
                this.f = (this.a - this.b) / this.a;
            }
            FinishInitialization(); 
        }
    }


    public class GSRTransforms
    {
        private const double RadiansPerDegree = Math.PI / 180.0;
        private const double DegreesPerRadian = 180.0 / Math.PI;
        private double Radians(double degrees)
        {
            return (degrees * RadiansPerDegree);
        }
        private double Degrees(double radians)
        {
            return (radians * DegreesPerRadian);
        }
        /// <summary>
        /// Convert a 3D WGS84 geographic point coordinates to EPSG 4979 - LTP-ENU
        /// </summary>
        /// <param name="tangentPoint"></param>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static EPSG4979? EPSG4327ToEPSG4979(EPSG4327 tangentPoint, EPSG4327 inValue)
        {
            const double a = 6378137;
            const double b = 6356752.3142;
            const double f = (a - b) / a;
            const double e_sq = f * (2.0 - f);
            double lamb = inValue.lat * RadiansPerDegree;
            double phi =  inValue.lon * RadiansPerDegree;
            double s = Math.Sin(lamb);
            double N = a / Math.Sqrt(1 - e_sq * s * s);

            double sin_lambda = Math.Sin(lamb);
            double cos_lambda = Math.Cos(lamb);
            double sin_phi = Math.Sin(phi);
            double cos_phi = Math.Cos(phi);

            double x = (inValue.h + N) * cos_lambda * cos_phi;
            double y = (inValue.h + N) * cos_lambda * sin_phi;
            double z = (inValue.h + (1 - e_sq) * N) * sin_lambda;
            EPSG4979 result = new EPSG4979();   
            result.east = x;
            result.north = y;
            result.up = z;
            return result;
        }

        public void InitializeEllipsoid(double a, double f)
        {
            double ellipse_a = a;
            double ellipse_a_sq = ellipse_a*ellipse_a;
            double ellipse_f = f;
            double ellipse_b = ellipse_a * (1.0 - ellipse_f);
            double ellipse_b_sq = ellipse_b + ellipse_b;
            double ellipse_a_sq_over_b = ellipse_a_sq / ellipse_b;
            double ellipse_c1 = (1.0 - ellipse_f) * (1.0 - ellipse_f);
        }
        /// <summary>
        /// Convert EPSG 4979 LTP-ENU point coordinates to 3D WGS 84
        /// </summary>
        /// <param name="tangentPoint"></param>
        /// <param name="inValue"></param>
        /// <returns></returns>
        public static EPSG4327? EPSG4979ToEPSG4327(EPSG4327 tangentPoint, EPSG4979 inValue)
        {
            return null;
        }
    }
    public class GSRMesh
    {

    }
    public class Relief
    {
        GSRMesh surface = new GSRMesh();
    }
    public class Water
    {

    }
    public class Transportation
    {

    }
    public class Furniture
    {

    }
    public class Vegetation
    {

    }
    public class Building
    {

    }
    public class Mobile
    {

    }
    public class GSR
    {
        // support LTP-ENU only
        public EPSG4327? tangentPoint { get; set; } = null;
        public string fileName { get; set; } = String.Empty;
        public string gsrName { get; set; } = String.Empty;
        public Polygon? boundary { get; set; } = null;

        public GSR()
        {
            gsrName = String.Empty;
            tangentPoint = null;
            fileName = String.Empty;
            boundary = null;
        }
        public GSR(string aGsrName, string aFileName, EPSG4327 aTangentPoint, Polygon aBoundary)
        {
            gsrName = aGsrName;
            tangentPoint = aTangentPoint;
            fileName = aFileName;
            boundary = aBoundary;
        }
    }
}