using System.Net;
using System.Text.Json;
namespace OpenTopo
{
    public class Elevation
    {
        public double Value { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }
    public class ElevationRequests
    {
        public static async Task RequestElevationOnPath(string database, Elevation[] path )
        {
            /*
             * 
             * curl https://api.opentopodata.org/v1/eudem25m?locations=50.936764404085416,-1.4702295790037962
             * {
             *  "results": [
             *   {
             *     "dataset": "eudem25m",
             *     "elevation": 11.047615051269531,
             *     "location": {
             *       "lat": 50.936764404085416,
             *       "lng": -1.4702295790037962
             *     }
             *   }
             * ],
             * "status": "OK"
             * }
             * 
             * 
             */
            HttpClient client = new HttpClient();   
            string uri = "https://api.opentopodata.org/v1/" + database + "?locations=" + "50.936764404085416,-1.4702295790037962";
            try
            {
                string responseBody = await client.GetStringAsync(uri);

                Console.WriteLine(responseBody);
            }
            catch (Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }

    }
}