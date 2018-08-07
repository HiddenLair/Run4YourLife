using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;

using Run4YourLife.Player;
using Run4YourLife.Utils;

namespace Run4YourLife.Interactables
{
    [RequireComponent(typeof(ParticleSystem))]
    public class TornadoController : MonoBehaviour
    {

        #region Inspector
        [SerializeField]
        private float pushForce;

        [SerializeField]
        private float activationDelay;

        [SerializeField]
        private float duration;

        [SerializeField]
        private float speed;

        [SerializeField]
        private float timeToTurn;

        [SerializeField]
        private float timeBetweenDirectionChange;

        #endregion

        #region Variables

        private Vector3 actualDirector;
        private ParticleSystem particle;
        private Collider particleCollider;
        private float timer;
        private float turnTimer;

        #endregion

        private void Awake()
        {
            particle = GetComponent<ParticleSystem>();
            particleCollider = GetComponent<Collider>();
        }

        private void OnEnable()
        {
            particle.Play();
            particleCollider.enabled = false;
            timer = Time.time + duration;
            turnTimer = Time.time + timeBetweenDirectionChange;
            actualDirector = GenerateRandomDirector();
            StartCoroutine(YieldHelper.WaitForSeconds(() => DelayActivate(), activationDelay));
        }

        private void DelayActivate()
        {
            particleCollider.enabled = true;
        }

        private void Update()
        {
            if (timer < Time.time)
            {
                Stop();
            }

            Move();
            if (turnTimer < Time.time)
            {
                StartCoroutine(Turn());
                turnTimer = Time.time + timeBetweenDirectionChange;
            }
        }

        Vector3 GenerateRandomDirector()
        {
            return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0);
        }

        private void Move()
        {
            transform.position = transform.position + actualDirector * speed * Time.deltaTime;
        }

        IEnumerator Turn()
        {
            float actualLerp = 0.0f;

            Vector3 lastDirector = actualDirector;
            Vector3 newDirector = GenerateRandomDirector();

            while (actualLerp < 1)
            {
                actualLerp += Time.deltaTime / timeToTurn;
                actualDirector = Vector3.Lerp(lastDirector, newDirector, actualLerp);
                yield return null;
            }
        }

        private void Stop()
        {
            particleCollider.enabled = false;
            particle.Stop();
            StartCoroutine(YieldHelper.WaitUntil(() => Deactivate(), () => !particle.IsAlive()));
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvents.Execute<IRunnerEvents>(other.gameObject, null, (x, y) => x.Impulse(new Vector3(0, pushForce, 0)));
        }
    }
}