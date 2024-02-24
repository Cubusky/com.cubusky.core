# Shader Id

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    public ShaderId smoothness = "_Glossiness";                 // Takes string input.
    public float GetSmoothness() => mat.GetFloat(smoothness);   // Returns int output.
}
```