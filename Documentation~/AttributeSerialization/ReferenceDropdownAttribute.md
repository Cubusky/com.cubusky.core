# ReferenceDropdown Attribute

ReferenceDropdown was created to allow selectable [interface](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) references inside of [Unity Objects](https://docs.unity3d.com/ScriptReference/Object.html). They work together with [SerializeReference](https://docs.unity3d.com/ScriptReference/SerializeReference.html) to serialize your [interface](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) so you can use it in the inspector.

```csharp
public interface ITest {}

public class StringTest : ITest { public string s; }

public class IntTest : ITest { public int i; }

public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ITest test;
}
```

![](images/ReferenceDropdown%20IntTest.png)

![](images/ReferenceDropdown%20StringTest.png)

## Restrictions

In order for an implementation to be selectable, it must adhere to the following rules:
- The implementation must have an empty constructor.
- The implementation must not be abstract.
- The implementation must not derive from [UnityEngine.Object](https://docs.unity3d.com/ScriptReference/Object.html).

## Nullable

You can specify your reference to be nullable through the following:

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown(nullable = true)] public ITest test;
}
```

![](images/ReferenceDropdown%20null.png)

## Types

Whilst [SerializeReference](https://docs.unity3d.com/ScriptReference/SerializeReference.html) can serialize [interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface), it cannot serialize generics. In order to circumvent this issue, you can specify [types](https://learn.microsoft.com/en-us/dotnet/api/system.type) that the reference needs to be [assignable from](https://learn.microsoft.com/en-us/dotnet/api/system.type.isassignablefrom).

```csharp
public interface ITest<T> {}

public class Text : ITest<string> {}

public class Password : ITest<string> {}

public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ITest<string> invalid; // invalid

    [SerializeReference, ReferenceDropdown(typeof(ITest<string>))] public object valid; // valid
}
```