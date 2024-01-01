# [TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan) Attribute

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
	[SerializeField, TimeSpan] private long _timeSpan;
	
	public TimeSpan timeSpan { get; set; }
	
	void ISerializationCallbackReceiver.OnBeforeSerialize() => _timeSpan = timeSpan.Ticks;
	void ISerializationCallbackReceiver.OnAfterDeserialize() => timeSpan = new(_timeSpan);
}
```