using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;
using Run4YourLife.Utils;

[RequireComponent(typeof(Rigidbody))]
public class CoconutController : MonoBehaviour {

    public bool Aviable { get; private set; }

    private Rigidbody m_rigidbody;

    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();   
        Reset();
 
    }

    public void Fall()
    {
        Aviable = false;
        m_rigidbody.useGravity = true;
        m_rigidbody.isKinematic = false;
    }

    public void Reset()
    {
        Aviable = true;
        gameObject.SetActive(true);
        m_rigidbody.useGravity = false;
        m_rigidbody.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!Aviable) // it is falling
        {
            ExecuteEvents.Execute<ICharacterEvents>(other.gameObject, null, (x, y) => x.Kill());
            gameObject.SetActive(false);
        }
    }
}
