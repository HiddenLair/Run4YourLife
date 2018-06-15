using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class VolumeSwitch : MenuEntryArrowed
    {
        [SerializeField]
        private Sprite activeMusicalNote;
        
        [SerializeField]
        private Sprite unactiveMusicalNote;

        [SerializeField]
        private GameObject[] musicalNotes;

        private int volumeLevel = -1;

        protected override void Awake()
        {
            base.Awake();

            volumeLevel = ComputeVolumeLevel();
            ActivateNotes();
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            int previousVolumeLevel = volumeLevel;

            volumeLevel += 2 * (int)moveEvent - 1;
            volumeLevel = Mathf.Clamp(volumeLevel, 0, musicalNotes.Length);

            UpdateVolume(previousVolumeLevel != volumeLevel);
        }

        private float ComputeVolume()
        {
            return volumeLevel / (float)musicalNotes.Length;
        }

        private int ComputeVolumeLevel()
        {
            return (int)(AudioListener.volume * musicalNotes.Length);
        }

        private void UpdateVolume(bool updateNote)
        {
            ActivateNotes();

            if(updateNote && volumeLevel > 0)
            {
                musicalNotes[volumeLevel - 1].GetComponent<PlayableDirector>().Play();
            }

            AudioListener.volume = ComputeVolume();
        }

        private void ActivateNotes()
        {
            for(int i = 0; i < volumeLevel; ++i)
            {
                ActivateNote(musicalNotes[i], true);
            }

            for(int i = volumeLevel; i < musicalNotes.Length; ++i)
            {
                ActivateNote(musicalNotes[i], false);
            }
        }

        private void ActivateNote(GameObject note, bool active)
        {
            note.GetComponent<Image>().sprite = active ? activeMusicalNote : unactiveMusicalNote;
        }
    }
}