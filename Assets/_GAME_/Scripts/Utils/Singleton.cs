using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{

    [SerializeField] public bool dontDestroyOnLoad;

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {

                    Debug.LogWarning("Singleton hasn't been found");

                    GameObject gameObject = new GameObject();

                    gameObject.name = typeof(T).Name;

                    instance = gameObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {

        if (instance == null)
        {
            instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this);
            }            
        }
        else
        {

            Destroy(gameObject.GetComponent<T>());
        }
    }
}
