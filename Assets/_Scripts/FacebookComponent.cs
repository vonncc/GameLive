using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR

#else
using Facebook.Unity;
#endif
public class FacebookComponent : MonoBehaviour
{
#if UNITY_EDITOR
    public bool Initialized = false;
#else
    public bool Initialized { get; private set; }

    private void Awake()
    {

        InitDelegate initDeleagete = () =>
        {
            print("success Init");
            Initialized = true;
        };

        FB.Init(initDeleagete);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
#endif
}
