using UnityEngine;
using Run4YourLife.GameManagement;
using Run4YourLife.Player;
using UnityEngine.EventSystems;

using Run4YourLife;

public class RockController : MonoBehaviour
{
    private PlayerHandle playerWhoThrew;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.BossRockScore))
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void SetplayerHandle(PlayerHandle playerHandle)
    {
        playerWhoThrew = playerHandle;
    }
}
