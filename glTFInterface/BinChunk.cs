using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace glTFInterface
{
    public class BinChunkStore
    {
        public List<byte[]> ChunkStore { get; set; } = new List<byte[]> ();
        public int AddChunk(ref byte[] chunk)
        {
            ChunkStore.Add(chunk);
            NextByte += chunk.Length;
            return NextByte;
        }
        public int AddChunk(float[] fchunk)
        {
            int nFloat = fchunk.Length;
            int nBytes = nFloat * sizeof(float);
            byte[] tbuffer = new byte[nBytes];
            System.Buffer.BlockCopy(fchunk, 0, tbuffer, 0, nBytes);
            return AddChunk(ref tbuffer);
        }
        public int AddChunk(ushort[] schunk)
        {
            int nShort = schunk.Length;
            int nBytes = nShort * sizeof(ushort);
            byte[] tbuffer = new byte[nBytes];
            System.Buffer.BlockCopy(schunk, 0, tbuffer, 0, nBytes);
            return AddChunk(ref tbuffer);
        }
        public int AddChunk(short[] schunk)
        {
            int nShort = schunk.Length;
            int nBytes = nShort * sizeof(short);
            byte[] tbuffer = new byte[nBytes];
            System.Buffer.BlockCopy(schunk, 0, tbuffer, 0, nBytes);
            return AddChunk(ref tbuffer);
        }
        public int NextByte { get; set; } = 0;
        public void WriteChunks(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                for (int nChunk = 0; nChunk < ChunkStore.Count; nChunk++)
                {
                    stream.Write(ChunkStore[nChunk], 0, ChunkStore[nChunk].Length);
                }
            }
        }
        public void Clear()
        {
            ChunkStore.Clear();
        }
    }
}
