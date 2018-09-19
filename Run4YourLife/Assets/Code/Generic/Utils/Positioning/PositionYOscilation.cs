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
        private Vector3 lastPosition;

        private void Awake()
        {
            yOffsetPerSec = maximumVariation / timePerOscilation/2.0f;
            initialYPos = transform.position.y;
            lastPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            initialYPos += transform.position.y - lastPosition.y;
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
            lastPosition = transform.position;
            lastPosition.y = lastPosition.y + actualOffset;
            transform.position = lastPosition;
        }
    }
}
