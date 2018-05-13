using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class VolumeSwitch : MenuEntryArrowed
    {
        [SerializeField]
        private Sprite activeMusicalNote;
        
        [SerializeField]
        public Sprite unactiveMusicalNote;

        [SerializeField]
        public GameObject[] musicalNotes;

        [SerializeField]
        public AudioMixer audioMixer;

        private int volumeLevel = 3;

        protected override void Awake()
        {
            base.Awake();
            SetVolume(3, false); 
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            switch(moveEvent)
            {
                case MoveEvent.Left:
                    if (volumeLevel > 0)
                    {
                        volumeLevel--;
                        SetVolume(volumeLevel);
                    }
                    break;
                case MoveEvent.Right:
                    if (volumeLevel < 5)
                    {
                        volumeLevel++;
                        SetVolume(volumeLevel);
                    }
                    break;
            }
        }

        private void SetVolume(int volumeValue, bool playNoteAnimation = true)
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

            if(playNoteAnimation && volumeValue > 0)
            {   
                musicalNotes[volumeValue - 1].GetComponent<PlayableDirector>().Play();
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
    }
}
