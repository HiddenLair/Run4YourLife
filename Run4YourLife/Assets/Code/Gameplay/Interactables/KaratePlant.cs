using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Run4YourLife.Interactables
{
    public class KaratePlant : MonoBehaviour
    {

        public enum Direction { Right,Left};

        #region Inspector

        [SerializeField]
        private Direction direction;

        [SerializeField]
        private float pushForce = 5.0f;

        [SerializeField]
        private Animator anim;

        #endregion

        private void Awake()
        {
            if(direction == Direction.Left)
            {
                pushForce = -pushForce;
            }
        }

        public void Hit()
        {
            switch (direction)
            {
                case Direction.Left:
                    {
                        anim.SetTrigger("Left");
                    }
                    break;
                case Direction.Right:
                    {
                        anim.SetTrigger("Right");
                    }
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvents.Execute<ICharacterEvents>(other.gameObject, null, (x, y) => x.Impulse(new Vector3(pushForce,0,0)));
        }
    }
}
