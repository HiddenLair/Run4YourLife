using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Run4YourLife.Player;
using Run4YourLife.Utils;

namespace Run4YourLife.Interactables
{
    public class ElectricField : MonoBehaviour
    {

        [SerializeField]
        private float fieldDuration;

        [SerializeField]
        private ParticleSystem[] particles;

        private float timeElapsed;

        private void OnEnable()
        {
            timeElapsed = 0;
            foreach (ParticleSystem p in particles)
            {
                p.Play();
            }
        }

        private void Update()
        {
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= fieldDuration)
            {
                Deactivate();
            }
        }

        private void Deactivate()
        {
            foreach (ParticleSystem p in particles)
            {
                p.Stop();
                StartCoroutine(YieldHelper.WaitUntil(() => gameObject.SetActive(false), () => !p.IsAlive()));
            }          
        }

        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvents.Execute<IRunnerEvents>(other.gameObject, null, (x, y) => x.Shock(fieldDuration - timeElapsed));
        }
    }
}
