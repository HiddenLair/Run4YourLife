using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Run4YourLife.OptionsMenu
{
    class VolumeSwitch : MonoBehaviour, IMoveHandler
    {
        #region Private Variables
        private int volumeLevel = 3;
        #endregion

        #region Public Variables
        public Sprite activeMusicalNote;
        public Sprite unactiveMusicalNote;
        public GameObject[] musicalNotes;
        #endregion

        public void Awake()
        {
            SetVolume(3); 
        }

        public void OnMove(AxisEventData eventData)
        {
            if (eventData.moveDir == MoveDirection.Right)
            {
                if (volumeLevel < 5)
                {
                    volumeLevel++;
                    SetVolume(volumeLevel);
                }
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                if (volumeLevel > 0)
                {
                    volumeLevel--;
                    SetVolume(volumeLevel);
                }
            }
        }

        private void SetVolume(int volumeValue)
        {
            for(int i = 0; i < volumeLevel; i++)
            {
                ActivateNote(musicalNotes[i]);               
            }

            for (int i = volumeLevel; i < musicalNotes.Length; i++)
            {
                DeactivateNote(musicalNotes[i]);
            }

            //Musical Notes could do something fancy when increased :( - (Ask Xavi and Gerard)
        }

        private void DeactivateNote(GameObject note)
        {
            note.GetComponent<Image>().sprite = unactiveMusicalNote;
        }

        private void ActivateNote(GameObject note)
        {
            note.GetComponent<Image>().sprite = activeMusicalNote;
        }
    }
}
