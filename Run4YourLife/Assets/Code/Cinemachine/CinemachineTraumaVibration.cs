using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cinemachine;

namespace Run4YourLife.Cinemachine{

    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineTraumaVibration : CinemachineExtension
    {
        [SerializeField]
        private float m_maxRotationalTraumaAngle;

        [SerializeField]
        private float m_maxTranslationalMovement;

        [SerializeField]
        private float m_frequency;

        [SerializeField]
        private float m_traumaDecreaseSpeed;
        
        ///<summary>
        /// Adds trauma to the same object camera
        ///</summary>
        public void AddTrauma(float amount) 
        {
            CinemachineVirtualCameraBase cinemachineVirtualCameraBase = GetComponent<CinemachineVirtualCameraBase>();
            AddTrauma(cinemachineVirtualCameraBase, amount);
        }


        ///<summary>
        /// Adds trauma to the provided camera
        ///</summary>
        public void AddTrauma(CinemachineVirtualCameraBase vcam, float amount) 
        {
            VcamTraumaState cameraState = GetExtraState<VcamTraumaState>(vcam);

            cameraState.trauma = Mathf.Clamp01(cameraState.trauma + amount); 
        }

        class VcamTraumaState
        {
            public float trauma;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if(stage == CinemachineCore.Stage.Noise)
            {
                VcamTraumaState cameraState = GetExtraState<VcamTraumaState>(vcam);
                
                state.PositionCorrection += new Vector3()
                { 
                    x = GetRandomPerlinNoiseTimeBased(20) * cameraState.trauma * cameraState.trauma * cameraState.trauma * m_maxTranslationalMovement, 
                    y = GetRandomPerlinNoiseTimeBased(40) * cameraState.trauma * cameraState.trauma * cameraState.trauma * m_maxTranslationalMovement 
                }; 
                state.OrientationCorrection *= Quaternion.Euler(0, 0, GetRandomPerlinNoiseTimeBased(0) * cameraState.trauma * cameraState.trauma * cameraState.trauma * m_maxRotationalTraumaAngle);
                
                
                cameraState.trauma = Mathf.Clamp01(cameraState.trauma - m_traumaDecreaseSpeed * Time.deltaTime);
            }
        } 

        /// <summary> 
        /// Get perlin noise that uses time as seed 
        /// </summary> 
        /// <param name="seed">seed that will be given to the perlin noise function</param> 
        /// <returns>value in the range [-1,1]</returns> 
        private float GetRandomPerlinNoiseTimeBased(float seed) 
        { 
            float timeSeed = Time.time * m_frequency; 
            return (Mathf.PerlinNoise(timeSeed, seed) - 0.5f)* 2.0f; 
        } 
    }
}
