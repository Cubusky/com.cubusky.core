using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cubusky
{
    #region Generic Interfaces
    /// <summary>Saves a <typeparamref name="TInput"/> to a single location.</summary>
    public interface ISaver<in TInput>
    {
        void Save(TInput data);
        Task SaveAsync(TInput data, CancellationToken cancellationToken = default) => Task.Run(() => Save(data), cancellationToken);
    }

    /// <summary>Saves a <see cref="IEnumerable{TInput}">IEnumerable</see>&lt;<typeparamref name="TInput"/>&gt; to multiple locations.</summary>
    public interface IEnumerableSaver<in TInput> : ISaver<IEnumerable<TInput>>
    {
        Task ISaver<IEnumerable<TInput>>.SaveAsync(IEnumerable<TInput> data, CancellationToken cancellationToken) => Task.WhenAll(SaveAsyncEnumrable(data, cancellationToken));
        IEnumerable<Task> SaveAsyncEnumrable(IEnumerable<TInput> data, CancellationToken cancellationToken = default);
    }

    /// <summary>Loads a <typeparamref name="TOutput"/> from a single location.</summary>
    public interface ILoader<TOutput>
    {
        TOutput Load<TData>() where TData : TOutput;
        Task<TOutput> LoadAsync<TData>(CancellationToken cancellationToken = default) where TData : TOutput => Task.Run(Load<TOutput>, cancellationToken);
    }

    /// <summary>Loads a <see cref="IEnumerable{TOutput}">IEnumerable</see>&lt;<typeparamref name="TOutput"/>&gt; from multiple locations.</summary>
    public interface IEnumerableLoader<TOutput> : ILoader<IEnumerable<TOutput>>
    {
        async IAsyncEnumerable<TOutput> LoadAsyncEnumerable<TData>([EnumeratorCancellation] CancellationToken cancellationToken = default) where TData : TOutput
        {
            using var enumerator = Load<IEnumerable<TOutput>>().GetEnumerator();
            while (await Task.Run(enumerator.MoveNext, cancellationToken).ConfigureAwait(false))
            {
                yield return enumerator.Current;
            }
        }
    }
    #endregion

    #region Typed Interfaces
    /// <inheritdoc/>
    public interface ISaver : ISaver<string>, ISaver<byte[]>
    {
        void ISaver<byte[]>.Save(byte[] data) => Save(Encoding.UTF8.GetString(data));
    }

    /// <inheritdoc/>
    public interface IEnumerableSaver : IEnumerableSaver<string>, IEnumerableSaver<byte[]>
    {
        void ISaver<IEnumerable<byte[]>>.Save(IEnumerable<byte[]> data) => Save(data.Select(Encoding.UTF8.GetString));
        IEnumerable<Task> IEnumerableSaver<byte[]>.SaveAsyncEnumrable(IEnumerable<byte[]> data, CancellationToken cancellationToken) => SaveAsyncEnumrable(data.Select(Encoding.UTF8.GetString), cancellationToken);
    }

    /// <inheritdoc/>
    public interface ILoader : ILoader<string>, ILoader<byte[]>
    {
        byte[] ILoader<byte[]>.Load<TData>() => Encoding.UTF8.GetBytes(Load<string>());
    }

    /// <inheritdoc/>
    public interface IEnumerableLoader : IEnumerableLoader<string>, IEnumerableLoader<byte[]>
    {
        IEnumerable<byte[]> ILoader<IEnumerable<byte[]>>.Load<TData>() => Load<IEnumerable<string>>().Select(Encoding.UTF8.GetBytes);
    }
    #endregion

    #region QoL Interfaces
    /// <summary>Saves and loads a <typeparamref name="TInputOutput"/> to and from a single location.</summary>
    public interface ISaverLoader<TInputOutput> : ISaver<TInputOutput>, ILoader<TInputOutput> { }

    /// <inheritdoc/>
    public interface ISaverLoader : ISaverLoader<string>, ISaverLoader<byte[]>, ISaver, ILoader { }

    /// <summary>Saves and loads a <see cref="IEnumerable{TInputOutput}">IEnumerable</see>&lt;<typeparamref name="TInputOutput"/>&gt; to and from multiple locations.</summary>
    public interface IEnumerableSaverLoader<TInputOutput> : IEnumerableSaver<TInputOutput>, IEnumerableLoader<TInputOutput> { }

    /// <inheritdoc/>
    public interface IEnumerableSaverLoader : IEnumerableSaverLoader<string>, IEnumerableSaverLoader<byte[]>, IEnumerableSaver, IEnumerableLoader { }
    #endregion
}