using UnityEngine;

public class CustomParticleCulling : MonoBehaviour
{
    [SerializeField]
    private float m_cullingRatius;

    [SerializeField]
    private float m_simulateTime;

    private CullingGroup m_CullingGroup;
    private ParticleSystem[] m_particleSystems;
    private Renderer[] m_ParticleRenderers;

    private void Awake()
    {
        m_particleSystems = GetComponentsInChildren<ParticleSystem>();
        m_ParticleRenderers = GetComponentsInChildren<Renderer>();

        m_CullingGroup = new CullingGroup();
        m_CullingGroup.targetCamera = Camera.main;
        m_CullingGroup.SetBoundingSpheres(new[] { new BoundingSphere(transform.position, m_cullingRatius) });
        m_CullingGroup.SetBoundingSphereCount(1);
        m_CullingGroup.onStateChanged += OnStateChanged;

        // We need to start in a culled state
        Cull(m_CullingGroup.IsVisible(0));
    }
    private void OnEnable()
    {
        m_CullingGroup.enabled = true;
    }

    private void OnDisable()
    {
        PlayParticleSystems();
        SetRenderers(true);
    }

    private void OnDestroy()
    {
        m_CullingGroup.Dispose();
    }

    private void OnStateChanged(CullingGroupEvent sphere)
    {
        Cull(sphere.isVisible);
    }

    private void Cull(bool visible)
    {
        if (visible)
        {
            PlayParticleSystems();
            SetRenderers(true);

        }
        else
        {
            StopParticleSystem();
            SetRenderers(false);
        }
    }

    private void PlayParticleSystems()
    {
        // We could simulate forward a little here to hide that the system was not updated off-screen.


        foreach (ParticleSystem particleSystem in m_particleSystems)
        {
            particleSystem.Simulate(m_simulateTime);
            particleSystem.Play(false);
        }
    }

    private void StopParticleSystem()
    {
        foreach (ParticleSystem particleSystem in m_particleSystems)
        {
            particleSystem.Stop(false);
        }
    }

    private void SetRenderers(bool enable)
    {
        // We also need to disable the renderer to prevent drawing the particles, such as when occlusion occurs.
        foreach (var particleRenderer in m_ParticleRenderers)
        {
            particleRenderer.enabled = enable;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (enabled)
        {
            Color col = Color.yellow;
            if (m_CullingGroup != null && !m_CullingGroup.IsVisible(0))
                col = Color.gray;

            Gizmos.color = col;
            Gizmos.DrawWireSphere(transform.position, m_cullingRatius);
        }
    }
}