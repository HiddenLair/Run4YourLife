using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Run4YourLife.Cinemachine;

namespace Run4YourLife.GameManagement {
    public class TrembleManager : SingletonMonoBehaviour<TrembleManager> {

        private CinemachineTraumaMultiple trembleInstance;


        public void Subscribe(CinemachineTraumaMultiple newInstance)
        {
            trembleInstance = newInstance;
        }

        public void Unsubscribe(CinemachineTraumaMultiple instance)
        {
            if(this.trembleInstance == instance)
            {
                instance = null;
            }
        }

        public void Tremble(TrembleConfig config)
        {
            Debug.Assert(trembleInstance != null);
            trembleInstance.AddTrauma(config);
        }
    }
}
