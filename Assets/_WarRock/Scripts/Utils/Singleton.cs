using UnityEngine;



/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as `T myT = new T();`
/// To prevent that, add `protected T () {}` to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

    private static object _lock = new object();
    private static T instance;

    public static T Instance {
        get {
            // Force single thread, can't be run by multiple instances at the same time
            lock (_lock) {
                if (instance == null) {
                    if (!applicationIsQuitting) {

                        // Try find existing object of same type
                        T[] instances = (T[])FindObjectsOfType(typeof(T));
                        
                        // Create new
                        if (instances.Length <= 0) {
                            #if !RELEASE_MODE
                            Debug.Log("[Singleton] No existing singletons found of current type [" + typeof(T).ToString() + "].\nCreating new one.");
                            #endif

                            GameObject singleton = new GameObject("(singleton) " + typeof(T).ToString());

                            instance = singleton.AddComponent<T>();
                        }
                        
                        // Use existing
                        else {
                            // If multiple exist, destroy all but one
                            if (instances.Length > 1) {
                                #if !RELEASE_MODE
                                Debug.Log("[Singleton] Found multiple singletons of same type [" + typeof(T).ToString() + "].\nDestroying all but one.");
                                #endif
                            
                                for (int i = instances.Length - 1; i > 0; i--)
                                    Destroy(instances[i].gameObject);
                            }

                            // Return existing obj
                            #if !RELEASE_MODE
                            Debug.Log("[Singleton] Found existing singleton of type [" + typeof(T).ToString() + "].\nReturning found instance.");
                            #endif
                            
                            instance = instances[0];
                        }
                    }
                }

                return instance;
            }
        }
    }

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    private static bool applicationIsQuitting = false;
    public virtual void OnDestroy() {
        //applicationIsQuitting = true;
        //instance = null;
    }

    public void OnApplicationQuit() {
        applicationIsQuitting = true;
    }

    public virtual void Awake() {
        DontDestroyOnLoad(Instance.gameObject);
    }
    
}
