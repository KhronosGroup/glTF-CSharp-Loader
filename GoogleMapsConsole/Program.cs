using System;
using System.IO;
using System.Net.Http;
using System.Drawing;

namespace GoogleMapsConsole
{
    /*
     * 
        public static void GeodeticToEnu(double lat, double lon, double h,
                                            double lat0, double lon0, double h0,
                                            out double xEast, out double yNorth, out double zUp)
        
        public static void EnuToGeodetic(double xEast, double yNorth, double zUp,
                                             double lat0, double lon0, double h0,
                                            out double lat, out double lon, out double h
     * 
     */
    internal class Program
    {
        // check/create working directories
        // define a center in wgs 84
        // define a radius in m
        // get conters of bb based on center and radius
        // make a point set with center and circular boundary with 1 + 256 points
        // get feature info from OSM
        // get texture from Google
        // build roads, paths, water, and buildings/footprints from feature info
        // triangulate points of the area features plus 
        // add 
        static async Task Main(string[] args)
        {
            // https://overpass-api.de/api/interpreter?data=[out:json];node(50.93545,-1.4727869,50.93946,-1.46737964);%20out%20body;
            // https://overpass-api.de/api/interpreter?data=[out:json];way(50.93545,-1.4727869,50.93946,-1.46737964);%20out%20body;
            // https://overpass-api.de/api/interpreter?data=[out:json];relation(50.93545,-1.4727869,50.93946,-1.46737964);%20out%20body;

            // top  left = 50.93946582314387, -1.4727869743311701
            // bot right = 50.93545318608545, -1.4673796409470672
            //
            // 50.938733425687315,%20-1.470220960392684
            // 50.93563783737563, -1.4691744971337046 is lower right
            // need 14 rows up and nine columns left at zoom 20

            string baseDir = "c:\\temp\\models\\world";
            // create directories if not already present
            if(!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
            string baseImagesDir = baseDir + "\\" + "TerrainImages";
            if (!Directory.Exists(baseDir))
            {
                Directory.CreateDirectory(baseDir);
            }
            double cLat = 50.9374713795844;
            double cLon = -1.4696387314938;
            double cH   = 19.08;
            double radius = 220.0;

            double north, east, up, latMin, lonMin, latMax, lonMax, aH;
            // get rectangular bounds corresponding to circle
            LTP_ENU.LTP_ENU.GeodeticToEnu(cLat, cLon, cH, cLat, cLon, cH, out east, out north, out up);
            LTP_ENU.LTP_ENU.EnuToGeodetic(east + radius, north + radius, up, cLat, cLon, cH, out latMax, out lonMax, out aH);
            LTP_ENU.LTP_ENU.EnuToGeodetic(east - radius, north - radius, up, cLat, cLon, cH, out latMin, out lonMin, out aH);
            // get centers of images to cover rectangle
            int imageSize = 512;
            uint zoom = 20;
            double overlap = 0.8;
            double shiftLines = (double)imageSize * overlap;
            //double lonShiftLines = 320.0;
            Console.Write("Rows per degree: " + RowsPerLatDegree(latMin, latMax, zoom).ToString("f8"));
            Console.WriteLine("  Cols per degree: " + ColsPerLonDegree(lonMin, lonMax, zoom).ToString("f8"));
            // what is the height in pseudo mercator northings? half of imageSize
            // what is the width in pseudo mercator eastings? half of imageSize
            // what is the height of an image in degrees? imageSize/RowsPerLatDegree(latMin, latMax, zoom)
            double imageSizeLatDegrees = (double)imageSize / RowsPerLatDegree(latMin, latMax, zoom);
            double imageSizeLonDegrees = (double)imageSize / ColsPerLonDegree(latMin, latMax, zoom);
            // what is the lat, lon of the lower left image?
            double latLL = latMin + imageSizeLatDegrees * overlap * 0.5;
            double lonLL = lonMin + imageSizeLonDegrees * overlap * 0.5;
            double latUR = latMax - imageSizeLatDegrees * overlap * 0.5;
            double lonUR = lonMax - imageSizeLonDegrees * overlap * 0.5;
            int nRows = (int)((latUR - latLL + imageSizeLatDegrees * 0.5) / (imageSizeLatDegrees * overlap));
            int nCols = (int)((lonUR - lonLL + imageSizeLonDegrees * 0.5) / (imageSizeLonDegrees * overlap));
            //double clatLR = 50.93563783737563;
            //double clonLR = -1.4691744971337046 + 0.001718 * 0.25; //-1.4691744971337046;
            //double clonLR = -1.4692580 + 0.001718 * 0.25; //-1.4691744971337046;
            //double latIncrement = 0.001082 * 0.25;
            //double lonIncrement = 0.001718 * 0.25;
            double latIncrement = shiftLines / RowsPerLatDegree(latMin, latMax, zoom);
            double lonIncrement = shiftLines / ColsPerLonDegree(lonMin, lonMax, zoom);
            //int nRows = 5;
            //int nCols = 5;
            HttpClient client = new HttpClient();
            for (int nCol = 0; nCol < nCols; nCol++)
            //for (int nCol = 0; nCol < 6; nCol++)
            {
                double colLon = lonLL + nCol * lonIncrement;
                for (int nRow = 0; nRow < nRows; nRow++)
                {
                    double rowLat = latLL + nRow * latIncrement;
                    string gmRC = latToY(rowLat, (uint)zoom).ToString() + "." + lonToX(colLon, (uint)zoom).ToString();
                    string latlon = rowLat.ToString("f7") + "," + colLon.ToString("f7");
                    string colrow = nCol.ToString("d2") + "." + nRow.ToString("d2");
                    string fileName = "c:\\temp\\models\\world\\satimages\\20." + gmRC + ".png";
                    if(File.Exists(fileName))
                    {
                        continue;
                    }
                    Uri uri = new Uri("https://maps.googleapis.com/maps/api/staticmap?center=<<latlon>>&format=png&maptype=satellite&zoom=20&size=640x640&key=AIzaSyDdzK4fey4N9dfCFDY78s02ICM3AyJ27Xk"
                        .Replace("<<latlon>>", latlon));
                    //client.BaseAddress = uri;
                    try
                    {

                        var imageContent = await client.GetByteArrayAsync(uri);

                        using (var imageBuffer = new MemoryStream(imageContent))
                        {
                            var image = System.Drawing.Image.FromStream(imageBuffer);
                            image.Save(fileName);
                            Thread.Sleep(1000);
                            //Do something with image
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
            }
            int usableImageSize = (int)((double)imageSize * overlap);
            int imageWidth = nCols * usableImageSize;
            imageWidth = ((imageWidth + 3) / 4) * 4;
            int imageHeight = nRows * usableImageSize;
            imageHeight = ((imageHeight + 3) / 4) * 4;
            System.Drawing.Bitmap finalBitmap = new System.Drawing.Bitmap(imageWidth, imageHeight);
            for (int nRow = 0; nRow < imageHeight; nRow++)
            {
                for(int nCol = 0; nCol < imageWidth; nCol++)
                {
                    int r = nRow % 256;
                    int g = nCol % 256;
                    int b = (nRow + nCol) % 256;
                    Color c = Color.FromArgb(240, r, g, b);
                    finalBitmap.SetPixel(nCol, nRow, c);
                }
            }
            for (int nCol = 0; nCol < nCols; nCol++)
            //for (int nCol = 0; nCol < 6; nCol++)
            {
                double colLon = lonLL + nCol * lonIncrement;
                for (int nRow = 0; nRow < nRows; nRow++)
                {
                    double rowLat = latLL + nRow * latIncrement;
                    string gmRC = latToY(rowLat, (uint)zoom).ToString() + "." + lonToX(colLon, (uint)zoom).ToString();
                    string latlon = rowLat.ToString("f7") + "," + colLon.ToString("f7");
                    string colrow = nCol.ToString("d2") + "." + nRow.ToString("d2");
                    string fileName = "c:\\temp\\models\\world\\satimages\\20." + gmRC + ".png";
                    if (!File.Exists(fileName))
                    {
                        continue;
                    }
                    Bitmap tileBitmap = new Bitmap(fileName);
                    for(int tRow = 0; tRow < imageSize; tRow++)
                    {
                        int outRow = tRow + ((nRows-1) - nRow) * (int)(imageSize * overlap);
                        if (outRow >= 0 && outRow < imageHeight)
                        {
                            for (int tCol = 0; tCol < imageSize; tCol++)
                            {
                                int outCol = tCol + nCol * (int)(imageSize * overlap);
                                if (outCol >= 0 && outCol < imageWidth)
                                {
                                    Color c = tileBitmap.GetPixel(tCol, tRow);
                                    finalBitmap.SetPixel(outCol, outRow, c);
                                }
                            }
                        }
                    }
                }
            }
            string finalFileName = "c:\\temp\\models\\world\\satimages\\Final.png";
            finalBitmap.Save(finalFileName);
        }

        static double RowsPerLatDegree(double minLat, double maxLat, uint zoom)
        {
            // get the delta lat per row for the given range;
            double deltaLat = maxLat - minLat;
            double minY = latToY(minLat, zoom);
            double maxY = latToY(maxLat, zoom);
            double deltaY = maxY - minY;
            //double deltaY = latToY(maxLat, zoom) - latToY(minLat, zoom);
            double result =  deltaY / deltaLat;
            return -result;
        }
        static double ColsPerLonDegree(double minLon, double maxLon, uint zoom)
        {
            // get the delta lon per row for the given range;
            double deltaX = lonToX(maxLon, zoom) - lonToX(minLon, zoom);
            double result = deltaX / (maxLon - minLon);
            return result;
        }


        static public uint lonToX(double lon, uint zoom)
        {
            uint offset = 256u << ((int)zoom - 1); // one pi worth of longitude
            return (uint)Math.Round(offset + (offset * lon / 180));
        }

        static public uint latToY(double lat, uint zoom)
        {
            uint offset = 256u << ((int)zoom - 1);
            return (uint)Math.Round(offset - offset / Math.PI * Math.Log((1.0 + Math.Sin(lat * Math.PI / 180.0)) / (1.0 - Math.Sin(lat * Math.PI / 180.0))) / 2.0);
        }
    }

}
