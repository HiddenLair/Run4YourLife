using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnBecomeInvisible : MonoBehaviour {

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
