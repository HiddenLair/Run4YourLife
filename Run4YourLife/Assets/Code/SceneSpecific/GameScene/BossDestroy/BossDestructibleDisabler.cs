using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.GameManagement.Refactor
{
    public class BossDestructibleDisabler : BossDestructibleBase
    {
        protected override void DestroyBehaviour()
        {
            gameObject.SetActive(false);
        }

        protected override void RegenerateBehaviour()
        {
            gameObject.SetActive(true);
        }
    }
}