using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonDestroyOnLoad : MonoBehaviour
{
    private static DonDestroyOnLoad instance = null;
    // Start is called before the first frame update
    private void Awake()
    {
        if (instance)
        {
            DestroyImmediate(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
