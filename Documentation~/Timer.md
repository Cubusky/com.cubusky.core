# Timer



## UpdateSynchronizer

A common usecase might be to synchronize the `Timer` with Unity's Update loop. This can be easily achieved by adding an `UpdateSynchronizer` to the [SynchronizingObject](https://learn.microsoft.com/en-us/dotnet/api/system.timers.timer.synchronizingobject).

```csharp
public class MyMonoBehaviour
{
    public Timer timer = new(0.1)
    {
        SynchronizingObject = new UpdateSynchronizer();
    };

    private void Start()
    {
        // Note that without the UpdateSynchronizer, this code wouldn't run because
        // the transform is only accessible from the main thread.
        timer.Elapsed += elapsedTime => transform.localScale = Vector3.one * elapsedTime;
        timer.Start();
    }

    private void OnDestroy()
    {
        timer.Stop();
    }
}
```