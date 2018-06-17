using System;
using UnityEngine;

namespace Run4YourLife.Utils
{
    [RequireComponent(typeof(Animator))]
    public class GameObjectEnablerTimelineController : MonoBehaviour
    {
        private const string ENABLE_STR = "Enable";
        private const string DISABLE_STR = "Disable";

        [Serializable]
        private struct AnimatorEventData
        {
            public int frame;
            public bool enable;
            public int gameObjectIndex;
        }

        [SerializeField]
        private float fps = 24.0f;

        [SerializeField]
        private AnimatorEventData[] animatorEventsDatas;

        [SerializeField]
        private GameObject[] gameObjects;

        private Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();

            AddEvents();
        }

        private void AddEvents()
        {
            foreach(AnimatorEventData animatorEventData in animatorEventsDatas)
            {
                AddEvent(animatorEventData.frame, animatorEventData.enable ? ENABLE_STR : DISABLE_STR, animatorEventData.gameObjectIndex);
            }
        }

        // Assumes: 1 animation, 1 clip
        void AddEvent(int frame, string functionName, int gameObjectIndex)
        {
            AnimationEvent animationEvent = new AnimationEvent();
            animationEvent.time = frame / fps;
            animationEvent.functionName = functionName;
            animationEvent.intParameter = gameObjectIndex;

            AnimationClip clip = animator.runtimeAnimatorController.animationClips[0];
            clip.AddEvent(animationEvent);
        }

        private void Enable(int gameObjectIndex)
        {
            if(gameObjectIndex < gameObjects.Length)
            {
                gameObjects[gameObjectIndex].SetActive(true);
            }
        }

        private void Disable(int gameObjectIndex)
        {
            if(gameObjectIndex < gameObjects.Length)
            {
                gameObjects[gameObjectIndex].SetActive(false);
            }
        }
    }
}