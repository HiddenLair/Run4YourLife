using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateByVisibleRender : MonoBehaviour {

    [SerializeField]
    private GameObject toActivate;

    private void OnBecameVisible()
    {
        toActivate.GetComponent<IActivateByRender>().Activate();
    }
}
