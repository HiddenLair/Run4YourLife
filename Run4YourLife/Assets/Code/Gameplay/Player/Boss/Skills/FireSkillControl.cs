using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife;
using Run4YourLife.Player;

public class FireSkillControl : MonoBehaviour {

    [SerializeField]
    [Tooltip("Time it take for the skill to disappear")]
    private float m_skillTime;

    [SerializeField]
    [Tooltip("How long the player is burnt")]
    private float m_burnTime;

    [SerializeField]
    private StatusEffectSet m_burnStatusEffectSet;

    private void Awake()
    {
        Destroy(gameObject, m_skillTime);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag(Tags.Runner))
        {
            GameObject runner = collider.gameObject;

            Burned burned = runner.gameObject.GetComponent<Burned>();
            if (burned == null)
            {
                burned = gameObject.AddComponent<Burned>();
                burned.Init(m_burnTime, m_burnStatusEffectSet);
            }
            
            burned.RefreshTime();
        }
    }
}
