# Saving & Loading

How you save and load files can be a complicated subject. There can be a lot of different methods of saving and loading your data, and all of them are valid. Cubusky therefor exposes [interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) to abstract all possible implementations to a few simple methods. This allows anyone to easily integrate their own solution for saving and loading based on their needs, which can be used in any scripts that reference these [interfaces](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/interface) free of charge.

# Implementation

To use saving & loading in code, reference an `ISaverLoader` using `ReferenceDropdown` and start using it.

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ISaverLoader saverLoader;

    private void Start() => var json = saverLoader.Load<string>();
    private void OnDestroy() => saverLoader.Save("{ \"name\": \"My Json!\" }");
}
```

If you only want to use saving _or_ loading capabilities, simply specify the desired implementation.

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ISaver saver;
    [SerializeReference, ReferenceDropdown] public ILoader loader;

    private void Start()
    {
        saver.Load<string>();                       // compiler error!
        loader.Save("{ \"name\": \"My Json!\" }");  // compiler error!
    }
}
```

If you need save and/or load collections of data, use the `IEnumerableSaverLoader` interfaces.

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public IEnumerableSaver saver;
    [SerializeReference, ReferenceDropdown] public IEnumerableLoader loader;
    [SerializeReference, ReferenceDropdown] public IEnumerableSaverLoader saverLoader;

    private void Start()
    {
        saver.Save(new [] 
        {  
            "{ \"name\": \"My First Json!\" }",
            "{ \"name\": \"My Second Json!\" }"
        });

        var jsons = loader.Load<IEnumerable<string>>();
    }
}
```

Although it is not recommended, if you want to save and/or load data of a generic type, use the generic interfaces.

```csharp
public class MyMonoBehaviour : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeReference, ReferenceDropdown(typeof(ISaverLoader<State>))] private object _stateSaverLoader;

    public ISaverLoader<State> stateSaverLoader { get; set; }

    void ISerializationCallbackReceiver.OnBeforeSerialize() => _stateSaverLoader = stateSaverLoader as object;
    void ISerializationCallbackReceiver.OnAfterDeserialize() => stateSaverLoader = _stateSaverLoader as ISaverLoader<State>;

    State state;

    private void Start()
    {
        state = stateSaverLoader.Load<State>();
    }

    private void OnDestroy()
    {
        stateSaverLoader.Save(state);
    }
}
```

## Asynchronous Saving & Loading

`ISaverLoader` and friends come with `async` implementations as well since saving & loading is an operation that in most cases can be savely performed on a different thread. It is especially useful for e.g. saving & loading from a server, which may otherwise block the editor for a noticable amount of time.

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ISaverLoader saverLoader;

    private async void Start() => var json = await saverLoader.LoadAsync<string>();
    private void OnDestroy() => saverLoader.SaveAsync("{ \"name\": \"My Json!\" }");
}
```

## Saving & Serialization

It is important to note that saving is NOT the same as serialization.
- Serialization translates state into a format, usually json, that can be stored and reconstructed.
- Saving is used to store data, usually json, to a location, usually a file or a server.
- Loading is used to retrieve data, usually json, from a location, usually a file or a server.

Whenever you work with `ISaverLoader`, you will need to serialize and deserialize the data before saving and loading. Keeping these steps apart allows them both to be implemented generically. 

```csharp
public class MyMonoBehaviour : MonoBehaviour
{
    [SerializeReference, ReferenceDropdown] public ISaverLoader saverLoader;

    private State state;

    private void Start()
    {
        var json = saverLoader.Load<string>();
        state = JsonUtility.FromJson<State>(json);
    }

    private void OnDestroy()
    {
        var json = JsonUtility.ToJson(state);
        saverLoader.Save(json);
    }
}
```