using UnityEngine;
using Run4YourLife.GameManagement;

public abstract class BossDestructedInstance : MonoBehaviour {

    [SerializeField]
    private float m_distance;

    public float DestroyPosition { get { return transform.position.x + m_distance; } }
    public float Distance { get { return m_distance; } }

    protected virtual void Start()
    {
        BossDestructorManager.Instance.Add(this);
    }

    public abstract void OnBossDestroy();

    public abstract void OnRegenerate();

    protected virtual void OnDrawGizmosSelected()
    {
        Vector3 position = transform.position;
        position.x = DestroyPosition;
        Gizmos.DrawSphere(position, 0.5f);
    }
}
