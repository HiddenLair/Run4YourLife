using UnityEngine;
using Run4YourLife;
using Run4YourLife.Interactables;

public class DestroyWallOnCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        IBreakable breakable = collision.gameObject.GetComponent<IBreakable>();
        if(breakable != null)
        {
            breakable.Break();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Wall))
        {
            Transform currentTransform = other.gameObject.transform;
            IBreakable breakable = currentTransform.GetComponent<IBreakable>();
            while (breakable == null)
            {
                currentTransform = currentTransform.parent;
                breakable = currentTransform.GetComponent<IBreakable>();
                Debug.Assert(currentTransform != transform.root);
            }
            breakable.Break();
        }
    }
}
