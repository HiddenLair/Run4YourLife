using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Playables;

namespace Run4YourLife.SceneSpecific.OptionsMenu
{
    public class VolumeSwitch : MenuEntryArrowed
    {
        private static readonly float MIN_VOLUME = -80f;

        [SerializeField]
        private Sprite activeMusicalNote;
        
        [SerializeField]
        private Sprite unactiveMusicalNote;

        [SerializeField]
        private GameObject[] musicalNotes;

        [SerializeField]
        private AudioMixerGroup audioMixerGroup;

        private int nActiveMusicalNotes = 5;

        protected override void Awake()
        {
            base.Awake();

            UpdateVolume(false);
        }

        protected override void OnArrowEvent(MoveEvent moveEvent)
        {
            int previousActiveMusicalNotes = nActiveMusicalNotes;
            
            nActiveMusicalNotes = Mathf.Clamp(nActiveMusicalNotes+(int)moveEvent, 0, musicalNotes.Length);

            // this will  make the last note to not pop up when keeping increasing the volume while at max volume
            UpdateVolume(nActiveMusicalNotes != previousActiveMusicalNotes); 
        }

        private float ComputeVolume(int nActiveMusicalNotes)
        {
            if(nActiveMusicalNotes == 0)
            {
                return MIN_VOLUME;
            }

            return 30f * Mathf.Log10(((float)nActiveMusicalNotes/musicalNotes.Length));
        }

        private void UpdateVolume(bool updateNote)
        {
            UpdateMusicalNotes();

            if(updateNote && nActiveMusicalNotes > 0)
            {
                musicalNotes[nActiveMusicalNotes - 1].GetComponent<PlayableDirector>().Play();
            }

            SetVolumeToAudioMixerGroup(ComputeVolume(nActiveMusicalNotes));
        }

        private void SetVolumeToAudioMixerGroup(float volume)
        {
            string parameter = audioMixerGroup.name+"Volume";
            audioMixerGroup.audioMixer.SetFloat(parameter, volume);
        }

        private void UpdateMusicalNotes()
        {
            for(int i = 0; i < nActiveMusicalNotes; ++i)
            {
                UpdateMusicalNote(musicalNotes[i], true);
            }

            for(int i = nActiveMusicalNotes; i < musicalNotes.Length; ++i)
            {
                UpdateMusicalNote(musicalNotes[i], false);
            }
        }

        private void UpdateMusicalNote(GameObject note, bool active)
        {
            note.GetComponent<Image>().sprite = active ? activeMusicalNote : unactiveMusicalNote;
        }
    }
}