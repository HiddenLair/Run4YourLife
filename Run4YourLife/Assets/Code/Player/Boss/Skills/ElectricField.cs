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
        private ParticleSystem particles;

        private float timeElapsed;

        private void OnEnable()
        {
            timeElapsed = 0;
            particles.Play();
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
            particles.Stop();
            StartCoroutine(YieldHelper.WaitUntil(()=>gameObject.SetActive(false),()=>!particles.IsAlive()));
        }

        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvents.Execute<IRunnerEvents>(other.gameObject, null, (x, y) => x.Shock(fieldDuration - timeElapsed));
        }
    }
}
