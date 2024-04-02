using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Cubusky
{
    [Serializable]
    public class GZipCompressor : ICompressor
    {
        public async Task<byte[]> CompressAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var uncompressedStream = new MemoryStream(data);
            using var compressedStream = new MemoryStream();
            using (var compressorStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                await uncompressedStream.CopyToAsync(compressorStream, cancellationToken);
            }

            return compressedStream.ToArray();
        }

        public async Task<byte[]> DecompressAsync(byte[] compressedData, CancellationToken cancellationToken = default)
        {
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressorStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();
            await decompressorStream.CopyToAsync(decompressedStream, cancellationToken);

            return decompressedStream.ToArray();
        }
    }
}
