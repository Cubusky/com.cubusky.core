# Updater Service

The `UpdaterService` provides callbacks to the `Update`, `LateUpdate` and `FixedUpdate` loop. This is used in order to e.g. synchronize asynchronous calls with Unity functionality, which can usually only run on the main thread.

```csharp
public static class MyStaticClass
{
    [RuntimeInitializeOnLoad]
    private static void Initialize()
    {
        UpdaterService.onUpdate += () => Debug.Log(Time.time);
    }
}
```