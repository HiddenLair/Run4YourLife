using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class VolumeSwitch : MonoBehaviour, IMoveHandler, ISelectHandler, IDeselectHandler
    {
        #region Private Variables
        private int volumeLevel = 3;
        #endregion

        #region Public Variables
        public Sprite activeMusicalNote;
        public Sprite unactiveMusicalNote;
        public GameObject[] musicalNotes;
        public GameObject leftSwitch;
        public GameObject rightSwitch;
        public AudioMixer audioMixer;
        #endregion

        public void Awake()
        {
            SetVolume(3, true); 
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

                rightSwitch.GetComponent<ScaleTick>().Tick();
            }
            else if (eventData.moveDir == MoveDirection.Left)
            {
                if (volumeLevel > 0)
                {
                    volumeLevel--;
                    SetVolume(volumeLevel);
                }

                leftSwitch.GetComponent<ScaleTick>().Tick();
            }
        }

        private void SetVolume(int volumeValue, bool ignoreNoteScaleTick = false)
        {
            for(int i = 0; i < volumeLevel; i++)
            {
                ActivateNote(musicalNotes[i]);
            }

            for (int i = volumeLevel; i < musicalNotes.Length; i++)
            {
                DeactivateNote(musicalNotes[i]);
            }

            float volumeOffset = 20.0f * (float)volumeValue;
            audioMixer.SetFloat("Volume", -80.0f + volumeOffset);

            if(!ignoreNoteScaleTick && volumeValue > 0)
            {
                musicalNotes[volumeValue - 1].GetComponent<ScaleTick>().Tick();
            }
        }

        private void DeactivateNote(GameObject note)
        {
            note.GetComponent<Image>().sprite = unactiveMusicalNote;
        }

        private void ActivateNote(GameObject note)
        {
            note.GetComponent<Image>().sprite = activeMusicalNote;
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(true);
            }

            if (rightSwitch != null)
            {
                rightSwitch.SetActive(true);
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (leftSwitch != null)
            {
                leftSwitch.SetActive(false);
            }

            if (rightSwitch != null)
            {
                rightSwitch.SetActive(false);
            }
        }
    }
}
