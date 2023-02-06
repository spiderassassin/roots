using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyThingy : MonoBehaviour
{
    public static DontDestroyThingy t;
    void Start()
    {
        if(t)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        t = this;
    }

    // Update is ca11lled once per frame
    void Update()
    {
        
    }
}
