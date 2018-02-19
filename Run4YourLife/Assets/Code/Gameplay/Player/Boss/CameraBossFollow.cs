using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.UI;


public class CameraBossFollow : MonoBehaviour {

    public Transform boss;
    public Vector3 bossPositionOffset;
    public float bossAndFloorHeight;

    public float timeFrequency;
    private float trauma;
    public float traumaDecreaseSpeed;
    public float maxRotationalScreenShakeAngle;
    public float maxTranslationalScreenShakeOffset;

    private bool lockTrauma;

    public Image traumaImage;
    public Image traumaCubeImage;
    public GameObject translational;
    public GameObject rotational;

    private void LateUpdate()
    {
        if (!lockTrauma)
            DecreaseTraumaLinear();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            trauma = 1;
            lockTrauma = !lockTrauma;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddTrauma(0.5f);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            bossAndFloorHeight += 2f * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            bossAndFloorHeight -= 2f * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            rotational.SetActive(!rotational.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            translational.SetActive(!translational.activeSelf);
        }

        traumaImage.fillAmount = trauma;
        traumaCubeImage.fillAmount = trauma * trauma * trauma;


        transform.position = CalculateCameraPosition();
        transform.LookAt(CalculateLookAtPosition());

        if (translational.activeSelf)
            transform.position += GetTranslationalCameraShake();

        if (rotational.activeSelf)
            transform.rotation *= GetRotationalCameraShake();
    }

    private Vector3 GetTranslationalCameraShake()
    {
        Vector3 cameraShake = new Vector3
        {
            x = GetRandomPerlinNoiseTimeBased(20) * trauma * trauma * trauma * maxTranslationalScreenShakeOffset,
            y = GetRandomPerlinNoiseTimeBased(40) * trauma * trauma * trauma * maxTranslationalScreenShakeOffset
        };
        return cameraShake;
    }

    private Quaternion GetRotationalCameraShake()
    {
        return Quaternion.Euler(0, 0, GetRandomPerlinNoiseTimeBased(0) * trauma * trauma * trauma * maxRotationalScreenShakeAngle);
    }


    /// <summary>
    /// Get perlin noise that uses time as seed
    /// </summary>
    /// <param name="seed">seed that will be given to the perlin noise function</param>
    /// <returns>value in the range [-1,1]</returns>
    private float GetRandomPerlinNoiseTimeBased(float seed)
    {
        float timeSeed = Time.time * timeFrequency;
        return (Mathf.PerlinNoise(timeSeed, seed) - 0.5f)* 2.0f;
    }

    public void AddTrauma(float amount)
    {
        trauma = Mathf.Clamp01(trauma + amount);
    }

    private void DecreaseTraumaLinear()
    {
        trauma = Mathf.Clamp01(trauma - traumaDecreaseSpeed * Time.deltaTime);
    }

    private Vector3 CalculateCameraPosition()
    {
        Camera camera = GetComponent<Camera>();
        Vector3 lookAt = CalculateLookAtPosition();

        float z = bossAndFloorHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2.0f));

        return lookAt + -boss.forward * z;
    }

    private Vector3 CalculateLookAtPosition()
    {
        Camera camera = GetComponent<Camera>();
        float aspectRatio = (float)Screen.width / Screen.height;

        float x = bossAndFloorHeight * aspectRatio;

        return boss.position + boss.rotation * bossPositionOffset + boss.right * (x / 2.0f) + Vector3.up * (bossAndFloorHeight / 2.0f);
    }
}
