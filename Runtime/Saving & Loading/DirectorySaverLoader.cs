using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubusky
{
    /// <summary>
    /// Saves data to a directory.
    /// </summary>
    public interface IDirectorySaver : IFileSaver, IEnumerableSaver, IEnumerableSaver<string[]>, IEnumerableSaver<IEnumerable<string>>
    {
        new string path { get; }
        string extension { get; }

        string IFileSaver.path => GetRandomFilePath();
        private string GetRandomFilePath() => Path.Combine(path, Path.ChangeExtension(Path.GetRandomFileName(), extension));

        private void Save<TData>(IEnumerable<TData> data, Action<string, TData> save)
        {
            Directory.CreateDirectory(path);
            foreach (var element in data)
            {
                save(GetRandomFilePath(), element);
            }
        }

        private IEnumerable<Task> SaveAsyncEnumerable<TData>(IEnumerable<TData> data, Func<string, TData, CancellationToken, Task> saveAsync, CancellationToken cancellationToken)
        {
            Directory.CreateDirectory(path);
            foreach (var element in data)
            {
                yield return saveAsync(GetRandomFilePath(), element, cancellationToken);
            }
        }

        void ISaver<IEnumerable<string>>.Save(IEnumerable<string> data) => Save(data, File.WriteAllText);
        Task ISaver<IEnumerable<string>>.SaveAsync(IEnumerable<string> data, CancellationToken cancellationToken) => Task.WhenAll(SaveAsyncEnumrable(data, cancellationToken));
        IEnumerable<Task> IEnumerableSaver<string>.SaveAsyncEnumrable(IEnumerable<string> data, CancellationToken cancellationToken) => SaveAsyncEnumerable(data, File.WriteAllTextAsync, cancellationToken);

        void ISaver<IEnumerable<byte[]>>.Save(IEnumerable<byte[]> data) => Save(data, File.WriteAllBytes);
        IEnumerable<Task> IEnumerableSaver<byte[]>.SaveAsyncEnumrable(IEnumerable<byte[]> data, CancellationToken cancellationToken) => SaveAsyncEnumerable(data, File.WriteAllBytesAsync, cancellationToken);

        void ISaver<IEnumerable<string[]>>.Save(IEnumerable<string[]> data) => Save(data, File.WriteAllLines);
        IEnumerable<Task> IEnumerableSaver<string[]>.SaveAsyncEnumrable(IEnumerable<string[]> data, CancellationToken cancellationToken) => SaveAsyncEnumerable(data, File.WriteAllLinesAsync, cancellationToken);

        void ISaver<IEnumerable<IEnumerable<string>>>.Save(IEnumerable<IEnumerable<string>> data) => Save(data, File.WriteAllLines);
        IEnumerable<Task> IEnumerableSaver<IEnumerable<string>>.SaveAsyncEnumrable(IEnumerable<IEnumerable<string>> data, CancellationToken cancellationToken) => SaveAsyncEnumerable(data, File.WriteAllLinesAsync, cancellationToken);
    }

    /// <summary>
    /// Loads data from a directory.
    /// </summary>
    public interface IDirectoryLoader : IEnumerableLoader, IEnumerableLoader<string[]>
    {
        string path { get; }
        string searchPattern { get; }
        SearchOption searchOption { get; }

        private IEnumerable<string> FilePaths => Directory.EnumerateFiles(path, searchPattern, searchOption);

        IEnumerable<string> ILoader<IEnumerable<string>>.Load<TData>() => FilePaths.Select(File.ReadAllText);
        async IAsyncEnumerable<string> IEnumerableLoader<string>.LoadAsyncEnumerable<TData>([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var filePath in FilePaths)
            {
                yield return await File.ReadAllTextAsync(filePath, cancellationToken);
            }
        }

        IEnumerable<byte[]> ILoader<IEnumerable<byte[]>>.Load<TData>() => FilePaths.Select(File.ReadAllBytes);
        async IAsyncEnumerable<byte[]> IEnumerableLoader<byte[]>.LoadAsyncEnumerable<TData>([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var filePath in FilePaths)
            {
                yield return await File.ReadAllBytesAsync(filePath, cancellationToken);
            }
        }

        IEnumerable<string[]> ILoader<IEnumerable<string[]>>.Load<TData>() => FilePaths.Select(File.ReadAllLines);
        async IAsyncEnumerable<string[]> IEnumerableLoader<string[]>.LoadAsyncEnumerable<TData>([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var filePath in FilePaths)
            {
                yield return await File.ReadAllLinesAsync(filePath, cancellationToken);
            }
        }
    }

    [Serializable]
    public class DirectorySaverLoader : UnityPath, IDirectorySaver, IDirectoryLoader, IEnumerableSaverLoader, IEnumerableSaverLoader<string[]>
    {
        [field: SerializeField, Path(withoutExtension = true)] public override string relativePath { get; set; }
        [field: SerializeField] public string extension { get; set; } = "json";
        [field: SerializeField] public string searchPattern { get; set; } = "*.json";
        [field: SerializeField] public SearchOption searchOption { get; set; }

        string IDirectorySaver.path => fullPath;
        string IDirectoryLoader.path => fullPath;
    }
}