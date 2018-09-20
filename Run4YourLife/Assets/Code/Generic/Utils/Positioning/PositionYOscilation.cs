using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Run4YourLife.Utils
{
    public class PositionYOscilation : MonoBehaviour
    {

        [SerializeField]
        private float timePerOscilation;

        [SerializeField]
        private float maximumVariation;

        private float yOffsetPerSec;
        private float actualOffset;
        private float initialYPos;

        private void Awake()
        {
            yOffsetPerSec = maximumVariation / timePerOscilation/2.0f;
            initialYPos = transform.localPosition.y;
        }

        // Update is called once per frame
        void Update()
        {
            actualOffset += yOffsetPerSec * Time.deltaTime;
            if (actualOffset > maximumVariation)
            {
                actualOffset = maximumVariation;
                yOffsetPerSec = -yOffsetPerSec;
            }
            if(actualOffset < 0)
            {
                actualOffset = 0;
                yOffsetPerSec = -yOffsetPerSec;
            }
            Vector3 tempPos = transform.localPosition;
            tempPos.y = initialYPos + actualOffset;
            transform.localPosition = tempPos;
        }
    }
}
