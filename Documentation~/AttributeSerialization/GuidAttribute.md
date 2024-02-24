# [Guid](https://learn.microsoft.com/en-us/dotnet/api/system.guid) Attribute

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField, Guid] private string _guid;

    public Guid guid { get; set; }

    void ISerializationCallbackReceiver.OnBeforeSerialize() => _guid = guid.ToString();
    void ISerializationCallbackReceiver.OnAfterDeserialize() => guid = new(_guid);
}
```