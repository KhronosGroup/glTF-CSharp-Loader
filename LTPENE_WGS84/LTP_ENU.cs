using System;
using System.Diagnostics;

namespace LTP_ENU
{


    // Some helpers for converting GPS readings from the WGS84 geodetic system to a local North-East-Up cartesian axis.

    // The implementation here is according to the paper:
    // "Conversion of Geodetic coordinates to the Local Tangent Plane" Version 2.01.
    // "The basic reference for this paper is J.Farrell & M.Barth 'The Global Positioning System & Inertial Navigation'"
    // Also helpful is Wikipedia: http://en.wikipedia.org/wiki/Geodetic_datum
    // Also helpful are the guidance notes here: http://www.epsg.org/Guidancenotes.aspx
    public class LTP_ENU
    {
        // WGS-84 geodetic constants
        const double a = 6378137.0;         // WGS-84 Earth semimajor axis (m)

        const double b = 6356752.314245;     // Derived Earth semiminor axis (m)
        const double f = (a - b) / a;           // Ellipsoid Flatness
        const double f_inv = 1.0 / f;       // Inverse flattening

        //const double f_inv = 298.257223563; // WGS-84 Flattening Factor of the Earth 
        //const double b = a - a / f_inv;
        //const double f = 1.0 / f_inv;

        const double a_sq = a * a;
        const double b_sq = b * b;
        const double e_sq = f * (2 - f);    // Square of Eccentricity

        // Converts WGS-84 Geodetic point (lat, lon, h) to the 
        // Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z).
        public static void GeodeticToEcef(double lat, double lon, double h,
                                            out double x, out double y, out double z)
        {
            // Convert to radians in notation consistent with the paper:
            var lambda = DegreesToRadians(lat);
            var phi = DegreesToRadians(lon);
            var s = Math.Sin(lambda);
            var N = a / Math.Sqrt(1 - e_sq * s * s);

            var sin_lambda = Math.Sin(lambda);
            var cos_lambda = Math.Cos(lambda);
            var cos_phi = Math.Cos(phi);
            var sin_phi = Math.Sin(phi);

            x = (h + N) * cos_lambda * cos_phi;
            y = (h + N) * cos_lambda * sin_phi;
            z = (h + (1 - e_sq) * N) * sin_lambda;
        }

        // Converts the Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z) to 
        // (WGS-84) Geodetic point (lat, lon, h).
        public static void EcefToGeodetic(double x, double y, double z,
                                            out double lat, out double lon, out double h)
        {
            var eps = e_sq / (1.0 - e_sq);
            var p = Math.Sqrt(x * x + y * y);
            var q = Math.Atan2((z * a), (p * b));
            var sin_q = Math.Sin(q);
            var cos_q = Math.Cos(q);
            var sin_q_3 = sin_q * sin_q * sin_q;
            var cos_q_3 = cos_q * cos_q * cos_q;
            var phi = Math.Atan2((z + eps * b * sin_q_3), (p - e_sq * a * cos_q_3));
            var lambda = Math.Atan2(y, x);
            var v = a / Math.Sqrt(1.0 - e_sq * Math.Sin(phi) * Math.Sin(phi));
            h = (p / Math.Cos(phi)) - v;

            lat = RadiansToDegrees(phi);
            lon = RadiansToDegrees(lambda);
        }

        // Converts the Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z) to 
        // East-North-Up coordinates in a Local Tangent Plane that is centered at the 
        // (WGS-84) Geodetic point (lat0, lon0, h0).
        public static void EcefToEnu(double x, double y, double z,
                                        double lat0, double lon0, double h0,
                                        out double xEast, out double yNorth, out double zUp)
        {
            // Convert to radians in notation consistent with the paper:
            var lambda = DegreesToRadians(lat0);
            var phi = DegreesToRadians(lon0);
            var s = Math.Sin(lambda);
            var N = a / Math.Sqrt(1 - e_sq * s * s);

            var sin_lambda = Math.Sin(lambda);
            var cos_lambda = Math.Cos(lambda);
            var cos_phi = Math.Cos(phi);
            var sin_phi = Math.Sin(phi);

            double x0 = (h0 + N) * cos_lambda * cos_phi;
            double y0 = (h0 + N) * cos_lambda * sin_phi;
            double z0 = (h0 + (1 - e_sq) * N) * sin_lambda;

            double xd, yd, zd;
            xd = x - x0;
            yd = y - y0;
            zd = z - z0;

            // This is the matrix multiplication
            xEast = -sin_phi * xd + cos_phi * yd;
            yNorth = -cos_phi * sin_lambda * xd - sin_lambda * sin_phi * yd + cos_lambda * zd;
            zUp = cos_lambda * cos_phi * xd + cos_lambda * sin_phi * yd + sin_lambda * zd;
        }

