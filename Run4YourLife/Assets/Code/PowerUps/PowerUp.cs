using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private StatModifier statModifier;

    void OnTriggerEnter(Collider collider)
    {
        collider.GetComponent<Stats>().AddModifier(statModifier);

        Destroy(gameObject);
    }
}