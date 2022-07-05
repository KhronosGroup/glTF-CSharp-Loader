
using NetTopologySuite;
using NetTopologySuite.Geometries;

namespace GSR00
{
    /// <summary>
    /// A geodetic position in the WGS84 reference frame
    /// </summary>
    public class EPSG4327
    {
        public EPSG4327(double aLat, double aLon, double aH)
        {
            this.lat = aLat;
            this.lon = aLon;
            this.h = aH;
        }
        public double lat { get; set; }
        public double lon { get; set; }
        public double h { get; set; }
        public EPSG4327(EPSG4327 original)
        {
            this.lat = original.lat;
            this.lon = original.lon;
            this.h = original.h;
        }
    }
    /// <summary>
    /// An ENU position based on a WGS84 tangent point
    /// </summary>
    public struct EPSG4979
    {
        public TopocentricFrame topocentricFrame { get; set; }
        public double east { get; set; }
        public double north { get; set; }
        public double up { get; set; }
        public EPSG4979(TopocentricFrame topocentricFrame, double anEast, double aNorth, double anUp)
        {
            this.east = anEast;
            this.north = aNorth;
            this.up = anUp;
            this.topocentricFrame = topocentricFrame;
        }
        public EPSG4979(EPSG4979 original)
        {
            this.east  = original.east;
            this.north = original.north;
            this.up    = original.up;
            this.topocentricFrame = original.topocentricFrame;
        }
    }
    /// <summary>
    /// An ECEF position in the WGS84 reference frame
    /// </summary>
    public class EPSG4978
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public EPSG4978(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        /// <summary>
        /// An ECEF position in the WGS84 reference frame
        /// </summary>
        /// <param name="original"></param>
        public EPSG4978(EPSG4978 original)
        {
            this.x = original.x;
            this.y = original.y;
            this.z = original.z;    
        }
    } 
    public class TopocentricFrame
    {
        static public readonly double DegToRadians = Math.PI / 180.0;
        static public readonly double RadiansToDeg = 180.0 / Math.PI;
        public EPSG4327 tangentPoint { get; set; }
        public EPSG4978? ecefTangentPoint { get; set; }

