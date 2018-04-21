using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateByInvisible : MonoBehaviour {

    [SerializeField]
    private GameObject toActivate;

    private void OnBecameInvisible()
    {
        toActivate.GetComponent<IDeactivateByInvisible>().Deactivate();
    }
}
