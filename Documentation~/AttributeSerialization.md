# Attribute Serialization

When you need to serialize a type in Unity that can be serialized through one of its underlying values, Attribute Serialization is a pattern that allows for rapid type coupling. It has zero performance cost at runtime and negligable performance cost in the editor.

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField, Guid] private string _guid;

    public Guid guid { get; set; }

    void ISerializationCallbackReceiver.OnBeforeSerialize() => _guid = guid.ToString();
    void ISerializationCallbackReceiver.OnAfterDeserialize() => guid = new(_guid);
}
```

It is not recommended to put this in a wrapper. While it may initially seem that a wrapper would make the developers job easier, it would actually complicate the access hierarchy within MyMonoBehaviour, accruing technical debt. For an easy way to implement the above, consider creating a [snippet](https://learn.microsoft.com/en-us/visualstudio/ide/code-snippets?view=vs-2022) instead.

[!include[](GuidAttribute.md)]

[!include[](OfTypeAttribute.md)]

[!include[](TimeSpanAttribute.md)]