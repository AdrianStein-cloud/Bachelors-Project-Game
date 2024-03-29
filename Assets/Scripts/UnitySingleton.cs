using UnityEngine;

public abstract class UnitySingleton<T> : MonoBehaviour
{
    public static T Instance { get; private set; }

    public static void BecomeSingleton(T instance)
    {
        Instance = instance;
    }
}
