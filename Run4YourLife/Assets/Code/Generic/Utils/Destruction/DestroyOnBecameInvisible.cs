using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBecameInvisible : MonoBehaviour {

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
