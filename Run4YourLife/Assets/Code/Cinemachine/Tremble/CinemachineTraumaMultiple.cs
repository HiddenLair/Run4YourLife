using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.GameManagement;
using Cinemachine;

namespace Run4YourLife.Cinemachine
{

    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineTraumaMultiple : CinemachineExtension
    {

        private class TrembleInfo
        {
            public TrembleInfo(TrembleConfig c, float t, float a) { config = c; timer = t; actualTraumaReduced = a; }
            public TrembleConfig config;
            public float timer;
            public float actualTraumaReduced;
        }

        List<TrembleInfo> trembles = new List<TrembleInfo>();

        ///<summary>
        /// Adds trauma to the same object camera
        ///</summary>
        public void AddTrauma(TrembleConfig config)
        {
            CinemachineVirtualCameraBase cinemachineVirtualCameraBase = GetComponent<CinemachineVirtualCameraBase>();
            AddTrauma(cinemachineVirtualCameraBase, config);
        }

        private void OnEnable()
        {
            TrembleManager.Instance.Subscribe(this);
        }

        private void OnDisable()
        {
            TrembleManager.Instance.Unsubscribe(this);
        }


        ///<summary>
        /// Adds trauma to the provided camera
        ///</summary>
        public void AddTrauma(CinemachineVirtualCameraBase vcam, TrembleConfig config)
        {
            VcamTraumaState cameraState = GetExtraState<VcamTraumaState>(vcam);

            trembles.Add(new TrembleInfo(config,0,0));

            cameraState.trauma = Mathf.Clamp01(cameraState.trauma + config.traumAmount);
        }

        class VcamTraumaState
        {
            public float trauma;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Noise)
            {
                VcamTraumaState cameraState = GetExtraState<VcamTraumaState>(vcam);
                if (trembles.Count > 0)
                {
                    float m_maxTranslationalMovement = 0;
                    float m_maxRotationalTraumaAngle = 0;
                    float frecuency = 0;

                    CalculateValuesFromTrembles(out m_maxTranslationalMovement, out m_maxRotationalTraumaAngle, out frecuency);

                    state.PositionCorrection += new Vector3()
                    {
                        x = GetRandomPerlinNoiseTimeBased(20, frecuency) * cameraState.trauma * cameraState.trauma * m_maxTranslationalMovement,
                        y = GetRandomPerlinNoiseTimeBased(40, frecuency) * cameraState.trauma * cameraState.trauma * m_maxTranslationalMovement
                    };
                    state.OrientationCorrection *= Quaternion.Euler(0, 0, GetRandomPerlinNoiseTimeBased(0, frecuency) * cameraState.trauma * cameraState.trauma * cameraState.trauma * m_maxRotationalTraumaAngle);

                    DecreaseTrauma(cameraState);
                }
            }
        }

        private void DecreaseTrauma(VcamTraumaState cameraState)
        {
            for (int i = 0; i < trembles.Count; ++i)
            {
                TrembleInfo info = trembles[i];
                if (info.config.useDuration)
                {
                    info.timer += Time.deltaTime;
                    if(info.timer < info.config.duration)
                    {
                        continue;//We dont decrease trauma, till this duration is over
                    }
                }
                cameraState.trauma = Mathf.Clamp01(cameraState.trauma - info.config.m_traumaDecreaseSpeed * Time.deltaTime);
                info.actualTraumaReduced += info.config.m_traumaDecreaseSpeed * Time.deltaTime;
                if(info.actualTraumaReduced >= info.config.traumAmount)
                {
                    trembles.Remove(info);
                }
            }
        }

        private void CalculateValuesFromTrembles(out float m_maxTranslationalMovement, out float m_maxRotationalTraumaAngle,out float frecuency)
        {
            m_maxTranslationalMovement = 0;
            m_maxRotationalTraumaAngle = 0;
            frecuency = 0;
            for (int i = 0; i < trembles.Count; ++i)
            {
                m_maxTranslationalMovement += trembles[i].config.m_maxTranslationalMovement;
                m_maxRotationalTraumaAngle += trembles[i].config.m_maxRotationalTraumaAngle;
                frecuency += trembles[i].config.m_frequency;
            }
            m_maxTranslationalMovement /= trembles.Count;
            m_maxRotationalTraumaAngle /= trembles.Count;
            frecuency /= trembles.Count;
        }

        /// <summary> 
        /// Get perlin noise that uses time as seed 
        /// </summary> 
        /// <param name="seed">seed that will be given to the perlin noise function</param> 
        /// <returns>value in the range [-1,1]</returns> 
        private float GetRandomPerlinNoiseTimeBased(float seed , float frecuency)
        {
            float timeSeed = Time.time * frecuency;
            return (Mathf.PerlinNoise(timeSeed, seed) - 0.5f) * 2.0f;
        }
    }
}
