# Time Mode

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    public float time => TimeMode.Time.GetTime();                       // Same as Time.time
    public float unscaledTime => TimeMode.UnscaledTime.GetTime();       // Same as Time.unscaledTime
    public float fixedDeltaTime => TimeMode.FixedTime.GetDeltaTime();   // Same as Time.fixedDeltaTime
}
```