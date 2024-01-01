# OfType Attribute

OfType was created to allow [interface](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) specification inside an [Object Field](https://www.foundations.unity.com/components/object-field).

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
	[SerializeField, OfType(typeof(IConstraint))] private Component _constraint;
	
	public IConstraint constraint { get; set; }
	
	void ISerializationCallbackReceiver.OnBeforeSerialize() => _constraint = constraint as Component;
	void ISerializationCallbackReceiver.OnAfterDeserialize() => constraint = _constraint as IConstraint;
}
```