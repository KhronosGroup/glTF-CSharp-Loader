using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Http;

// OS_Mx_Base
// UL 50.93925384031266, -1.4729219239115887 ...  50.939231501601334 -1.4728973704950428 
// UR: 50.93922098838908, -1.4691131492000304 ... 50.939238779949285, -1.4691383887293623
// LL: 50.93651102170811, -1.4728974291043124 ... 50.93651055712209, -1.4729011469784137
// LR: 50.93651561359327, -1.4691285578670317 ... 50.93650514479016, -1.4691165325482984

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
            catch (System.Exception e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }

        }

    }
}