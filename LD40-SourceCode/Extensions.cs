using UnityEngine;

namespace StefanRichings
{
    public enum Axis
        {
            X,
            Y,
            Z
        }

    public static class Extensions
    {
        public static void Error (this GameObject g, string message)
        {
            Debug.LogError(message + " [" + g.name + "]");
        }
    }
}

public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = gameObject.GetComponent<T>();
        }
        else
        {
            Debug.LogError("Singleton of type " + typeof(T) + " already exists, destroying gameobject " + gameObject.name);
            Destroy(gameObject);
        }
    }
}