# Animator Hash

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    public AnimatorHash jump = "Jump";                  // Takes string input.
    public void DoJump() => animator.SetTrigger(jump);  // Returns int output.
}
```