        public double radius { get; set; } // z value at origin of topocentric system
        public double sin_lat { get; set; } // sine of angle between normal and equatorial plane
        public double cos_lat { get; set; } // cosine of angle between normal and equatorial plane
        public double sin_lon { get; set; } // sine of angle between normal and plane of prime meridian
        public double cos_lon { get; set; } // cosine of angle between normal and plane of prime meridian
        public double sin_cos { get; set; } // sin(lat) * cos(lon)
        public double sin_sin { get; set; } // sin(lat)°sin(on))
        public double cos_cos { get; set; } // cos(lat)'cos(lon) 
        public double cos_sin { get; set; } // cos(lat)'sin(lon) 
        public static void EPSG4978ToEPSG4327(Ellipsoid ellipsoid, EPSG4978 inPosition, out EPSG4327 outPosition)
        {
            double aLat, aLon, aH;
            double x = inPosition.x;
            double y = inPosition.y;
            double z = inPosition.z;
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
            outPosition = new EPSG4327(aLat, aLon, aH); 
        }
        public static void EPSG4327ToEPSG4978(Ellipsoid ellipsoid, EPSG4327 inPosition, out EPSG4978 outPosition)
        {
            double N = ellipsoid.a / Math.Sqrt(1.0 - ellipsoid.e2 * Math.Sin(inPosition.lat * DegToRadians) * Math.Sin(inPosition.lat * DegToRadians));
            double x = (N + inPosition.h) * Math.Cos(inPosition.lat * DegToRadians) * Math.Cos(inPosition.lon * DegToRadians);    //ECEF x
            double y = (N + inPosition.h) * Math.Cos(inPosition.lat * DegToRadians) * Math.Sin(inPosition.lon * DegToRadians);    //ECEF y
            double z = (N * (1.0 - ellipsoid.e2) + inPosition.h) * Math.Sin(inPosition.lat * DegToRadians);          //ECEF z
            outPosition = new EPSG4978(x, y, z);
        }
        public static void EPSG4978ToEPSG4979(TopocentricFrame topoFrame, EPSG4978 inPosition, out EPSG4979 outPosition)
        {
            double x = inPosition.x - topoFrame.ecefTangentPoint.x; 
            double y = inPosition.y - topoFrame.ecefTangentPoint.y;
            double z = inPosition.z - topoFrame.ecefTangentPoint.z;

           // x = x - topoFrame.ecefTangentPoint.x;
           // y = y - topoFrame.ecefTangentPoint.y;
           // z = z - topoFrame.ecefTangentPoint.z;

            double xENU = x * (-topoFrame.sin_lon) + y * ( topoFrame.cos_lon) + z * ( 0.0 ) ;
            double yENU = x * (-topoFrame.cos_sin) + y * (-topoFrame.sin_sin) + z * ( topoFrame.cos_lat);
            double zENU = x * ( topoFrame.cos_cos) + y * ( topoFrame.sin_cos) + z * ( topoFrame.sin_lat) ; // translate to the topocentric origin)

            outPosition = new EPSG4979(topoFrame, xENU, yENU, zENU);
        }
        public static void EPSG4979ToEPSG4978(TopocentricFrame topoFrame, EPSG4979 enuPosition, out EPSG4978 ecefPosition)
        {
            double xENU = enuPosition.east;
            double yENU = enuPosition.north;
            double zENU = enuPosition.up;
            double x = xENU * (-topoFrame.sin_lon) + yENU * (-topoFrame.cos_sin) + zENU * ( topoFrame.cos_cos);
            double y = xENU * ( topoFrame.cos_lon) + yENU * (-topoFrame.sin_sin) + zENU * ( topoFrame.sin_cos);
            double z = xENU * ( 0.0 )              + yENU * ( topoFrame.cos_lat) + zENU * ( topoFrame.sin_lat);
            x = x + topoFrame.ecefTangentPoint.x;
            y = y + topoFrame.ecefTangentPoint.y;
            z = z + topoFrame.ecefTangentPoint.z;
            ecefPosition = new EPSG4978(x, y, z);
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
            if(tangentPoint == null)
            {
                throw new ArgumentNullException("tangentPoint argument cannot be null in TopocentricFrame constructor.");
            }
            this.tangentPoint = tangentPoint;   
            EPSG4978 outPosition;
            EPSG4327ToEPSG4978(ellipsoid, tangentPoint, out outPosition);
            EPSG4978 ecefTangentPoint = new EPSG4978(outPosition.x, outPosition.y, outPosition.z);
            this.ecefTangentPoint = ecefTangentPoint;
            double wsq = outPosition.x * outPosition.x + outPosition.y * outPosition.y;
            double aLat = tangentPoint.lat;
            double aLon = tangentPoint.lon;
            double aH = tangentPoint.h;
            this.radius = Math.Sqrt(wsq + outPosition.z * outPosition.z);
            double w = Math.Sqrt(wsq);
            this.cos_lat = Math.Cos(aLat * DegToRadians);
            this.sin_lat = Math.Sqrt(1.0 - this.cos_lat * this.cos_lat);
            double xcos_lon = outPosition.x / w;
            double xsin_lon = outPosition.y / w;
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
            //double aLat, aLon, aH;
            this.radius = Math.Sqrt(wsq + z * z);
            EPSG4327 tangentPoint;
            EPSG4978ToEPSG4327(ellipsoid, new EPSG4978(x, y, z), out tangentPoint);
            this.tangentPoint = tangentPoint;   
            double w = Math.Sqrt(wsq);
            this.cos_lat = Math.Cos(tangentPoint.lat * DegToRadians); // lat was aLat
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