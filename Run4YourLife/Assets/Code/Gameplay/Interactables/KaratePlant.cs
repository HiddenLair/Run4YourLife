using UnityEngine;
using UnityEngine.EventSystems;

using Run4YourLife.Player;

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
            anim.SetTrigger("Hit");
        }

        private void OnTriggerEnter(Collider other)
        {
            ExecuteEvents.Execute<ICharacterEvents>(other.gameObject, null, (x, y) => x.Impulse(new Vector3(pushForce,0,0)));
        }
    }
}
