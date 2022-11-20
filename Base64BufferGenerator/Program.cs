using System;

using System.Runtime;
using System.Text;

namespace Base64BufferGenerator
{
    internal class Program
    {
        static void ENUDoubleToglTFFloat(double east, double north, double up, out float x, out float y, out float z)
        {
            x = (float)east;
            y = (float)up;
            z = -(float)north;
        }
        static void oglTFFloatToENUDouble(float x, float y, float z, out double east, out double north, out double up)
        {
            east = x;
            up = y;
            north = -z;
        }
        static byte[] GetBytesFloat(float[] values)
        {
            var result = new byte[values.Length * sizeof(float)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static byte[] GetBytesInt16(System.Int16[] values)
        {
            var result = new byte[values.Length * sizeof(short)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static byte[] GetBytesDouble(double[] values)
        {
            var result = new byte[values.Length * sizeof(double)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static byte[] GetBytesInt32(System.Int32[] values)
        {
            var result = new byte[values.Length * sizeof(Int32)];
            Buffer.BlockCopy(values, 0, result, 0, result.Length);
            return result;
        }
        static Int16[] GetInt16s(byte[] bytes, int start, int count)
        {
            var result = new Int16[count / sizeof(Int16)];
            Buffer.BlockCopy(bytes, start, result, 0, count);
            return result;
        }
        static Int32[] GetInt32s(byte[] bytes, int start, int count)
        {
            var result = new Int32[count / sizeof(Int32)];
            Buffer.BlockCopy(bytes, start, result, 0, count);
            return result;
        }
        static float[] GetFloats(byte[] bytes, int start, int count)
        {
            var result = new float[count / sizeof(float)];
            Buffer.BlockCopy(bytes, start, result, 0, count);
            return result;
        }
        static double[] GetDoubles(byte[] bytes, int start, int count)
        {
            var result = new double[count / sizeof(double)];
            Buffer.BlockCopy(bytes, start, result, 0, count);
            return result;
        }
        static void Main(string[] args)
        {
            string tBase64 = "AACAvwAAgD8AAIA/AACAPwAAgD8AAIC/AACAvwAAgD8AAIC/AACAPwAAgD8AAIA/AACAPwAAgD8AAIA/AACAvwAAgL8AAIA/AACAPwAAgL8AAIA/AACAvwAAgD8AAIA/AACAvwAAgD8AAIA/AACAvwAAgL8AAIC/AACAvwAAgL8AAIA/AACAvwAAgD8AAIC/AACAPwAAgL8AAIA/AACAvwAAgL8AAIC/AACAPwAAgL8AAIC/AACAvwAAgL8AAIA/AACAPwAAgD8AAIC/AACAPwAAgL8AAIA/AACAPwAAgL8AAIC/AACAPwAAgD8AAIA/AACAvwAAgD8AAIC/AACAPwAAgL8AAIC/AACAvwAAgL8AAIC/AACAPwAAgD8AAIC/AAAAAAAAgD8AAAAAAAAAAAAAgD8AAAAAAAAAAAAAgD8AAAAAAAAAAAAAgD8AAAAAAAAAAAAAAAAAAIA/AAAAAAAAAAAAAIA/AAAAAAAAAAAAAIA/AAAAAAAAAAAAAIA/AACAvwAAAAAAAAAAAACAvwAAAAAAAAAAAACAvwAAAAAAAAAAAACAvwAAAAAAAAAAAAAAAAAAgL8AAACAAAAAAAAAgL8AAACAAAAAAAAAgL8AAACAAAAAAAAAgL8AAACAAACAPwAAAAAAAAAAAACAPwAAAAAAAAAAAACAPwAAAAAAAAAAAACAPwAAAAAAAAAAAAAAAAAAAAAAAIC/AAAAAAAAAAAAAIC/AAAAAAAAAAAAAIC/AAAAAAAAAAAAAIC/AAAgPwAAQD8AAMA+AACAPwAAID8AAIA/AADAPgAAQD8AACA/AABAPwAAwD4AAAA/AADAPgAAQD8AACA/AAAAPwAAID8AAAA/AADAPgAAgD4AAMA+AAAAPwAAID8AAIA+AAAgPwAAAAAAAMA+AACAPgAAID8AAIA+AADAPgAAAAAAAMA+AAAAPwAAAD4AAIA+AAAAPgAAAD8AAMA+AACAPgAAYD8AAAA/AAAgPwAAgD4AACA/AAAAPwAAYD8AAIA+AAABAAIAAAADAAEABAAFAAYABAAHAAUACAAJAAoACAALAAkADAANAA4ADAAPAA0AEAARABIAEAATABEAFAAVABYAFAAXABUA";
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
            //byte[] inArray = GetBytesInt32(idx);
            //string base64String = System.Convert.ToBase64String(inArray, 0, 40, Base64FormattingOptions.InsertLineBreaks);
            //byte[] outArray = System.Convert.FromBase64String(base64String);
            //int[] outInts = GetInt32s(outArray, 0, );

            //inArray = GetBytesDouble(vtx);
            //base64String = System.Convert.ToBase64String(inArray, 0, 64, Base64FormattingOptions.InsertLineBreaks);
            //outArray = System.Convert.FromBase64String(base64String);
            //double[] outDoubles = GetDoubles(outArray);

            byte[] outArray = System.Convert.FromBase64String(tBase64);
            //Int16[] outInt16s = GetInt16s(outArray, 0, 6);
            //float[] outFloats = GetFloats(outArray, 8, 36);

            // 24 vec3, 24 vec3, 24 vec2, 36 shorts
            // east +/-140 m
            // north +/- 160 m
            // up 10 m - 60 m

            float[] vec3_1 = GetFloats(outArray, 0, 4 * 24 * 3);
            float[] vec3_2 = GetFloats(outArray, 4 * 24 * 3, 4 * 24 * 3);
            float[] vec2_1 = GetFloats(outArray, 2 * 4 * 24 * 3, 4 * 24 * 2);
            Int16[] int_1 = GetInt16s(outArray, 2 * 4 * 24 * 3 + 4 * 24 * 2, 2 * 36);

            // go though first chunk
            // x *= 140, y *=160, if z== -1 then 10 else 60
            // convert to glTF 
            for(int nCoord = 0; nCoord < 24 * 3; nCoord +=3)
            {
                double east = (vec3_1[nCoord] * 140.0);
                double north = (vec3_1[nCoord + 1] * 160.0);
                double up = 60.0;
                if (vec3_1[nCoord + 2] < 0.0)
                {
                    up = 10.0;
                }
                ENUDoubleToglTFFloat(east, north, up, out vec3_1[nCoord], out vec3_1[nCoord + 1], out vec3_1[nCoord + 2]);
                //vec3_1[nCoord] *= (float)-140.0;
                //vec3_1[nCoord+1] *= (float)-160.0;
               // if (vec3_1[nCoord + 2] < 0.0)
                //{
                //    vec3_1[nCoord + 2] = (float)10.0;
                //}
               // else
               // {
                //    vec3_1[nCoord + 2] = (float)60.0;
               // }
            }

            byte[] newVec3_1Bytes = GetBytesFloat(vec3_1);
            for(int nByte = 0; nByte < 288; nByte++)
            {
                outArray[nByte] = newVec3_1Bytes[nByte];
            }
            // then recreate base64
            string newBase64 = System.Convert.ToBase64String(outArray, 0, outArray.Length, Base64FormattingOptions.None);

        }
    }
}