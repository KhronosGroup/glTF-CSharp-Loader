using System.Runtime;
using System.Text;

namespace Base64BufferGenerator
{
    internal class Program
    {
        static byte[] GetBytesDouble(double[] values)
        {
            var result = new byte[values.Length * sizeof(double)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static byte[] GetBytesInt32(Int32[] values)
        {
            var result = new byte[values.Length * sizeof(Int32)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static Int32[] GetInt32sAlt(byte[] bytes)
        {
            var result = new Int32[bytes.Length / sizeof(Int32)];
            Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
            return result;
        }
        static double[] GetDoubles(byte[] bytes)
        {
            var result = new double[bytes.Length / sizeof(double)];
            Buffer.BlockCopy(bytes, 0, result, 0, bytes.Length);
            return result;
        }
        static void Main(string[] args)
        {
            //string tBase64 = "AAABAAIAAAAAAAAAAAAAAAAAAAAAAIA/AAAAAAAAAAAAAAAAAACAPwAAAAA=";
            //double[] tDouble = GetDoublesAlt( Encoding.ASCII.GetBytes(tBase64));  
            double[] vtx = {

              0.0857, 0.0759,
              0.0367, 0.9726,
              0.9678, 0.0318,
              0.9754, 0.9327
            };

            int[] idx = {
              0, 2,
              2, 4,
              4, 0,
              0, 1,
              1, 4
            };
            byte[] inArray = GetBytesInt32(idx);
            string base64String = System.Convert.ToBase64String(inArray, 0, 40, Base64FormattingOptions.InsertLineBreaks);
            byte[] outArray = System.Convert.FromBase64String(base64String);
            int[] outInts = GetInt32sAlt(outArray);

            inArray = GetBytesDouble(vtx);
            base64String = System.Convert.ToBase64String(inArray, 0, 64, Base64FormattingOptions.InsertLineBreaks);
            outArray = System.Convert.FromBase64String(base64String);
            double[] outDoubles = GetDoubles(outArray);
        }
    }
}