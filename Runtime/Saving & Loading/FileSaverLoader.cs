using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Saves data to a filepath.
    /// </summary>
    public interface IFileSaver : ISaver, ISaver<string[]>, ISaver<IEnumerable<string>>
    {
        string path { get; }

        private void Save<TData>(TData data, Action<string, TData> save)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            save(path, data);
        }

        private Task SaveAsync<TData>(TData data, Func<string, TData, CancellationToken, Task> saveAsync, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            return saveAsync(path, data, cancellationToken);
        }

        void ISaver<string>.Save(string data) => Save(data, File.WriteAllText);
        Task ISaver<string>.SaveAsync(string data, CancellationToken cancellationToken) => SaveAsync(data, File.WriteAllTextAsync, cancellationToken);

        void ISaver<byte[]>.Save(byte[] data) => Save(data, File.WriteAllBytes);
        Task ISaver<byte[]>.SaveAsync(byte[] data, CancellationToken cancellationToken) => SaveAsync(data, File.WriteAllBytesAsync, cancellationToken);

        void ISaver<string[]>.Save(string[] data) => Save(data, File.WriteAllLines);
        Task ISaver<string[]>.SaveAsync(string[] data, CancellationToken cancellationToken) => SaveAsync(data, File.WriteAllLinesAsync, cancellationToken);

        void ISaver<IEnumerable<string>>.Save(IEnumerable<string> data) => Save(data, File.WriteAllLines);
        Task ISaver<IEnumerable<string>>.SaveAsync(IEnumerable<string> data, CancellationToken cancellationToken) => SaveAsync(data, File.WriteAllLinesAsync, cancellationToken);
    }

    /// <summary>
    /// Loads data from a filepath.
    /// </summary>
    public interface IFileLoader : ILoader, ILoader<string[]>
    {
        string path { get; }

        string ILoader<string>.Load<TData>() => File.ReadAllText(path);
        Task<string> ILoader<string>.LoadAsync<TData>(CancellationToken cancellationToken) => File.ReadAllTextAsync(path, cancellationToken);

        byte[] ILoader<byte[]>.Load<TData>() => File.ReadAllBytes(path);
        Task<byte[]> ILoader<byte[]>.LoadAsync<TData>(CancellationToken cancellationToken) => File.ReadAllBytesAsync(path, cancellationToken);

        string[] ILoader<string[]>.Load<TData>() => File.ReadAllLines(path);
        Task<string[]> ILoader<string[]>.LoadAsync<TData>(CancellationToken cancellationToken) => File.ReadAllLinesAsync(path, cancellationToken);
    }

    [Serializable]
    public class FileSaverLoader : UnityPath, IFileSaver, IFileLoader, ISaverLoader, ISaverLoader<string[]>
    {
        [field: SerializeField, Path] public override string relativePath { get; set; }
    }
}
