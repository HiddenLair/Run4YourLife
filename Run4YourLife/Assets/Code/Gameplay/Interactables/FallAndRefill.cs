using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FallAndRefill : MonoBehaviour {

    #region Inspector

    [SerializeField]
    private float gravity = 9.8f;

    [SerializeField]
    private float refillTime = 2.0f;

    [SerializeField]
    private GameObject graphics;

    #endregion

    #region Variables

    private float velocity = 0.0f;
    private bool falling = false;
    private Collider hitTrigger;
    private float refillTimer = 0.0f;

    

    #endregion

    private void Awake()
    {
        hitTrigger = GetComponent<Collider>();
        hitTrigger.enabled = false;
        refillTimer = refillTime;
    }

    void FixedUpdate () {
        if (falling)
        {
            velocity += gravity * Time.fixedDeltaTime;
            transform.Translate(new Vector3(0,-velocity * Time.fixedDeltaTime, 0));
            refillTimer -= Time.fixedDeltaTime;
        }
	}

    public void Fall()
    {
        falling = true;
        hitTrigger.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("coconuthit");
        ExecuteEvents.Execute<ICharacterEvents>(other.gameObject, null, (x, y) => x.Kill());

        StartCoroutine(Refill(refillTimer));
    }

    private IEnumerator Refill(float time)
    {
        graphics.SetActive(false);
        this.enabled = false;
        hitTrigger.enabled = false;
        yield return new WaitForSeconds(time);

        transform.localPosition = new Vector3(0, 0, 0);
        refillTimer = refillTime;
        falling = false;
        velocity = 0.0f;
        this.enabled = true;
        graphics.SetActive(true);
    }
}
