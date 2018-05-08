using Run4YourLife.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairControl : MonoBehaviour {

    #region Inspector

    [SerializeField]
    private GameObject crossHair;

    [SerializeField]
    private float crossHairSpeed;

    #endregion

    #region Variables

    private bool crossHairBlock = false;

    #endregion

    public bool IsCrossHairActive()
    {
        return crossHair.GetComponent<CrossHair>().GetActive();
    }

    public void Translate(Vector3 input)
    {
        if (!crossHairBlock)
        {
            crossHair.transform.Translate(input * crossHairSpeed * Time.deltaTime);
        }
    }

    public Vector3 GetPosition()
    {
        return crossHair.transform.position;
    }

    public void ChangePosition(Vector3 newPosition)
    {
        if (!crossHairBlock)
        {
            crossHair.transform.position = newPosition;
        }
    }

    public void BlockCrossHair()
    {
        crossHairBlock = true;
    }

    public void UnblockCrossHair()
    {
        crossHairBlock = false;
    }
}
