# Application Path

See also [Application.dataPath](https://docs.unity3d.com/ScriptReference/Application-dataPath.html), [Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html), [Application.streamingAssetsPath](https://docs.unity3d.com/ScriptReference/Application-streamingAssetsPath.html), [Application.temporaryCachePath](https://docs.unity3d.com/ScriptReference/Application-temporaryCachePath.html), [Application.consoleLogPath](https://docs.unity3d.com/ScriptReference/Application-consoleLogPath.html).

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    public ApplicationPath applicationPath = ApplicationPath.DataPath;

    public string path => applicationPath.GetPath(); // Will return Application.dataPath
}
```