        // Inverse of EcefToEnu. Converts East-North-Up coordinates (xEast, yNorth, zUp) in a
        // Local Tangent Plane that is centered at the (WGS-84) Geodetic point (lat0, lon0, h0)
        // to the Earth-Centered Earth-Fixed (ECEF) coordinates (x, y, z).
        public static void EnuToEcef(double xEast, double yNorth, double zUp,
                                        double lat0, double lon0, double h0,
                                        out double x, out double y, out double z)
        {
            // Convert to radians in notation consistent with the paper:
            var lambda = DegreesToRadians(lat0);
            var phi = DegreesToRadians(lon0);
            var s = Math.Sin(lambda);
            var N = a / Math.Sqrt(1 - e_sq * s * s);

            var sin_lambda = Math.Sin(lambda);
            var cos_lambda = Math.Cos(lambda);
            var cos_phi = Math.Cos(phi);
            var sin_phi = Math.Sin(phi);

            double x0 = (h0 + N) * cos_lambda * cos_phi;
            double y0 = (h0 + N) * cos_lambda * sin_phi;
            double z0 = (h0 + (1 - e_sq) * N) * sin_lambda;

            double xd = -sin_phi * xEast - cos_phi * sin_lambda * yNorth + cos_lambda * cos_phi * zUp;
            double yd = cos_phi * xEast - sin_lambda * sin_phi * yNorth + cos_lambda * sin_phi * zUp;
            double zd = cos_lambda * yNorth + sin_lambda * zUp;

            x = xd + x0;
            y = yd + y0;
            z = zd + z0;
        }

        // Converts the geodetic WGS-84 coordinated (lat, lon, h) to 
        // East-North-Up coordinates in a Local Tangent Plane that is centered at the 
        // (WGS-84) Geodetic point (lat0, lon0, h0).
        public static void GeodeticToEnu(double lat, double lon, double h,
                                            double lat0, double lon0, double h0,
                                            out double xEast, out double yNorth, out double zUp)
        {
            double x, y, z;
            GeodeticToEcef(lat, lon, h, out x, out y, out z);
            EcefToEnu(x, y, z, lat0, lon0, h0, out xEast, out yNorth, out zUp);
        }
        public static void EnuToGeodetic(double xEast, double yNorth, double zUp,
                                             double lat0, double lon0, double h0,
                                            out double lat, out double lon, out double h
                                            )
        {
            double x, y, z;
            EnuToEcef(xEast, yNorth, zUp, lat0, lon0, h0, out x, out y, out z);
            EcefToGeodetic(x, y, z, out lat, out lon, out h);
        }

#if DEBUGx
        // South African Coordinate Reference System (Hartebeesthoek94) to Geodetic
        // From "CDNGI Coordinate Conversion Utility v1 Sep 2009.xls"
        public static void SacrsToGeodetic(int loMeridian, double yWesting, double xSouthing,
            out double lat, out double lon)
        {
            var loMeridianRadians = DegreesToRadians(loMeridian);
            const double ec_sq = (a_sq - b_sq) / a_sq;   // e^2   in "CDNGI Coordinate Conversion Utility v1 Sep 2009.xls"
            const double ep_sq = (a_sq - b_sq) / b_sq;   // e'^2
            const double n = (a - b) / (a + b);
            const double n_2 = n * n;
            const double n_3 = n_2 * n;
            const double n_4 = n_2 * n_2;
            const double n_5 = n_2 * n_3;
            const double p2 = 3.0 / 2.0 * n - 27.0 / 32.0 * n_3 + 269.0 / 512.0 * n_5;
            const double p4 = 21.0 / 16.0 * n_2 - 55.0 / 32.0 * n_4;
            const double p6 = 151.0 / 96.0 * n_3 - 417.0 / 128.0 * n_5;
            const double p8 = 1097.0 / 512.0 * n_4;
            const double p10 = 8011.0 / 2560.0 * n_5;
            const double a0 = 1.0 / (n + 1.0) * (1.0 + 1.0 / 4.0 * n_2 + 1.0 / 64.0 * n_4);
            var footBar = xSouthing / (a * a0);
            var foot = footBar + p2 * Sin(2.0 * footBar) + p4 * Sin(4.0 * footBar) + p6 * Sin(6.0 * footBar) + p8 * Sin(8.0 * footBar) + p10 * Sin(10.0 * footBar);
            var Nf = a / Sqrt(1.0 - ec_sq * Pow(Sin(foot), 2));

            var b1 = 1.0 / (Nf * Cos(foot));
            var b2 = Tan(foot) / (2.0 * Nf * Nf * Cos(foot));
            var b3 = (1.0 + 2.0 * Pow(Tan(foot), 2) + ep_sq * Pow(Cos(foot), 2)) / (6.0 * Pow(Nf, 3) * Cos(foot));
            var b4 = (Tan(foot) * (5.0 + 6.0 * Pow(Tan(foot), 2) + ep_sq * Pow(Cos(foot), 2))) / (24.0 * Pow(Nf, 4) * Cos(foot));
            var b5 = (5.0 + 28.0 * Pow(Tan(foot), 2) + 24.0 * Pow(Tan(foot), 4)) / (120.0 * Pow(Nf, 5) * Cos(foot));
            var d1 = Cos(foot) * (1.0 + ep_sq * Pow(Cos(foot), 2));
            var d2 = -1.0 / 2.0 * Pow(Cos(foot), 2) * Tan(foot) * (1.0 + 4.0 * ep_sq * Pow(Cos(foot), 2));

            var latRadians = -(foot - b2 * d1 * Pow(yWesting, 2) + (b4 * d1 + b2 * b2 * d2) * Pow(yWesting, 4));
            var lonRadians = loMeridianRadians - (b1 * yWesting - b3 * Pow(yWesting, 3) + b5 * Pow(yWesting, 5));
            lat = RadiansToDegrees(latRadians);
            lon = RadiansToDegrees(lonRadians);


        GeodeticToSacrs(lat, lon, out int loMeridianCheck, out var yWestingCheck, out var xSouthingCheck);
        Debug.Assert(Abs(yWesting - yWestingCheck) < 1e-6);
        Debug.Assert(Abs(xSouthing - xSouthingCheck) < 1e-6);
        Debug.Assert(loMeridian == loMeridianCheck);

        }

