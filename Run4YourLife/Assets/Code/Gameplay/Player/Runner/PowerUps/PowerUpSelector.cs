using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{
    public class PowerUpSelector : MonoBehaviour
    {

        #region Enumerator

        public enum Type { SINGLE,GROUP};

        #endregion

        #region Inspector

        [SerializeField]
        private Type type;

        #endregion

        #region Variables

        GameObject[] players;
        PowerUp effect;

        #endregion

        private void Awake()
        {
           effect = GetComponent<PowerUp>();
           if(type == Type.GROUP)
            {
                players = GameObject.FindGameObjectsWithTag(Tags.Player);
            } 
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (type)
            {
                case Type.SINGLE:
                    {
                        effect.Effect(other.gameObject);
                    }
                    break;
                case Type.GROUP:
                    {
                        foreach (GameObject g in players)
                        {
                            effect.Effect(g);
                        }
                    }
                    break;
            }
            Destroy(gameObject);
        }
    }
}
