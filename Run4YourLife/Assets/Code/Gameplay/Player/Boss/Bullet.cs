using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Run4YourLife.Player
{

    public class Bullet : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" || other.tag == "Trap")
            {
                Destroy(other.gameObject);
            }
            Destroy(gameObject);
            //TODO: destroy if it goes out off screen
        }

    }
}