        // South African Coordinate Reference System (Hartebeesthoek94) to Geodetic
        // From "CDNGI Coordinate Conversion Utility v1 Sep 2009.xls"
        public static void GeodeticToSacrs(double lat, double lon,
            out int loMeridian, out double yWesting, out double xSouthing)
        {
            // longitude of origin
            loMeridian = lon == 0 ? 1 : (int)Truncate(lon / 2.0) * 2 + Sign(lon);    // Take integer part of lon, rounded up to nearest odd number (or down if negative), so =ODD(TRUNC(lon))
            double loMeridianRadians = DegreesToRadians(loMeridian);
            const double ec_sq = (a_sq - b_sq) / a_sq;   // e^2   in "CDNGI Coordinate Conversion Utility v1 Sep 2009.xls"
            const double ep_sq = (a_sq - b_sq) / b_sq;   // e'^2
            const double n = (a - b) / (a + b);
            const double n_2 = n * n;
            const double n_3 = n_2 * n;
            const double n_4 = n_2 * n_2;
            const double n_5 = n_2 * n_3;
            const double A0 = 1.0 / (n + 1.0) * (1.0 + 1.0 / 4.0 * n_2 + 1.0 / 64.0 * n_4);
            const double A2 = -1.0 / (n + 1.0) * (3.0 / 2.0 * n - 3.0 / 16.0 * n_3 - 3.0 / 128.0 * n_5);
            const double A4 = 1.0 / (n + 1.0) * (15.0 / 16.0 * n_2 - 15.0 / 64.0 * n_4);
            const double A6 = -1.0 / (n + 1.0) * (35.0 / 48.0 * n_3 - 175.0 / 768.0 * n_5);
            const double A8 = 1.0 / (n + 1.0) * (315.0 / 512.0 * n_4);
            const double A10 = 1.0 / (n + 1.0) * (693.0 / 1280.0 * n_5);

            double latRadians = DegreesToRadians(-lat);
            double lonRadians = DegreesToRadians(lon);
            double G = a * (A0 * latRadians + A2 * Sin(2 * latRadians) + A4 * Sin(4 * latRadians) + A6 * Sin(6 * latRadians) + A8 * Sin(8 * latRadians) + A10 * Sin(10 * latRadians));
            double N = a / Sqrt(1.0 - ec_sq * Sin(latRadians) * Sin(latRadians));

            double latCos = Cos(latRadians);
            double latCos_2 = latCos * latCos;
            double latCos_3 = latCos_2 * latCos;
            double latCos_4 = latCos_2 * latCos_2;
            double latCos_5 = latCos_4 * latCos;
            double latTan = Tan(latRadians);
            double latTan_2 = latTan * latTan;
            double latTan_4 = latTan_2 * latTan_2;
            double a1 = N * latCos;
            double a2 = -1.0 / 2.0 * N * latCos_2 * latTan;
            double a3 = -1.0 / 6.0 * N * latCos_3 * (1.0 - latTan_2 + ep_sq * latCos_2);
            double a4 = 1.0 / 24.0 * N * latCos_4 * latTan * (5 - latTan_2 + 9.0 * ep_sq * latCos_2);
            double a5 = 1.0 / 120.0 * N * latCos_5 * (5.0 - 18.0 * latTan_2 + latTan_4);
            double l = lonRadians - loMeridianRadians;
            double l_2 = l * l;
            double l_3 = l * l_2;
            double l_4 = l_2 * l_2;
            double l_5 = l_2 * l_3;
            xSouthing = G - a2 * l_2 + a4 * l_4;
            yWesting = -(a1 * l - a3 * l_3 + a5 * l_5);
        }
#endif
        // Just check that we get the same values as the paper for the main calculations.
        public static void Test()
        {
            var latLA = 34.00000048;
            var lonLA = -117.3335693;
            var hLA = 251.702;

            double x0, y0, z0;
            GeodeticToEcef(latLA, lonLA, hLA, out x0, out y0, out z0);

            System.Diagnostics.Debug.Assert(AreClose(-2430601.8, x0));
            Debug.Assert(AreClose(-4702442.7, y0));
            Debug.Assert(AreClose(3546587.4, z0));

            // Checks to read out the matrix entries, to compare to the paper
            double x, y, z;
            double xEast, yNorth, zUp;

            // First column
            x = x0 + 1;
            y = y0;
            z = z0;
            EcefToEnu(x, y, z, latLA, lonLA, hLA, out xEast, out yNorth, out zUp);
            Debug.Assert(AreClose(0.88834836, xEast));
            Debug.Assert(AreClose(0.25676467, yNorth));
            Debug.Assert(AreClose(-0.38066927, zUp));

            x = x0;
            y = y0 + 1;
            z = z0;
            EcefToEnu(x, y, z, latLA, lonLA, hLA, out xEast, out yNorth, out zUp);
            Debug.Assert(AreClose(-0.45917011, xEast));
            Debug.Assert(AreClose(0.49675810, yNorth));
            Debug.Assert(AreClose(-0.73647416, zUp));

            x = x0;
            y = y0;
            z = z0 + 1;
            EcefToEnu(x, y, z, latLA, lonLA, hLA, out xEast, out yNorth, out zUp);
            Debug.Assert(AreClose(0.00000000, xEast));
            Debug.Assert(AreClose(0.82903757, yNorth));
            Debug.Assert(AreClose(0.55919291, zUp));

        }

