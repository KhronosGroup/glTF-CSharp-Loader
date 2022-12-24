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
