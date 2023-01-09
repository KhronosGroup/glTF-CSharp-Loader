using System;
using System.IO;
using System.Net.Http;
using System.Drawing;

namespace GoogleMapsConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 50.938733425687315,%20-1.470220960392684
            // 50.93563783737563, -1.4691744971337046 is lower right
            // need 6 rows up and two columns left at zoom 20
            double clatLR = 50.93563783737563;
            double clonLR = -1.4691744971337046 + 0.001718*0.25; //-1.4691744971337046;
            double latIncrement = 0.001082 * 0.25;
            double lonIncrement = 0.001718 * 0.25;
            int zoom = 20;
            HttpClient client = new HttpClient();
            for (int nCol = 6; nCol < 9; nCol++)
                //for (int nCol = 0; nCol < 6; nCol++)
                {
                    double colLon = clonLR - nCol * lonIncrement;
                for (int nRow = 0; nRow < 14; nRow++)
                {
                    double rowLat = clatLR + nRow * latIncrement;
                    string gmRC = latToY(rowLat, (uint)zoom).ToString() + "." + lonToX(colLon, (uint)zoom).ToString();
                    string latlon = rowLat.ToString("f7") + "," + colLon.ToString("f7");
                    string colrow = nCol.ToString("d2") + "." + nRow.ToString("d2");
                    string fileName = "c:\\temp\\models\\world\\20." + gmRC + ".png";
                    Uri uri = new Uri("https://maps.googleapis.com/maps/api/staticmap?center=<<latlon>>&format=png&maptype=satellite&zoom=20&size=640x640&key=AIzaSyDdzK4fey4N9dfCFDY78s02ICM3AyJ27Xk"
                        .Replace("<<latlon>>", latlon));
                    //client.BaseAddress = uri;
                    var imageContent = await client.GetByteArrayAsync(uri);

                    using (var imageBuffer = new MemoryStream(imageContent))
                    {
                        var image = System.Drawing.Image.FromStream(imageBuffer);
                        image.Save(fileName);
                        Thread.Sleep(1000);
                        //Do something with image
                    }
                }
            }

            /*
            uint crow = latToY(clat, (uint)zoom);
            uint ccol = lonToX(clon, (uint)zoom);
            Console.WriteLine(clat.ToString("f5") + ", " + clon.ToString("f5") + " is " + crow.ToString() + ", " + ccol.ToString()); ;
            double lat = 50.938733425687315;
            double lon = -1.470220960392684;
            zoom = 18;
            lat += 0.001082; // -320 pixels
            lon += 0.001718; // +320 pixels
            uint row = latToY(lat, (uint)zoom);
            uint col = lonToX(lon, (uint)zoom);
            Console.WriteLine(lat.ToString("f5") + ", " + lon.ToString("f5") + " is " + row.ToString() + ", " + col.ToString()); ;
            Console.WriteLine("rel row: " + ((int)row - (int)crow).ToString() + ", rel col: " + (col-ccol).ToString()); ;
            */
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
