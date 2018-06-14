using UnityEngine;
using Run4YourLife;
using Run4YourLife.Interactables;

public class DestroyWallOnCollision : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == Tags.Wall)
        {
            collision.gameObject.GetComponent<BreakByDash>().ManualBreak();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.Wall)
        {
            other.gameObject.GetComponent<BreakByDash>().ManualBreak();
        }
    }
}
