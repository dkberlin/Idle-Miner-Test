using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = (T) FindObjectOfType(typeof(T));

            if (instance != null) return instance;
            var go = new GameObject(typeof(T).ToString());
            go.AddComponent(typeof(T));

            return instance;
        }
    }
}