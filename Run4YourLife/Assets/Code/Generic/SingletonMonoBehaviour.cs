using UnityEngine;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;
    private static GameObject m_instanceGameObject;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<T>();

                /* 

                if (m_instance == null)
                {
                    Debug.LogError("[Singleton] An instance of " + typeof(T) + 
                        " is needed in the scene, but has not been found");
                }
                else if (FindObjectsOfType(typeof(T)).Length > 1)
                {
                    Debug.LogError("[Singleton] Something went really wrong " +
                        " - there should never be more than 1 singleton!" +
                        " Reopening the scene might fix it.");
                }
                */
            }
            return m_instance;
        }
    }

    public static GameObject InstanceGameObject
    {
        get
        {
            if (m_instanceGameObject == null)
            {
                T instance = Instance;
                if (instance != null)
                {
                    m_instanceGameObject = instance.gameObject;
                }
            }
            return m_instanceGameObject;
        }
    }

    /// <summary>
    /// Clears singleton instance because object has been destroyed 
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (this == m_instance)
            m_instance = null;
    }
}