using System.Threading;
using System.Threading.Tasks;

namespace Cubusky
{
    public interface ICompressor<T>
    {
        T Compress(T data) => CompressAsync(data).GetAwaiter().GetResult();
        T Decompress(T compressedData) => DecompressAsync(compressedData).GetAwaiter().GetResult();

        Task<T> CompressAsync(T data, CancellationToken cancellationToken = default);
        Task<T> DecompressAsync(T compressedData, CancellationToken cancellationToken = default);
    }

    public interface ICompressor : ICompressor<byte[]> { }
}
