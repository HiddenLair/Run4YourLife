using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraBossFollow : MonoBehaviour {

    public Transform boss;
    public Vector3 bossPositionOffset;
    public float bossAndFloorHeight;

    public float timeFrequency;
    private float trauma;
    public float traumaDecreaseSpeed;
    public float maxRotationalScreenShakeAngle;
    public float maxTranslationalScreenShakeOffset;

    private void LateUpdate()
    {
        DecreaseTraumaLinear();

        if (Input.GetKey(KeyCode.Space))
            trauma = 1;

        transform.position = CalculateCameraPosition();
        transform.LookAt(CalculateLookAtPosition());

        transform.position += GetTranslationalCameraShake();
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
        float aspectRatio = (float)Screen.width / Screen.height;

        float z = bossAndFloorHeight / (2.0f * Mathf.Tan(Mathf.Deg2Rad * camera.fieldOfView / 2.0f));
        float horizontalFieldOfView = camera.fieldOfView * aspectRatio;
        float x = bossAndFloorHeight * aspectRatio;

        return boss.position + bossPositionOffset + new Vector3(x / 2.0f, bossAndFloorHeight / 2.0f, -z);
    }

    private Vector3 CalculateLookAtPosition()
    {
        Vector3 lookAt = CalculateCameraPosition();
        lookAt.z = boss.position.z;
        return lookAt;
    }
}