        public static void Test2()
        {
            var latLA = 34.00000048;
            var lonLA = -117.3335693;
            var hLA = 251.702;

            double x0, y0, z0;
            GeodeticToEcef(latLA, lonLA, hLA, out x0, out y0, out z0);

            Debug.Assert(AreClose(-2430601.8, x0));
            Debug.Assert(AreClose(-4702442.7, y0));
            Debug.Assert(AreClose(3546587.4, z0));

            EcefToEnu(x0, y0, z0, latLA, lonLA, hLA, out double xEast, out double yNorth, out double zUp);

            Debug.Assert(AreClose(0, xEast));
            Debug.Assert(AreClose(0, yNorth));
            Debug.Assert(AreClose(0, zUp));

            EnuToEcef(xEast, yNorth, zUp, latLA, lonLA, hLA, out double xTest, out double yTest, out double zTest);
            EcefToGeodetic(xTest, yTest, zTest, out double latTest, out double lonTest, out double hTest);

            Debug.Assert(AreClose(latLA, latTest));
            Debug.Assert(AreClose(lonLA, lonTest));
            Debug.Assert(AreClose(hTest, hLA));

        }

        static bool AreClose(double x0, double x1)
        {
            var d = x1 - x0;
            return (d * d) < 0.1;
        }


        static double DegreesToRadians(double degrees)
        {
            return Math.PI / 180.0 * degrees;
        }

        static double RadiansToDegrees(double radians)
        {
            return 180.0 / Math.PI * radians;
        }
    }
}
