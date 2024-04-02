using System;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;

namespace Cubusky
{
    [Serializable]
    public class DeflateCompressor : ICompressor
    {
        /// <summary>
        /// Compresses a string and returns a deflate compressed, Base64 encoded string.
        /// </summary>
        /// <param name="uncompressedString">String to compress</param>
        public async Task<byte[]> CompressAsync(byte[] data, CancellationToken cancellationToken = default)
        {
            using var uncompressedStream = new MemoryStream(data);
            using var compressedStream = new MemoryStream();
            using (var compressorStream = new DeflateStream(compressedStream, CompressionMode.Compress))
            {
                await uncompressedStream.CopyToAsync(compressorStream, cancellationToken);
            }

            return compressedStream.ToArray();
        }

        /// <summary>
        /// Decompresses a deflate compressed, Base64 encoded string and returns an uncompressed string.
        /// </summary>
        /// <param name="compressedString">String to decompress.</param>
        public async Task<byte[]> DecompressAsync(byte[] compressedData, CancellationToken cancellationToken = default)
        {
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();
            await decompressorStream.CopyToAsync(decompressedStream, cancellationToken);

            return decompressedStream.ToArray();
        }
    }
}
