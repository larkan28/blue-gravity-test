using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T k_instance = null;

    public static T Instance
    {
        get
        {
            if (k_instance == null)
            {
                k_instance = (T)FindObjectOfType(typeof(T));

                if (k_instance == null)
                {
                    GameObject singletonHolder = new();
                    k_instance = singletonHolder.AddComponent<T>();

                    singletonHolder.name = "_" + typeof(T).ToString() + " (Singleton)";
                    DontDestroyOnLoad(singletonHolder);
                }
            }

            return k_instance;
        }
    }

    public static bool IsExisting
    {
        get
        {
            return (T)FindObjectOfType(typeof(T)) != null;
        }
    }

    private void OnApplicationQuit()
    {
        k_instance = null;
    }

    private void OnDestroy()
    {
        k_instance = null;
    }
}
