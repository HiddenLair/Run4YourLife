using UnityEngine;
using Run4YourLife;
using Run4YourLife.Interactables;

public class DestroyWallOnCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == Tags.Wall)
        {
            collision.gameObject.GetComponent<BreakByDash>().Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Wall))
        {
            Transform currentTransform = other.gameObject.transform;
            BreakByDash breaker= currentTransform.GetComponent<BreakByDash>();
            while (breaker == null)
            {
                currentTransform = currentTransform.parent;
                breaker = currentTransform.GetComponent<BreakByDash>();
                Debug.Assert(currentTransform != transform.root);
            }
            breaker.Break();
        }
    }
}
