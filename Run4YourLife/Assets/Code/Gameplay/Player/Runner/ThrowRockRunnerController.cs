using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Player;

public class ThrowRockRunnerController : MonoBehaviour {

    [SerializeField]
    private GameObject m_rockPrefab;

    [SerializeField]
    private Transform m_rockInstantiationTransform;

    [SerializeField]
    private Transform m_graphics;

    [SerializeField]
    [Tooltip("Only values ranging from [0,1] can be used")]
    private AnimationCurve m_angle;

    [SerializeField]
    [Tooltip("Only values ranging from [0,1] can be used")]
    private AnimationCurve m_force;

    [SerializeField]
    private float m_timeToFullyChargeThrow;

     

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            ThrowRock();
        }
    }

    private void ThrowRock()
    {
        StartCoroutine(ThrowRockCoroutine());
    }

    private IEnumerator ThrowRockCoroutine()
    {
        float startingTime = Time.time;
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.E) || Time.time >= startingTime + m_timeToFullyChargeThrow);

        float normalizedExecutionTime = Mathf.Min(Time.time - startingTime, m_timeToFullyChargeThrow) / m_timeToFullyChargeThrow;

        float force = m_force.Evaluate(normalizedExecutionTime);
        float angle = m_angle.Evaluate(normalizedExecutionTime);

        GameObject rock = Instantiate(m_rockPrefab, m_rockInstantiationTransform.position, Quaternion.identity);
        Rigidbody rigidbody = rock.GetComponent<Rigidbody>();
        rigidbody.velocity = GetThrowVelocity(force, angle);
    }

    private Vector3 GetThrowVelocity(float force, float angle)
    {
        return GetQuaternionforFacingDirection(angle) * Vector3.right * force;
    }

    private Quaternion GetQuaternionforFacingDirection(float angle)
    {
        float facingDirectionAngle = m_graphics.eulerAngles.y == 90 ? angle : 180 - angle;
        return Quaternion.Euler(0, 0, facingDirectionAngle);
    }
}
