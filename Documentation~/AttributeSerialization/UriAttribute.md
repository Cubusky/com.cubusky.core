# [Uri](https://learn.microsoft.com/en-us/dotnet/api/system.uri) Attribute

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField, Uri] private string _uri;

    public Uri uri { get; set; }

    void ISerializationCallbackReceiver.OnBeforeSerialize() => _uri = uri.ToString();
    void ISerializationCallbackReceiver.OnAfterDeserialize() => uri = new(_uri);
}
```