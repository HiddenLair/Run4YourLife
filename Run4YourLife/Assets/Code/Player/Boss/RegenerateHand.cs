using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerateHand : MonoBehaviour {

    private GameObject handToRecover;

    private void OnBecameInvisible()
    {
        handToRecover.SetActive(true);
    }

    public void SetHandToRecover(GameObject hand)
    {
        handToRecover = hand;
    }
}
