# Path Attribute

See also [Application Path](https://cubusky.github.io/com.cubusky.core/manual/SerializableValues/ApplicationPath.html).

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    public ApplicationPath applicationPath;
    public string relativePath;

    public string path => Path.Combine(applicationPath.GetPath(), relativePath);
}
```