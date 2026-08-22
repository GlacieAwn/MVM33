#if FMOD_PRESENT
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class AudioManager : MonoBehaviour
    {
        [Header("One Shot")]
        [SerializeField] private FmodOneShotPlayer oneShotPlayer;

        [Header("Looped Music")]
        [SerializeField] private FmodMusicPlayer loopedMusicPlayer;

        [Header("Scene Start Event")]
        [SerializeField] private EventReference sceneStartEvent;
        [SerializeField] private ParamRef[] sceneStartParameters = System.Array.Empty<ParamRef>();

        public EventReference SceneStartEvent => sceneStartEvent;
        public ParamRef[] SceneStartParameters => sceneStartParameters;

        private void Start()
        {
            PlaySceneStartEvent();
        }

        public void PlayOneShot()
        {
            if (oneShotPlayer != null)
                oneShotPlayer.Play();
        }

        public void PlayLoopedMusic()
        {
            if (loopedMusicPlayer != null)
                loopedMusicPlayer.PlayLooped();
        }

        public void StopLoopedMusic()
        {
            if (loopedMusicPlayer != null)
                loopedMusicPlayer.StopCurrent();
        }

        public void PlaySceneStartEvent()
        {
            if (sceneStartEvent.IsNull)
                return;

            EventInstance instance = RuntimeManager.CreateInstance(sceneStartEvent);
            FmodParameterUtility.Apply(instance, sceneStartParameters);
            instance.start();
            instance.release();
        }
    }
#endif
